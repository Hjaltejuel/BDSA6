using BDSA2017.Assignment06.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BDSA2017.Assignment06.Repositories
{
    public interface IRaceRepository : IDisposable
    {
        Task<int> CreateAsync(RaceCreateDTO race);
        Task<IEnumerable<RaceListDTO>> ReadAsync();
        Task<RaceCreateDTO> ReadAsync(int raceId);
        Task<(bool ok, string error)> UpdateAsync(RaceCreateDTO race);
        Task<(bool ok, string error)> AddCarToRaceAsync(int carId, int raceId, int? startPosition = null);
        Task<(bool ok, string error)> UpdateCarInRaceAsync(RaceCarDTO car);
        Task<(bool ok, string error)> RemoveCarFromRaceAsync(int carId, int raceId);
        Task<(bool ok, string error)> DeleteAsync(int raceId);
    }
}
