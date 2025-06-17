using BoatBackend.Data;
using BoatBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace BoatBackend.Repositories;

public class BoatRepository(AppDbContext dbContext, ILogger<BoatRepository> logger)
{
    public async Task<Boat?> CreateBoat(Boat boat)
    {
        try
        {
            dbContext.Add(boat);
            await dbContext.SaveChangesAsync();
            return boat;
        }
        catch (Exception e)
        {
            logger.LogError(e, "CreateBoat failed {@Boat}", boat);
            return null;
        }
    }

    public async Task<Boat?> GetBoatById(int id)
    {
        try
        {
            return await dbContext.Boats.FirstOrDefaultAsync(b => b.Id == id);
        }
        catch (Exception e)
        {
            logger.LogError(e, "GetBoatById failed {@Id}", id);
            return null;
        }
    }

    public async Task<IEnumerable<Boat>> GetAllBoats()
    {
        try
        {
            return await dbContext.Boats.ToListAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "GetAllBoats failed");
            return [];
        }
    }

    public async Task<Boat?> UpdateBoat(Boat boat)
    {
        try
        {
            logger.LogInformation("Update boat {id} {name}", boat.Id, boat.Name);
            dbContext.Boats.Update(boat);
            await dbContext.SaveChangesAsync();
            return boat;
        }
        catch (Exception e)
        {
            logger.LogError(e, "UpdateBoat failed {@Boat}", boat);
            return null;
        }
    }

    public async Task<bool> DeleteBoat(int id)
    {
        try
        {
            logger.LogInformation("Delete boat {id}", id);
            var boat = await dbContext.Boats.FindAsync(id);
            if (boat == null) return false;

            dbContext.Boats.Remove(boat);
            await dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "DeleteBoat failed {@Id}", id);
            return false;
        }
    }
}