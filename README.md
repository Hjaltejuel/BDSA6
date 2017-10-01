# Assignment #6: Week 40

## Software Engineering

### Exercise 1
For each of the SOLID principles, provide an example through either a UML diagram or a code listing that showcases the violation of the specific principle.
_Note:_ the examples do not need to be too sophisticated.

### Exercise 2
For each of the examples provided for SE-1, provide a refactored solution as either a UML diagram or a code listing that showcases a possible solution respecting the principle violeted.
_Note:_ the examples do not need to be too sophisticated.

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

### Task Parallel Library

Using the `ParallelOperations` class do the following:

Test and implement the `Squares` method with the following specification:
- It should return a collection of the squares from `lowerBound` to `upperBound`).
- Example: given `1` and `5` returns `[1, 4, 9, 16, 25]`
- Computation must be done in parallel using a thread-safe collection to hold the calculated values.

Test and implement the `CreateThumbnails` method with the following specification:

- It should create a thumbnail for each of the supplied `imageFiles` and save them in the ´outputFolder`.
- Image files must be at most of `size`.
- Computation must be done in parallel.
- Testing must *verify* that the `resizer` was called with the right parameters.
