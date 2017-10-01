namespace BDSA2017.Assignment06.DTOs
{
    public class RaceCarDTO
    {
        public int CarId { get; set; }
        public int RaceId { get; set; }
        public int? StartPosition { get; set; }
        public int? EndPosition { get; set; }
        public long? FastestLap { get; set; }
        public long? TotalTime { get; set; }
    }
}