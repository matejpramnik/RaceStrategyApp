using Microsoft.AspNetCore.Mvc;
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
        public int MandatoryStops { get; set; }
        public int NumberOfStops { get; set; }
        public bool Refueling { get; set; }
        public int LastRefuelLap { get; set; }
        public int Position { get; set; }
        public int AmountOfOpponents { get; set; }
        public List<TyreCompound> SelectedTyres { get; set; } = new();
        public TyreCompound CurrentTyre { get; set; }
        public TrackState TrackState { get; set; }
        public bool Damage { get; set; }
        public bool TerminalDamage { get; set; }
        public Weather TrackWeather { get; set; }
        public virtual int RaceSeriesId { get; set; } // cudzi kluc

    }
}
