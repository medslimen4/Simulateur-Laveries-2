using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using LaverieEntities.Entities;
using Simulateur.Domain.Entities;
using Simulateur.Domain.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Simulateur.Infrastructure
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerSettings _jsonSettings;

        public DataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public async Task<LaverieData> GetLaverieDataAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("your_api_url_here");
                response.EnsureSuccessStatusCode();

                var jsonData = await response.Content.ReadAsStringAsync();
                var laverieData = JsonConvert.DeserializeObject<LaverieData>(jsonData, _jsonSettings);

                if (laverieData == null)
                {
                    throw new Exception("Failed to deserialize LaverieData from API response");
                }

                return laverieData;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching laverie data: {ex.Message}", ex);
            }
        }

        public async Task<SimulationConfig> GetSimulationConfigAsync()
        {
            var laverieData = await GetLaverieDataAsync();
            return await ConfigureRelationships(laverieData);
        }

        private async Task<SimulationConfig> ConfigureRelationships(LaverieData laverieData)
        {
            try
            {
                foreach (var laverie in laverieData.Laveries)
                {
                    laverie.machinesLaverie = laverieData.Machines
                        .Where(m => m.IDLaverie == laverie.IdLaverie)
                        .ToList();

                    foreach (var machine in laverie.machinesLaverie)
                    {
                        machine.cyclesMachine = laverieData.Cycles
                            .Where(c => c.IdMachine == machine.IdMachine)
                            .ToList();
                        machine.Laverie = laverie;
                    }

                    var proprietaire = laverieData.Proprietaires
                        .FirstOrDefault(p => p._CIN == laverie.ProprietaireCIN);

                    if (proprietaire != null)
                    {
                        laverie.Proprietaire = proprietaire;
                        proprietaire.propLaverie ??= new List<Laverie>();
                        proprietaire.propLaverie.Add(laverie);
                    }
                }

                return new SimulationConfig
                {
                    Proprietaires = laverieData.Proprietaires,
                    Laveries = laverieData.Laveries,
                    Machines = laverieData.Machines,
                    Cycles = laverieData.Cycles
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error configuring relationships: {ex.Message}", ex);
            }
        }

        public async Task SaveSimulationConfigAsync(SimulationConfig config, string filePath = null)
        {
            try
            {
                filePath ??= Path.Combine(Directory.GetCurrentDirectory(), "laverie_data.json");
                var jsonData = JsonConvert.SerializeObject(config, _jsonSettings);
                await File.WriteAllTextAsync(filePath, jsonData);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving simulation config: {ex.Message}", ex);
            }
        }
    }
}
