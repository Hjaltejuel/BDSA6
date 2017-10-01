# Assignment #6: Week 40

## Software Engineering

### Exercise 1

## C&#35;

Fork this repository and implement the code required for the assignments below.

### Slot Car Tournament part trois

![](images/slotcars.jpg "Slot Cars")

Implement and test the `IRaceRepository` interface.

```csharp
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
```

with the following rules:

- Once a race is started (actual start time != `null`) it cannot be deleted.
- Cars can only be added or removed from a race before start.
- You cannot add more cars to a race than the track supports.

Your code should not throw exceptions. Instead, if for instance someone is trying to add a car which does exist to a race which does not exist:

```csharp
return (false, "race not found");
```

Your code must use an in-memory database and/or mocks for testing.