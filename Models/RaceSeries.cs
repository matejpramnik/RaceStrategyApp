namespace RaceStrategyApp.Models {
    public class RaceSeries {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParticipantCount { get; set; }

        private ICollection<Race> _Races;
        public virtual ICollection<Race> Races {
            get { return _Races ?? (_Races = new HashSet<Race>()); }
            set { _Races = value; }
        }
    }
}
