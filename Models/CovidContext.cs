using Microsoft.EntityFrameworkCode;

namespace dashboard_api.Models
{
    public class CovidContext : DbContext
    {
        public CovidContext(DbContextOptions<CovidContext> options)
            : base(options)
        {
        }

        public DbSet<CovidItem> CovidItems { get; set; }
    }
}