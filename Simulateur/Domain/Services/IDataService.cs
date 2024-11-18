using LaverieEntities.Entities;
using Simulateur.Domain.Entities;
using System.Threading.Tasks;

namespace Simulateur.Domain.Services
{
    public interface IDataService
    {
        Task<LaverieData> GetLaverieDataAsync();
        Task<SimulationConfig> GetSimulationConfigAsync();
        Task SaveSimulationConfigAsync(SimulationConfig config, string filePath = null);
    }
}