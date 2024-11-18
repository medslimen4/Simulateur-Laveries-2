using LaverieEntities.Entities;
using Simulateur.Domain.Entities;
using Simulateur.Domain.Services;
namespace Simulateur.Business
{
    public class GereLaverie
    {
        private readonly IDataService _dataService;
        private readonly SimulationConfig _simulationConfig;

        public GereLaverie(IDataService dataService, SimulationConfig simulationConfig)
        {
            _dataService = dataService;
            _simulationConfig = simulationConfig;
        }

        public async Task StartSimulation()
        {
            try
            {
                Console.WriteLine("Starting simulation process...\n");

                // Example simulation steps
                foreach (var laverie in _simulationConfig.Laveries)
                {
                    Console.WriteLine($"Processing laundry ID: {laverie.IdLaverie}");
                    Console.WriteLine($"Owner CIN: {laverie.ProprietaireCIN}");
                    Console.WriteLine($"Number of machines: {laverie.machinesLaverie.Count}");

                    foreach (var machine in laverie.machinesLaverie)
                    {
                        Console.WriteLine($"\tMachine ID: {machine.IdMachine}");
                        Console.WriteLine($"\tAvailable cycles: {machine.cyclesMachine.Count}");

                        await SimulateMachineOperation(machine);
                    }

                    Console.WriteLine();
                }

                await _dataService.SaveSimulationConfigAsync(_simulationConfig);
                Console.WriteLine("Simulation completed successfully!");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error during simulation: {ex.Message}");
                Console.ResetColor();
                throw;
            }
        }

        private async Task SimulateMachineOperation(Machine machine)
        {
            foreach (var cycle in machine.cyclesMachine)
            {
                Console.WriteLine($"\t\tSimulating cycle {cycle.IdCycle}...");

                // Simulate cycle duration
                await Task.Delay(1000); 

                Console.WriteLine($"\t\tCycle {cycle.IdCycle} completed");
            }
        }
    }
}