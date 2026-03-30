using System.Text;
using Solution.Domain.Rentals;
using Solution.Domain.Users;

namespace Solution.Services;

public class ReportService
{
    private readonly EquipmentService _equipmentService;
    private readonly UserService _userService;
    private readonly RentalService _rentalService;

    public ReportService(
        EquipmentService equipmentService,
        UserService userService,
        RentalService rentalService)
    {
        _equipmentService = equipmentService;
        _userService = userService;
        _rentalService = rentalService;
    }

    public string GenerateAllEquipmentReport()
    {
        var equipmentItems = _equipmentService.GetAllEquipment();
        var builder = new StringBuilder();

        builder.AppendLine("=== All equipment ===");

        if (!equipmentItems.Any())
        {
            builder.AppendLine("No equipment found.");
            return builder.ToString();
        }

        foreach (var equipment in equipmentItems)
        {
            builder.AppendLine(equipment.ToString());
        }

        return builder.ToString();
    }

    public string GenerateAvailableEquipmentReport()
    {
        var equipmentItems = _equipmentService.GetAvailableEquipment();
        var builder = new StringBuilder();

        builder.AppendLine("=== Available equipment ===");

        if (!equipmentItems.Any())
        {
            builder.AppendLine("No available equipment found.");
            return builder.ToString();
        }

        foreach (var equipment in equipmentItems)
        {
            builder.AppendLine(equipment.ToString());
        }

        return builder.ToString();
    }

    public string GenerateActiveRentalsForUserReport(string userId)
    {
        var builder = new StringBuilder();
        var user = _userService.GetById(userId);

        builder.AppendLine("=== Active rentals for selected user ===");

        if (user is null)
        {
            builder.AppendLine($"User with id {userId} was not found.");
            return builder.ToString();
        }

        var activeRentals = _rentalService.GetActiveRentalsByUser(userId);

        builder.AppendLine($"User: {user.FullName} ({user.UserType})");

        if (!activeRentals.Any())
        {
            builder.AppendLine("No active rentals found.");
            return builder.ToString();
        }

        foreach (var rental in activeRentals)
        {
            builder.AppendLine(rental.ToString());
        }

        return builder.ToString();
    }

    public string GenerateOverdueRentalsReport()
    {
        var overdueRentals = _rentalService.GetOverdueRentals();
        var builder = new StringBuilder();

        builder.AppendLine("=== Overdue rentals ===");

        if (!overdueRentals.Any())
        {
            builder.AppendLine("No overdue rentals found.");
            return builder.ToString();
        }

        foreach (var rental in overdueRentals)
        {
            builder.AppendLine(rental.ToString());
        }

        return builder.ToString();
    }

    public string GenerateSummaryReport()
    {
        var allEquipment = _equipmentService.GetAllEquipment();
        var availableEquipment = _equipmentService.GetAvailableEquipment();
        var allUsers = _userService.GetAllUsers();
        var allRentals = _rentalService.GetAllRentals();
        var activeRentals = _rentalService.GetActiveRentals();
        var overdueRentals = _rentalService.GetOverdueRentals();

        var builder = new StringBuilder();

        builder.AppendLine("=== Rental service summary ===");
        builder.AppendLine($"Total users: {allUsers.Count}");
        builder.AppendLine($"Total equipment items: {allEquipment.Count}");
        builder.AppendLine($"Available equipment items: {availableEquipment.Count}");
        builder.AppendLine($"Total rentals: {allRentals.Count}");
        builder.AppendLine($"Active rentals: {activeRentals.Count}");
        builder.AppendLine($"Overdue rentals: {overdueRentals.Count}");

        return builder.ToString();
    }
}