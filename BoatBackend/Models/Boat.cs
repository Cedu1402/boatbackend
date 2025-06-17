using System.ComponentModel.DataAnnotations;

namespace BoatBackend.Models;

public class Boat
{
    public int Id { get; set; }

    [MaxLength(100)] public string Name { get; set; } = string.Empty;

    [MaxLength(500)] public string Description { get; set; } = string.Empty;
}