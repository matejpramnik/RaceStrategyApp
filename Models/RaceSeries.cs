namespace RaceStrategyApp.Models {
    public class RaceSeries {
        private static int globalRSId;
        public int Id { get; set; } = Interlocked.Increment(ref globalRSId);
        public string Name { get; set; }
        public int ParticipantCount { get; set; }

        private ICollection<Race> _Races;
        public virtual ICollection<Race> Races {
            get { return _Races ?? (_Races = new HashSet<Race>()); }
            set { _Races = value; }
        }
    }
}
