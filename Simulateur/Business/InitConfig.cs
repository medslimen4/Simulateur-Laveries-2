using LaverieEntities.Entities;
using Simulateur.Domain.Services;
using System.Threading.Tasks;
using Simulateur.Domain.Entities;

namespace Simulateur.Business
{
    public class InitConfig
    {
        private readonly IDataService _dataService;

        public InitConfig(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<SimulationConfig> InitializeConfigurationAsync()
        {
            return await _dataService.GetSimulationConfigAsync();
        }
    }
}