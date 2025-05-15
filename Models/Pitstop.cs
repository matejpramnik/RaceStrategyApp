using Microsoft.Identity.Client;

namespace RaceStrategyApp.Models {
    public class Pitstop {
        public int Id { get; set; }
        public int Lap { get; set; }
        public Tyre OldTyre { get; set; }
        public Tyre NewTyre { get; set; }
    }
}
