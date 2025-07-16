using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceStrategyApp.Models {
    public class Race {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfLaps { get; set; }
        public int LapCount { get; set; }
        public int Position { get; set; }
        public int AmountOfOpponents { get; set; }
        public bool Damage { get; set; }
        public bool TerminalDamage { get; set; }
        public virtual PitStop PitStop { get; set; } = new();
        public virtual TrackInfo TrackInfo { get; set; } = new();
        public virtual int RaceSeriesId { get; set; }
    }
}
