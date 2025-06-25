using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceStrategyApp.Models {
    public class Race {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfLaps { get; set; }
        public int LapCount { get; set; }
        public int MandatoryStops { get; set; }
        public bool Refueling { get; set; }
        public int Position { get; set; }
        public int AmountOfOpponents { get; set; }
        public virtual ICollection<Tyre> AvailableTyres { get; set; } = new List<Tyre>();
        public List<tyreCompound> SelectedTyres { get; set; } = new();
        public trackState TrackState { get; set; }
        public bool Damage { get; set; }
        public bool TerminalDamage { get; set; }
        public weather TrackWeather { get; set; }
        public virtual int RaceSeriesId { get; set; } // cudzi kluc

    }
}
