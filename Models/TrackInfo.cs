using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceStrategyApp.Models {
    public class TrackInfo {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int RaceId { get; set; }
        public Weather TrackWeather { get; set; }
        public TrackState TrackState { get; set; }
    }
}
