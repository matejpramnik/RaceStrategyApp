using Microsoft.EntityFrameworkCore;

namespace RaceStrategyApp.Models {
    public class RaceStrategyContext: DbContext {

        public RaceStrategyContext() {

        }

        public DbSet<Models.PitStop> PitStop { get; set; }
        public DbSet<Models.TrackInfo> TrackInfo { get; set; }
        public DbSet<Models.Race> Races { get; set; }
        public DbSet<Models.RaceSeries> RaceSeries { get; set; }
        public DbSet<Models.RaceProgress> RaceProgresses { get; set; }
        public DbSet<Models.RaceSnapshot> RaceSnapshots { get; set; }
        

        protected override void OnModelCreating(ModelBuilder mb) {
            mb.Entity<Models.Race>().Property(p => p.Name).HasMaxLength(100);
            mb.Entity<Models.RaceProgress>().Ignore(r => r.Race);
            //mb.Entity<Models.Race>().Ignore(r => r.PitStop);
            //mb.Entity<Models.Race>().Ignore(r => r.TrackInfo);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            options.UseLazyLoadingProxies()
            .UseSqlite(@"Data Source=RaceStrategyDB.db");
        }
    }
}
