using BoatBackend.Data;
using BoatBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace BoatBackend.Repositories;

public class UserRepository(AppDbContext dbContext, ILogger<UserRepository> logger)
{
    public async Task<bool> CreateUser()
    {
        try
        {
            var user = new User
            {
                Name = "TestUser",
                Password = "plaintext_password" // NOTE: In real application, this needs be hashed securely
            };

            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "CreateUser failed");
            return false;
        }
    }

    public async Task<User?> GetUserByName(string name)
    {
        try
        {
            return await dbContext.Users.Where(u => u.Name == name).FirstOrDefaultAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "GetUserById failed {@Name}", name);
            return null;
        }
    }
}