using BoatBackend.Models;
using BoatBackend.Repositories;

namespace BoatBackend.Services;

public class SetupService(UserRepository userRepository, BoatRepository boatRepository)
{
    public async Task RunSetup()
    {
        await userRepository.CreateUser();
        await boatRepository.CreateBoat(new Boat
        {
            Name = "Test1",
            Description = "Test Boat 1"
        });
        await boatRepository.CreateBoat(new Boat
        {
            Name = "Test2",
            Description = "Test Boat 2"
        });
    }
}