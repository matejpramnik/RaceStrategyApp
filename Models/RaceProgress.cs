using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceStrategyApp.Models {
    public class RaceProgress {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int RaceId { get; set; }
        public virtual ICollection<Race> RaceSnapshots { get; set; } = new List<Race>();
    }
}
