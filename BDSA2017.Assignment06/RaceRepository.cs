
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


        public Task<(bool ok, string error)> AddCarToRaceAsync(int carId, int raceId, int? startPosition)
        {
            return Task.Run<(bool, string)>(() =>
            {
                return AddCarToRace(carId, raceId, startPosition);
            });
        }

        public Task<int> CreateAsync(RaceCreateDTO race)
        {
            return Task.Run<int>(() =>
            {
                return Create(race);
            });
        }

        public Task<(bool ok, string error)> DeleteAsync(int raceId)
        {
            return Task.Run<(bool, string)>(() =>
            {
                return Delete(raceId);
            });
        }

        public Task<IEnumerable<RaceListDTO>> ReadAsync()
        {
            return Task.Run<IEnumerable<RaceListDTO>>(() =>
            {
                return Read();
            });
        }

        public Task<RaceCreateDTO> ReadAsync(int raceId)
        {
            return Task.Run<RaceCreateDTO>(() =>
            {
                return Read(raceId);
            });
        }

        public Task<(bool ok, string error)> RemoveCarFromRaceAsync(int carId, int raceId)
        {
            return Task.Run<(bool, string)>(() =>
            {
                return RemoveCarFromRace(carId, raceId);
            });
        }

        public Task<(bool ok, string error)> UpdateAsync(RaceCreateDTO race)
        {
            return Task.Run<(bool, string)>(() =>
            {
                return Update(race);
            });
        }

        public Task<(bool ok, string error)> UpdateCarInRaceAsync(RaceCarDTO car)
        {
            return Task.Run<(bool, string)>(() =>
            {
                return UpdateCarInRace(car);
            });
        }

        public (bool ok, string error) AddCarToRace(int carId, int raceId, int? startPosition = null)
        {
            Race race = context.Races.Find(raceId);
            Car car = context.Cars.Find(carId);
            if (race != null && car != null && race.ActualStart == null)
            {
                var CarInRacesNumber = (from a in context.CarsInRace
                                        where a.Race == race
                                        select a).Count();

                if (CarInRacesNumber < race.Track.MaxCars)
                {
                    context.CarsInRace.Add(new CarInRace() { Car = context.Cars.Find(carId), Race = context.Races.Find(raceId), StartPosition = startPosition });
                    context.SaveChanges();
                    return (true, "");
                }
                return (false, "Too many cars in race");

            }
            return (false, "Race or car not excisting or has started");

        }

        public int Create(RaceCreateDTO race)
        {
            if (race != null)
            {
                Track track = context.Tracks.Find(race.TrackId);
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
                    context.Races.Add(createdRace);
                    context.SaveChanges();
                    return createdRace.Id;
                }
                return 0;
            }
            return 0;


        }

        public (bool ok, string error) Delete(int raceId)
        {
            Race race = context.Races.Find(raceId);
            if (race?.ActualStart == null)
            {
                context.Remove(race);
                context.SaveChanges();
                return (true, ""); ;

            }
            return (false, "Race was not found or hasnt started yet");


        }

        public void Dispose()
        {
            context.Dispose();
        }

        public IEnumerable<RaceListDTO> Read()
        {
            foreach (Race race in context.Races)
            {

                var carInRaces = from carInRace in context.CarsInRace
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
                yield return raceList;
            }
        }

        public RaceCreateDTO Read(int raceId)
        {
            Race race = context.Races.Find(raceId);
            
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

        public (bool ok, string error) RemoveCarFromRace(int carId, int raceId)
        {
            Race race = context.Races.Find(raceId);
            Car car = context.Cars.Find(carId);
            if (race != null && race.ActualStart == null && car != null)
            {
                CarInRace TestForCarInRace = (from carInRaces in context.CarsInRace
                                              where carInRaces.RaceId == raceId && carInRaces.CarId == carId
                                              select carInRaces).FirstOrDefault();
                if (TestForCarInRace != null)
                {
                    context.CarsInRace.Remove(TestForCarInRace);
                    context.SaveChanges();
                    return (true, "");
                }
                return (false, "The choosen car was not in the choosen race");

            }
            return (false, "Race not excisting or has started");

        }

        public (bool ok, string error) Update(RaceCreateDTO race)
        {
            Race choosen = (from races in context.Races
                            where races.Id == race.Id
                            select races).FirstOrDefault();
            if (choosen != null)
            {
                choosen.ActualStart = race.ActualStart;
                choosen.ActualEnd = race.ActualEnd;
                choosen.PlannedEnd = race.PlannedEnd;
                choosen.PlannedStart = race.PlannedStart;
                choosen.TrackId = race.TrackId;
                choosen.NumberOfLaps = race.NumberOfLaps;
                context.SaveChanges();
                return (true, "");
            }
            return (false, "no race found");

        }

        public (bool ok, string error) UpdateCarInRace(RaceCarDTO car)
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
                context.SaveChanges();
                return (true, "");
            }
            return (false, "no Car In Race found");
        }

    }
}