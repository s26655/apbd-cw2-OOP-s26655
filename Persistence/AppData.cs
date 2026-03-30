using System.Text.Json.Serialization;

namespace Solution.Persistence;

public class AppData
{
    public List<EquipmentDto> EquipmentItems { get; set; } = new();
    public List<UserDto> Users { get; set; } = new();
    public List<RentalDto> Rentals { get; set; } = new();
}

public class EquipmentDto
{
    public string EquipmentType { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
    public bool IsMarkedUnavailable { get; set; }
    public string UnavailableReason { get; set; } = string.Empty;

    public int? RamGb { get; set; }
    public string? OperatingSystem { get; set; }

    public int? Lumens { get; set; }
    public string? Resolution { get; set; }

    public int? Megapixels { get; set; }
    public string? LensType { get; set; }
}

public class UserDto
{
    public string UserType { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}

public class RentalDto
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string EquipmentId { get; set; } = string.Empty;
    public DateTime RentalDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnedAt { get; set; }
    public decimal Penalty { get; set; }
}