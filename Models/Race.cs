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

        private ICollection<Tyre> _Tyres;
        public ICollection<Tyre> AvailableTyres { get; set; }
        public trackState TrackState { get; set; }
        public bool Damage { get; set; }
        public bool TerminalDamage { get; set; }
        public weather TrackWeather { get; set; }
        public int RaceSeriesID { get; set; } // cudzi kluc

    }
}
