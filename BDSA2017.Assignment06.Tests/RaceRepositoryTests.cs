using BDSA2017.Assignment05.Entities;
using BDSA2017.Assignment05;
using Xunit;
using System.Linq;
using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using BDSA2017.Assignment06.Entities;
using BDSA2017.Assignment06.DTOs;

namespace BDSA2017.Assignment06.Tests
{
    public class RaceRepositoryTests : IDisposable
    {
        DesignTimeDbContextFactory contextBuilder;
        RaceRepository raceRepository;
        SlotCarContext context;

        public RaceRepositoryTests()
        {
            contextBuilder = new DesignTimeDbContextFactory();
            context = contextBuilder.CreateDbContext();
            raceRepository = new RaceRepository(context);

        }
        [Fact]
        public async void TestUpdateCarInRaceAsync()
        {
            Car car = new Car() { Driver = "Mads", Name = "Suzuki" };
      
            var track = new Track()
            {
                BestTime = 121213123,
                LengthInMeters = 123214,
                MaxCars = 50,
                Name = "RaceTrack"
            };
            var race = new Race()
            {

                NumberOfLaps = 5,
                PlannedEnd = new DateTime(1920, 11, 11),
                PlannedStart = new DateTime(1920, 11, 11),
                Track = track
            };
            var carInRace = new CarInRace() { Car = car, Race = race };
            context.Add(carInRace);
            context.SaveChanges();
            var UpdatedCarInRaceInfo = new RaceCarDTO()
            {
                CarId = car.Id,
                RaceId = race.Id,
                EndPosition = 2,
                FastestLap = 123123,
                StartPosition = 1212,
                TotalTime = 2121212

            };
            await raceRepository.UpdateCarInRaceAsync(UpdatedCarInRaceInfo);
            Assert.Equal(UpdatedCarInRaceInfo.EndPosition,context.CarsInRace.Find(carInRace.RaceId,carInRace.CarId).EndPosition);
            Assert.Equal(UpdatedCarInRaceInfo.FastestLap, context.CarsInRace.Find(carInRace.RaceId, carInRace.CarId).FastestLap);
            Assert.Equal(UpdatedCarInRaceInfo.TotalTime, context.CarsInRace.Find(carInRace.RaceId, carInRace.CarId).TotalTime);
            Assert.Equal(UpdatedCarInRaceInfo.StartPosition, context.CarsInRace.Find(carInRace.RaceId, carInRace.CarId).StartPosition);

        }
        [Fact]
        public void TestUpdate()
        {
            var track = new Track()
            {
                BestTime = 121213123,
                LengthInMeters = 123214,
                MaxCars = 50,
                Name = "RaceTrack"
            };
            var race = new Race()
            {

                NumberOfLaps = 5,
                PlannedEnd = new DateTime(1920, 11, 11),
                PlannedStart = new DateTime(1920, 11, 11),
                Track = track
            };
            var trackupdated = new Track()
            {
                BestTime = 12121312321121,
                LengthInMeters = 1232142112,
                MaxCars = 40,
                Name = "RaceTrackupdated"
            };
            context.Add(race);
            context.Add(trackupdated);
            context.SaveChanges();
            var RaceCreate = new RaceCreateDTO()
            {
                Id = race.Id,
                NumberOfLaps = 5,
                PlannedEnd = new DateTime(1920, 11, 11),
                PlannedStart = new DateTime(1920, 11, 11),
                TrackId = trackupdated.Id,
                ActualEnd = new DateTime(1221, 1, 1),
                ActualStart = new DateTime(2412, 2, 2)
            };
            raceRepository.Update(RaceCreate);
            Assert.Equal(context.Races.Find(race.Id).Track, trackupdated);
        }
        [Fact]
        public void TestUpdateRaceNotFound()
        {        
            var race = new Race()
            {
                NumberOfLaps = 5,
                PlannedEnd = new DateTime(1920, 11, 11),
                PlannedStart = new DateTime(1920, 11, 11),           
            };
           
            var RaceCreate = new RaceCreateDTO()
            {
                Id = race.Id,
                NumberOfLaps = 5,
                PlannedEnd = new DateTime(1920, 11, 11),
                PlannedStart = new DateTime(1920, 11, 1),
                ActualEnd = new DateTime(1221, 1, 1),
                ActualStart = new DateTime(2412, 2, 2)
            };
            
            Assert.Equal((false, "no race found"), raceRepository.Update(RaceCreate));
        }


