using BoatBackend.Models;

namespace BoatBackend.Interfaces;

public interface IUserRepository
{
    public Task<User?> GetUserByName(string name);
    public Task<bool> CreateUser();
}