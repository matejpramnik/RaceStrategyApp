using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceStrategyApp.Models {
    public class RaceProgress {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int RaceId { get; set; }
        public required virtual Race Race { get; set; }
        public required virtual RaceSnapshot RaceSnapshot { get; set; } = new() { ChangeName = "", Change = "" };
    }

    public class RaceSnapshot {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int LapCount { get; set; }
        public required string ChangeName { get; set; }
        public required string Change { get; set; }
    }
}
