using Xunit;
using System.Linq;
using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using BDSA2017.Assignment06.Entities;
using BDSA2017.Assignment06.DTOs;
using BDSA2017.Assignment05.Entities;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;
using System.Reflection;
using Moq;

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
        public void TestSquared()
        {
            Assert.Equal(new long[] { 1, 4, 9, 16, 25 }, ParallelOperations.Squares(1, 5));
        }
        [Fact]
        public void TestRezized()
        {
            string directory = Directory
            .GetParent(Assembly.GetExecutingAssembly().Location)
            .Parent.Parent.Parent.Parent.FullName + @"\BDSA2017.Assignment06";
            IEnumerable<string> test = Directory.GetFiles(directory + @"\images");
            ParallelOperations.CreateThumbnails(new PictureModule(), test, directory + @"\imageRezized", new Size(1000, 1000));
            var mock = new Mock<IPictureModule>();

            Assert.False(true);
        }
        [Fact]
        public async Task TestUpdateCarInRaceAsync()
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
                await context.AddAsync(carInRace);
                await context.SaveChangesAsync();
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
                Assert.Equal(UpdatedCarInRaceInfo.EndPosition, context.CarsInRace.Find(carInRace.RaceId, carInRace.CarId).EndPosition);
                Assert.Equal(UpdatedCarInRaceInfo.FastestLap, context.CarsInRace.Find(carInRace.RaceId, carInRace.CarId).FastestLap);
                Assert.Equal(UpdatedCarInRaceInfo.TotalTime, context.CarsInRace.Find(carInRace.RaceId, carInRace.CarId).TotalTime);
                Assert.Equal(UpdatedCarInRaceInfo.StartPosition, context.CarsInRace.Find(carInRace.RaceId, carInRace.CarId).StartPosition);
            }

        }
        [Fact]
        public async Task TestUpdate()
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
                await context.SaveChangesAsync();
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
                await raceRepository.UpdateAsync(RaceCreate);
                Assert.Equal((await context.Races.FindAsync(race.Id)).Track, trackupdated);
            }
        }
        [Fact]
        public async Task TestUpdateRaceNotFound()
        {
            using (raceRepository)
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

                Assert.Equal((false, "no race found"), await raceRepository.UpdateAsync(RaceCreate));
            
            }
        }


        [Fact]
        public async Task TestRead()
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
                    Track = track
                };
                context.Add(race);
                await context.SaveChangesAsync();
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

                Assert.Equal(raceCreate, await raceRepository.ReadAsync(race.Id));
            }

        }
        [Fact]
        public async Task TestReadList()
        {
            using (raceRepository)
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
                await context.SaveChangesAsync();
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

                Assert.Equal(new List<RaceListDTO> { raceList }, await raceRepository.ReadAsync());
            }

        }

        [Fact]
        public async Task TestRemoveCarFromRace()
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
                await context.SaveChangesAsync();
                await raceRepository.RemoveCarFromRaceAsync(car.Id, race.Id);


                Assert.Null(await context.CarsInRace.FindAsync(carInRace.CarId, carInRace.RaceId));

            }
        }
        [Fact]
        public async Task TestRemoveCarFromRaceReturnsCarDosntExist()
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

                Assert.Equal((false, "The choosen car was not in the choosen race"), await raceRepository.RemoveCarFromRaceAsync(car.Id, race.Id));

            }
        }
        [Fact]
        public async Task TestAddCarToRace()
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
                await context.SaveChangesAsync();
                await raceRepository.AddCarToRaceAsync(car.Id, race.Id, 5);
                var carInRace = (from carInRaces in context.CarsInRace.AsParallel()
                                 where carInRaces.CarId == car.Id && carInRaces.RaceId == race.Id
                                 select carInRaces).Count();
                Assert.True(carInRace > 0);

            }
        }
        [Fact]
        public async Task TestAddCarToRaceFalseRaceHasStarted()
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
                    ActualStart = new DateTime(1231, 04, 11),
                    PlannedEnd = new DateTime(1920, 11, 11),
                    PlannedStart = new DateTime(1920, 11, 11),
                    Track = track
                };
                context.Cars.Add(car);
                context.Races.Add(race);
                await context.SaveChangesAsync();


                Assert.Equal((false, "Race or car not excisting or has started"), await raceRepository.AddCarToRaceAsync(car.Id, race.Id, 5));

            }
        }
        [Fact]
        public async Task TestCreateRace()
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
                await context.SaveChangesAsync();
                var raceDTO = new RaceCreateDTO()
                {
                    ActualEnd = new DateTime(1920, 11, 11),
                    ActualStart = new DateTime(1920, 11, 11),
                    NumberOfLaps = 5,
                    PlannedEnd = new DateTime(1920, 11, 11),
                    PlannedStart = new DateTime(1920, 11, 11),
                    TrackId = track.Id
                };

                Assert.NotNull(await context.Races.FindAsync(await raceRepository.CreateAsync(raceDTO)));


            }

        }
        [Fact]
        public async Task TestCreateRaceFailsStarted()
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
                await context.SaveChangesAsync();
                RaceCreateDTO raceDTO = null;

                Assert.Equal(0, await raceRepository.CreateAsync(raceDTO));


            }

        }
        [Fact]
        public async Task TestDeleteRace()
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
                await context.SaveChangesAsync();
                Assert.Equal((false, "Race was not found or hasnt started yet"), await raceRepository.DeleteAsync(race.Id));

            }


        }
        [Fact]
        public async Task TestDeleteRace2()
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
                await context.SaveChangesAsync();
                Assert.Equal((true, ""), await raceRepository.DeleteAsync(race.Id));

            }


        }
       

        public void Dispose()
        {
            context.Dispose();
        }
    }
}