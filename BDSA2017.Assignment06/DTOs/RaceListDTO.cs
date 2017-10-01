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
    }
}
