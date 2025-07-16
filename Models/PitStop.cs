using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceStrategyApp.Models {
    public class PitStop {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int RaceId { get; set; }
        public int MandatoryStops { get; set; }
        public int NumberOfStops { get; set; }
        public bool Refueling { get; set; }
        public int LastRefuelLap { get; set; }
        public List<TyreCompound> SelectedTyres { get; set; } = new();
        public TyreCompound CurrentTyre { get; set; }
    }
}
