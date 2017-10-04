using System;

namespace BDSA2017.Assignment06.DTOs
{
    public class RaceListDTO
    {
        public int Id { get; set; }

        public string TrackName { get; set; }

        public int NumberOfLaps { get; set; }

        /// <summary>
        /// ActualStart ?? PlannedStart
        /// </summary>
        public DateTime? Start { get; set; }

        /// <summary>
        /// ActualEnd ?? PlannedEnd
        /// </summary>
        public DateTime? End { get; set; }

        public int MaxCars { get; set; }

        public int NumberOfCars { get; set; }

        public string WinningCar { get; set; }

        public string WinningDriver { get; set; }

        public override bool Equals(Object obj)
        {
            var other = obj as RaceListDTO;
            return Id == (other.Id) && TrackName == other.TrackName && NumberOfLaps == other.NumberOfLaps && Start == other.Start && End == other.End && MaxCars == MaxCars && NumberOfCars == NumberOfCars &&
                WinningCar == WinningCar && WinningDriver == WinningDriver; ;
        }
    }
}
