using System.Text.Json;
using Solution.Domain.Equipment;
using Solution.Domain.Rentals;
using Solution.Domain.Users;
using Solution.Services;

namespace Solution.Persistence;

public class JsonStorageService
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    public void SaveToFile(
        string filePath,
        IReadOnlyList<Equipment> equipmentItems,
        IReadOnlyList<User> users,
        IReadOnlyList<Rental> rentals)
    {
        var appData = new AppData
        {
            EquipmentItems = equipmentItems.Select(MapEquipmentToDto).ToList(),
            Users = users.Select(MapUserToDto).ToList(),
            Rentals = rentals.Select(MapRentalToDto).ToList()
        };

        var json = JsonSerializer.Serialize(appData, _jsonOptions);
        File.WriteAllText(filePath, json);
    }

    public AppData LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"JSON file was not found: {filePath}");
        }

        var json = File.ReadAllText(filePath);
        var appData = JsonSerializer.Deserialize<AppData>(json, _jsonOptions);

        if (appData is null)
        {
            throw new InvalidOperationException("Failed to deserialize application data.");
        }

        return appData;
    }

    public List<Equipment> RestoreEquipment(AppData appData)
    {
        return appData.EquipmentItems.Select(MapDtoToEquipment).ToList();
    }

    public List<User> RestoreUsers(AppData appData)
    {
        return appData.Users.Select(MapDtoToUser).ToList();
    }

    public List<Rental> RestoreRentals(AppData appData)
    {
        return appData.Rentals.Select(MapDtoToRental).ToList();
    }

    private static EquipmentDto MapEquipmentToDto(Equipment equipment)
    {
        var dto = new EquipmentDto
        {
            Id = equipment.Id,
            Name = equipment.Name,
            IsAvailable = equipment.IsAvailable,
            IsMarkedUnavailable = equipment.IsMarkedUnavailable,
            UnavailableReason = equipment.UnavailableReason
        };

        switch (equipment)
        {
            case Laptop laptop:
                dto.EquipmentType = "Laptop";
                dto.RamGb = laptop.RamGb;
                dto.OperatingSystem = laptop.OperatingSystem;
                break;

            case Projector projector:
                dto.EquipmentType = "Projector";
                dto.Lumens = projector.Lumens;
                dto.Resolution = projector.Resolution;
                break;

            case Camera camera:
                dto.EquipmentType = "Camera";
                dto.Megapixels = camera.Megapixels;
                dto.LensType = camera.LensType;
                break;

            default:
                throw new InvalidOperationException($"Unsupported equipment type: {equipment.GetType().Name}");
        }

        return dto;
    }

    private static UserDto MapUserToDto(User user)
    {
        return new UserDto
        {
            UserType = user.UserType,
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }

    private static RentalDto MapRentalToDto(Rental rental)
    {
        return new RentalDto
        {
            Id = rental.Id,
            UserId = rental.UserId,
            EquipmentId = rental.EquipmentId,
            RentalDate = rental.RentalDate,
            DueDate = rental.DueDate,
            ReturnedAt = rental.ReturnedAt,
            Penalty = rental.Penalty
        };
    }

    private static Equipment MapDtoToEquipment(EquipmentDto dto)
    {
        Equipment equipment = dto.EquipmentType switch
        {
            "Laptop" => new Laptop(
                dto.Id,
                dto.Name,
                dto.RamGb ?? 0,
                dto.OperatingSystem ?? string.Empty),

            "Projector" => new Projector(
                dto.Id,
                dto.Name,
                dto.Lumens ?? 0,
                dto.Resolution ?? string.Empty),

            "Camera" => new Camera(
                dto.Id,
                dto.Name,
                dto.Megapixels ?? 0,
                dto.LensType ?? string.Empty),

            _ => throw new InvalidOperationException($"Unsupported equipment type in JSON: {dto.EquipmentType}")
        };

        equipment.RestoreAvailabilityState(
            dto.IsAvailable,
            dto.IsMarkedUnavailable,
            dto.UnavailableReason);

        return equipment;
    }

    private static User MapDtoToUser(UserDto dto)
    {
        return dto.UserType switch
        {
            "Student" => new Student(dto.Id, dto.FirstName, dto.LastName),
            "Employee" => new Employee(dto.Id, dto.FirstName, dto.LastName),
            _ => throw new InvalidOperationException($"Unsupported user type in JSON: {dto.UserType}")
        };
    }

    private static Rental MapDtoToRental(RentalDto dto)
    {
        var rental = new Rental(
            dto.Id,
            dto.UserId,
            dto.EquipmentId,
            dto.RentalDate,
            dto.DueDate);

        rental.RestoreReturnState(dto.ReturnedAt, dto.Penalty);
        return rental;
    }
}