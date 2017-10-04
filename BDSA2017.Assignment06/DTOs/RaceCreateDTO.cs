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

        public override bool Equals(object obj)
        {
            var other = obj as RaceCreateDTO;

            return Id.Equals(other.Id) && TrackId.Equals(other.TrackId) && NumberOfLaps.Equals(other.NumberOfLaps) && PlannedStart.Equals(other.PlannedStart) && ActualStart.Equals(other.ActualStart)
                && PlannedEnd.Equals(other.PlannedEnd) && ActualEnd.Equals(other.ActualEnd);
        }
    }
}
