
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using BDSA2017.Assignment06.Repositories;
using BDSA2017.Assignment06.Entities;
using BDSA2017.Assignment06.DTOs;

using System.Threading.Tasks;

namespace BDSA2017.Assignment06
{
    public class RaceRepository : IRaceRepository
    {
        readonly SlotCarContext context;

        public RaceRepository(SlotCarContext context)
        {
            this.context = context;
        }


        public async Task<(bool ok, string error)> AddCarToRaceAsync(int carId, int raceId, int? startPosition)
        {
         
                Race race = await context.Races.FindAsync(raceId);
                Car car = await context.Cars.FindAsync(carId);
                if (race != null && car != null && race.ActualStart == null)
                {
                    var CarInRacesNumber = (from a in context.CarsInRace.AsParallel()
                                            where a.Race == race
                                            select a).Count();

                    if (CarInRacesNumber < race.Track.MaxCars)
                    {
                        await context.CarsInRace.AddAsync(new CarInRace() { Car = await context.Cars.FindAsync(carId), Race = await context.Races.FindAsync(raceId), StartPosition = startPosition });
                        await context.SaveChangesAsync();
                        return (true, "");
                    }
                    return (false, "Too many cars in race");

                }
                return (false, "Race or car not excisting or has started");
          
        }

        public async Task<int> CreateAsync(RaceCreateDTO race)
        {
           
                if (race != null)
                {
                    Track track = await context.Tracks.FindAsync(race.TrackId);
                    if (track != null)
                    {
                        Race createdRace = new Race()
                        {
                            ActualEnd = race.ActualEnd,
                            ActualStart = race.ActualStart,
                            NumberOfLaps = race.NumberOfLaps,
                            PlannedEnd = race.PlannedEnd,
                            PlannedStart = race.PlannedStart,
                            Track = track
                        };
                        await context.Races.AddAsync(createdRace);
                        await context.SaveChangesAsync();
                        return createdRace.Id;
                    }
                    return 0;
                }
                return 0;
            
        }

        public async Task<(bool ok, string error)> DeleteAsync(int raceId)
        {
            
                Race race = await context.Races.FindAsync(raceId);
                if (race?.ActualStart == null)
                {
                    context.Remove(race);
                    await context.SaveChangesAsync();
                    return (true, ""); ;

                }
                return (false, "Race was not found or hasnt started yet");

        }

        public async Task<IEnumerable<RaceListDTO>> ReadAsync()
        {
           
                List<RaceListDTO> raceListList = new List<RaceListDTO>();
                foreach (Race race in context.Races)
                {

                    var carInRaces = from carInRace in context.CarsInRace.AsParallel()
                                     where carInRace.Race == race
                                     orderby carInRace.TotalTime
                                     select carInRace;

                    RaceListDTO raceList = new RaceListDTO
                    {
                        Id = race.Id,
                        End = race.ActualEnd ?? race.PlannedEnd,
                        Start = race.ActualStart ?? race.PlannedStart,
                        TrackName = race.Track.Name,
                        MaxCars = race.Track.MaxCars,
                        NumberOfLaps = race.NumberOfLaps,
                        NumberOfCars = carInRaces.Count(),
                        WinningCar = carInRaces.FirstOrDefault().Car.Name,
                        WinningDriver = carInRaces.FirstOrDefault().Car.Driver
                    };
                    raceListList.Add(raceList);
                }
                return raceListList;
           
        }

        public async Task<RaceCreateDTO> ReadAsync(int raceId)
        {
           
                Race race = await context.Races.FindAsync(raceId);

                RaceCreateDTO raceList = new RaceCreateDTO
                {
                    ActualEnd = race.ActualEnd,
                    ActualStart = race.ActualStart,
                    Id = race.Id,
                    NumberOfLaps = race.NumberOfLaps,
                    PlannedEnd = race.PlannedEnd,
                    PlannedStart = race.PlannedStart,
                    TrackId = race.TrackId

                };
                return raceList;
           
        }

        public async Task<(bool ok, string error)> RemoveCarFromRaceAsync(int carId, int raceId)
        {
           
                Race race = await context.Races.FindAsync(raceId);
                Car car = await context.Cars.FindAsync(carId);
                if (race != null && race.ActualStart == null && car != null)
                {
                CarInRace TestForCarInRace = await context.CarsInRace.FindAsync(raceId, carId);
                    if (TestForCarInRace != null)
                    {
                        context.CarsInRace.Remove(TestForCarInRace);
                        await context.SaveChangesAsync();
                        return (true, "");
                    }
                    return (false, "The choosen car was not in the choosen race");

                }
                return (false, "Race not excisting or has started");

           
        }

        public async Task<(bool ok, string error)> UpdateAsync(RaceCreateDTO race)
        {

            Race choosen = await context.Races.FindAsync(race.Id);
                if (choosen != null)
                {
                    choosen.ActualStart = race.ActualStart;
                    choosen.ActualEnd = race.ActualEnd;
                    choosen.PlannedEnd = race.PlannedEnd;
                    choosen.PlannedStart = race.PlannedStart;
                    choosen.TrackId = race.TrackId;
                    choosen.NumberOfLaps = race.NumberOfLaps;
                    await context.SaveChangesAsync();
                    return (true, "");
                }
                return (false, "no race found");

        }

        public async Task<(bool ok, string error)> UpdateCarInRaceAsync(RaceCarDTO car)
        {
            
                CarInRace toBeUpdated = (from races in context.CarsInRace
                                         where races.CarId == car.CarId && races.RaceId == car.RaceId
                                         select races).FirstOrDefault();
                if (toBeUpdated != null)
                {
                    toBeUpdated.FastestLap = car.FastestLap;
                    toBeUpdated.EndPosition = car.EndPosition;
                    toBeUpdated.CarId = car.CarId;
                    toBeUpdated.RaceId = car.RaceId;
                    toBeUpdated.StartPosition = car.StartPosition;
                    toBeUpdated.TotalTime = car.TotalTime;
                    await context.SaveChangesAsync();
                    return (true, "");
                }
                return (false, "no Car In Race found");

        }



        public void Dispose()
        {
            context.Dispose();
        }




    }
}
