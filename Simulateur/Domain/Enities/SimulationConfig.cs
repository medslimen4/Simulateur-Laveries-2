using LaverieEntities.Entities;



namespace Simulateur.Domain.Entities
{
    public class SimulationConfig
    {
        public List<Proprietaire> Proprietaires { get; set; }
        public List<Laveries> Laveries { get; set; }
        public List<Machine> Machines { get; set; }
        public List<Cycle> Cycles { get; set; }

        public SimulationConfig()
        {
            Proprietaires = new List<Proprietaire>();
            Laveries = new List<Laveries>();
            Machines = new List<Machine>();
            Cycles = new List<Cycle>();
        }
    }
}