using BoatBackend.Interfaces;
using BoatBackend.Models;
using BoatBackend.Repositories;

namespace BoatBackend.Services;

public class SetupService(IUserRepository userRepository, BoatRepository boatRepository)
{
    public async Task RunSetup()
    {
        await userRepository.CreateUser();

        for (var i = 0; i < 100; i++)
        {
            var repeatedChars = new string('k', i);
            await boatRepository.CreateBoat(new Boat
            {
                Name = $"Test {i + 1}",
                Description = $"Test boat {i + 1} {repeatedChars}"
            });
        }
    }
}