        [Fact]
        public void TestRead()
        {
            var track = new Track()
            {
                BestTime = 121213123,
                LengthInMeters = 123214,
                MaxCars = 50,
                Name = "RaceTrack"
            };
            var race = new Race()
            {

                NumberOfLaps = 5,
                PlannedEnd = new DateTime(1920, 11, 11),
                PlannedStart = new DateTime(1920, 11, 11),
                Track = track
            };
            context.Add(race);
            context.SaveChanges();
            var raceCreate = new RaceCreateDTO()
            {
                PlannedEnd = race.PlannedEnd,
                PlannedStart = race.PlannedStart,
                Id = race.Id,
                ActualEnd = race.ActualEnd,
                ActualStart = race.ActualStart,
                NumberOfLaps = race.NumberOfLaps,
                TrackId = race.Track.Id
            };

            Assert.Equal( raceCreate, raceRepository.Read(race.Id));

        }
        [Fact] 
        public void TestReadList()
        {
            Car car = new Car() { Driver = "Mads", Name = "Suzuki" };
            Car car1 = new Car() { Driver = "Mads2", Name = "Suzuki" };
            Car car2 = new Car() { Driver = "Mads3", Name = "Suzuki" };

            var track = new Track()
            {
                BestTime = 121213123,
                LengthInMeters = 123214,
                MaxCars = 50,
                Name = "RaceTrack"
            };
            var race = new Race()
            {

                NumberOfLaps = 5,
                PlannedEnd = new DateTime(1920, 11, 11),
                PlannedStart = new DateTime(1920, 11, 11),
                Track = track
            };
            var carInRace = new CarInRace() { Car = car, Race = race, TotalTime = 120210 };
            var carInRace2 = new CarInRace() { Car = car1, Race = race, TotalTime = 21314212421 };
            var carInRace3 = new CarInRace() { Car = car2, Race = race, TotalTime = 214124214141 };
            context.Add(carInRace);
            context.Add(carInRace2);
            context.Add(carInRace3);
            context.SaveChanges();
            var raceList = new RaceListDTO()
            {
                End = race.PlannedEnd,
                Start = race.PlannedStart,
                Id = race.Id,
                MaxCars = race.Track.MaxCars,
                NumberOfCars = 3,
                NumberOfLaps = race.NumberOfLaps,
                TrackName = race.Track.Name,
                WinningCar = car.Name,
                WinningDriver = car.Driver
            };

            Assert.Equal(new List<RaceListDTO> { raceList }, raceRepository.Read());

        }

