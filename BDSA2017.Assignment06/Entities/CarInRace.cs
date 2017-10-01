using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDSA2017.Assignment06.Entities
{
    public class CarInRace
    {
        [Key]
        public int CarId { get; set; }

        [Key]
        public int RaceId { get; set; }

        public Car Car { get; set; }

        public Race Race { get; set; }

        public int? StartPosition { get; set; }

        public int? EndPosition { get; set; }

        public long? FastestLap { get; set; }

        public long? TotalTime { get; set; }
    }
}