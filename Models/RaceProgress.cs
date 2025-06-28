namespace RaceStrategyApp.Models {
    public class RaceProgress {
        public int Id { get; set; }
        public int RaceId { get; set; }
        public virtual ICollection<Race> RaceSnapshots { get; set; } = new List<Race>();
    }
}
