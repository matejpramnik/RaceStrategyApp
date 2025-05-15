using Microsoft.EntityFrameworkCore;

namespace RaceStrategyApp.Models {
    public class RaceStrategyContext: DbContext {

        public RaceStrategyContext() {

        }


        protected override void OnModelCreating(ModelBuilder mb) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            options.UseLazyLoadingProxies()
            .UseSqlite(@"Data Source=RaceStrategyDB.db");
        }
    }
}
