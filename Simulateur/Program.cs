using Microsoft.Extensions.DependencyInjection;
using Simulateur.Business;
using Simulateur.Domain.Services;
using System;
using System.Threading.Tasks;

namespace Simulateur.Infrastructure
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Setup DI
                var services = ConfigureServices();
                var serviceProvider = services.BuildServiceProvider();

                // Get InitConfig service
                var initConfig = serviceProvider.GetRequiredService<InitConfig>();

                Console.WriteLine("Starting laundry simulation...");
                Console.WriteLine("Initializing configuration...");

                // Initialize configuration
                var simulationConfig = await initConfig.InitializeConfigurationAsync();

                Console.WriteLine($"Configuration loaded successfully!");
                Console.WriteLine($"Loaded {simulationConfig.Laveries.Count} laundries");
                Console.WriteLine($"Loaded {simulationConfig.Machines.Count} machines");
                Console.WriteLine($"Loaded {simulationConfig.Cycles.Count} cycles");
                Console.WriteLine($"Loaded {simulationConfig.Proprietaires.Count} owners");

                // Get GereLaverie service
                var gereLaverie = serviceProvider.GetRequiredService<GereLaverie>();

                Console.WriteLine("\nPress any key to start the simulation...");
                Console.ReadKey();

                // Start simulation
                await gereLaverie.StartSimulation();

                Console.WriteLine("\nSimulation completed. Press any key to exit.");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.ResetColor();
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
            }
        }

        private static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            // Register HttpClient
            services.AddHttpClient();

            // Register services
            services.AddTransient<IDataService, DataService>();
            services.AddTransient<InitConfig>();
            services.AddTransient<GereLaverie>();

            return services;
        }
    }
}