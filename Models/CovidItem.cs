namespace dashboard_api.Models
{
    public class CovidItem
    {
        public long Id { get; set; }
        public string? IsoCode { get; set; }
        public string? Location { get; set; }
        public string? Continent { get; set; }
        public DateTime Date { get; set; }
        public int TotalCases { get; set; }
        public int TotalDeaths { get; set; }
        public int NewCases { get; set; }
        public int NewDeaths { get; set; }
        public double TotalCasesPerMilion { get; set; }
        public double NewCasesPerMilion { get; set; }
        public double NewDeathsPerMilion { get; set; }
        public double TotalDeathsPerMilion { get; set; }
        public double StringencyIndex { get; set; }
    }
}