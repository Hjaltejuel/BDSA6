using System;

namespace BDSA2017.Assignment06.DTOs
{
    public class RaceCreateDTO
    {
        public int Id { get; set; }

        public int TrackId { get; set; }

        public int NumberOfLaps { get; set; }

        public DateTime? PlannedStart { get; set; }

        public DateTime? ActualStart { get; set; }

        public DateTime? PlannedEnd { get; set; }

        public DateTime? ActualEnd { get; set; }
    }
}
