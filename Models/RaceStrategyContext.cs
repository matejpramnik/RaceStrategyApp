using Microsoft.EntityFrameworkCore;

namespace RaceStrategyApp.Models {
    public class RaceStrategyContext: DbContext {

        public RaceStrategyContext() {

        }

        public DbSet<Models.Race> Races { get; set; }
        public DbSet<Models.RaceSeries> RaceSeries { get; set; }
        public DbSet<Models.RaceProgress> RaceProgresses { get; set; }

        protected override void OnModelCreating(ModelBuilder mb) {
            mb.Entity<Models.Race>().Property(p => p.Name).HasMaxLength(100);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            options.UseLazyLoadingProxies()
            .UseSqlite(@"Data Source=RaceStrategyDB.db");
        }
    }
}
