using BoatBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace BoatBackend.Data;

public class AppDbContext : DbContext
{
    public DbSet<Boat> Boats { get; set; }
    public DbSet<User> Users { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseInMemoryDatabase("BoatDb");
    }
}