        [Fact]
        public void TestRemoveCarFromRace()
        {
            using (raceRepository)
            {
                Car car = new Car() { Driver = "Mads", Name = "Suzuki" };
                var track = new Track()
                {
                    BestTime = 121213123,
                    LengthInMeters = 123214,
                    MaxCars = 50,
                    Name = "RaceTrack"
                };
                var race = new Race()
                {

                    NumberOfLaps = 5,
                    PlannedEnd = new DateTime(1920, 11, 11),
                    PlannedStart = new DateTime(1920, 11, 11),
                    Track = track
                };
                var carInRace = new CarInRace() { Car = car, Race = race };
                context.Add(carInRace);
                context.SaveChanges();
                raceRepository.RemoveCarFromRace(car.Id, race.Id);
                
                var test = context.CarsInRace.Find(carInRace.CarId, carInRace.RaceId);

                Assert.Null(context.CarsInRace.Find(carInRace.CarId,carInRace.RaceId));

            }
        }
        [Fact]
        public void TestRemoveCarFromRaceReturnsCarDosntExist()
        {
            using (raceRepository)
            {
                Car car = new Car() { Driver = "Mads", Name = "Suzuki" };
                var track = new Track()
                {
                    BestTime = 121213123,
                    LengthInMeters = 123214,
                    MaxCars = 50,
                    Name = "RaceTrack"
                };
                var race = new Race()
                {

                    NumberOfLaps = 5,
                    PlannedEnd = new DateTime(1920, 11, 11),
                    PlannedStart = new DateTime(1920, 11, 11),
                    Track = track
                };
                context.Add(race);
                context.Add(car);

                Assert.Equal((false, "The choosen car was not in the choosen race"), raceRepository.RemoveCarFromRace(car.Id, race.Id));

            }
        }
        [Fact]
        public void TestAddCarToRace()
        {
            using (raceRepository)
            {
                Car car = new Car() { Driver = "Mads", Name = "Suzuki" };
                var track = new Track()
                {
                    BestTime = 121213123,
                    LengthInMeters = 123214,
                    MaxCars = 50,
                    Name = "RaceTrack"
                };
                var race = new Race()
                {
                    
                    NumberOfLaps = 5,
                    PlannedEnd = new DateTime(1920, 11, 11),
                    PlannedStart = new DateTime(1920, 11, 11),
                    Track = track
                };
               
                context.Cars.Add(car);
                context.Races.Add(race);
                context.SaveChanges();
                raceRepository.AddCarToRace(car.Id, race.Id, 5);
                var carInRace = (from carInRaces in context.CarsInRace
                                where carInRaces.CarId == car.Id && carInRaces.RaceId == race.Id
                                select carInRaces).Count();
                Assert.True(carInRace>0);

            }
        }
        [Fact]
        public void TestAddCarToRaceFalseRaceHasStarted()
        {
            using (raceRepository)
            {
                Car car = new Car() { Driver = "Mads", Name = "Suzuki" };
                var track = new Track()
                {
                    BestTime = 121213123,
                    LengthInMeters = 123214,
                    MaxCars = 50,
                    Name = "RaceTrack"
                };
                var race = new Race()
                {
                   
                    NumberOfLaps = 5,
                    ActualStart = new DateTime(1231,04,11),
                    PlannedEnd = new DateTime(1920, 11, 11),
                    PlannedStart = new DateTime(1920, 11, 11),
                    Track = track
                };
                context.Cars.Add(car);
                context.Races.Add(race);
                context.SaveChanges();
                
              
                Assert.Equal((false, "Race or car not excisting or has started"), raceRepository.AddCarToRace(car.Id, race.Id, 5));

            }
        }
        [Fact]
        public void TestCreateRace()
        {
              using (raceRepository)
            {
                var track = new Track()
                {
                    BestTime = 121213123,
                    LengthInMeters = 123214,
                    MaxCars = 50,
                    Name = "RaceTrack"
                };
                context.Add(track);
                context.SaveChanges();
                var raceDTO = new RaceCreateDTO()
                {
                    ActualEnd = new DateTime(1920, 11, 11),
                    ActualStart = new DateTime(1920, 11, 11),
                    NumberOfLaps = 5,
                    PlannedEnd = new DateTime(1920, 11, 11),
                    PlannedStart = new DateTime(1920, 11, 11),
                    TrackId = track.Id
                };
                
                  Assert.NotNull(context.Races.Find(raceRepository.Create(raceDTO)));
               

            }
             
        }
        [Fact]
        public void TestCreateRaceFailsStarted()
        {
            using (raceRepository)
            {
                var track = new Track()
                {
                    BestTime = 121213123,
                    LengthInMeters = 123214,
                    MaxCars = 50,
                    Name = "RaceTrack"
                };
                context.Add(track);
                context.SaveChanges();
                RaceCreateDTO raceDTO = null;

                Assert.Equal(0,raceRepository.Create(raceDTO));


            }

        }
        [Fact]
        public void TestDeleteRace()
        {
            
            using (raceRepository)
            {
                
                var track = new Track()
                {
                    BestTime = 121213123,
                    LengthInMeters = 123214,
                    MaxCars = 50,
                    Name = "RaceTrack"
                };
                var race = new Race()
                {
                    ActualEnd = new DateTime(1920, 11, 11),
                    ActualStart = new DateTime(1920, 11, 11),
                    NumberOfLaps = 5,
                    PlannedEnd = new DateTime(1920, 11, 11),
                    PlannedStart = new DateTime(1920, 11, 11),
                    Track = track
                };
                context.Tracks.Add(track);
                context.Races.Add(race);
                context.SaveChanges();
                Assert.False(raceRepository.Delete(race.Id).ok);

            }
           

        }
        [Fact]
        public void TestDeleteRace2()
        {

            
            using (raceRepository)
            {

                var track = new Track()
                {
                    BestTime = 121213123,
                    LengthInMeters = 123214,
                    MaxCars = 50,
                    Name = "RaceTrack"
                };
                var race = new Race()
                {
                    NumberOfLaps = 5,
                    PlannedEnd = new DateTime(1920, 11, 11),
                    PlannedStart = new DateTime(1920, 11, 11),
                    Track = track,
                };
                context.Tracks.Add(track);
                context.Races.Add(race);
                context.SaveChanges();
                Assert.True(raceRepository.Delete(race.Id).ok);

            }


        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}