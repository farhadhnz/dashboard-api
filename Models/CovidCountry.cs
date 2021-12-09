namespace dashboard_api.Models
{
    public class CovidCountry
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public int? Population { get; set; }
        public double? PopulationDensity { get; set; }
        public double? GDP { get; set; }
        public double? HumanDevelopmentIndex { get; set; }
        public double? LifeExpectancy { get; set; }
    }
}