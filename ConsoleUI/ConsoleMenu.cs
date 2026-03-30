using System.IO;
using System.Text.RegularExpressions;
using Solution.Common;
using Solution.Domain.Equipment;
using Solution.Domain.Users;
using Solution.Persistence;
using Solution.Services;

namespace Solution.ConsoleUI;

public class ConsoleMenu
{
    private readonly IdGenerator _idGenerator;
    private readonly EquipmentService _equipmentService;
    private readonly UserService _userService;
    private readonly RentalService _rentalService;
    private readonly ReportService _reportService;
    private readonly JsonStorageService _jsonStorageService;

    public ConsoleMenu(
        IdGenerator idGenerator,
        EquipmentService equipmentService,
        UserService userService,
        RentalService rentalService,
        ReportService reportService,
        JsonStorageService jsonStorageService)
    {
        _idGenerator = idGenerator;
        _equipmentService = equipmentService;
        _userService = userService;
        _rentalService = rentalService;
        _reportService = reportService;
        _jsonStorageService = jsonStorageService;
    }

    public void Run()
    {
        bool exitRequested = false;

        while (!exitRequested)
        {
            DisplayMenu();
            var choice = ReadRequiredText("Choose an option: ");

            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    AddUser();
                    break;
                case "2":
                    AddEquipment();
                    break;
                case "3":
                    ShowAllUsers();
                    break;
                case "4":
                    Console.WriteLine(_reportService.GenerateAllEquipmentReport());
                    break;
                case "5":
                    Console.WriteLine(_reportService.GenerateAvailableEquipmentReport());
                    break;
                case "6":
                    RentEquipment();
                    break;
                case "7":
                    ReturnEquipment();
                    break;
                case "8":
                    MarkEquipmentAsUnavailable();
                    break;
                case "9":
                    ShowActiveRentalsForUser();
                    break;
                case "10":
                    Console.WriteLine(_reportService.GenerateOverdueRentalsReport());
                    break;
                case "11":
                    Console.WriteLine(_reportService.GenerateSummaryReport());
                    break;
                case "12":
                    SaveToJson();
                    break;
                case "13":
                    LoadFromJson();
                    break;
                case "0":
                    exitRequested = true;
                    Console.WriteLine("Exiting application.");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }

            Console.WriteLine();

            if (!exitRequested)
            {
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }

    private void DisplayMenu()
    {
        Console.WriteLine("=== University Equipment Rental Service ===");
        Console.WriteLine("1. Add user");
        Console.WriteLine("2. Add equipment");
        Console.WriteLine("3. Show all users");
        Console.WriteLine("4. Show all equipment");
        Console.WriteLine("5. Show available equipment");
        Console.WriteLine("6. Rent equipment");
        Console.WriteLine("7. Return equipment");
        Console.WriteLine("8. Mark equipment as unavailable");
        Console.WriteLine("9. Show active rentals for user");
        Console.WriteLine("10. Show overdue rentals");
        Console.WriteLine("11. Show summary report");
        Console.WriteLine("12. Save data to JSON");
        Console.WriteLine("13. Load data from JSON");
        Console.WriteLine("0. Exit");
    }

    private void AddUser()
    {
        Console.WriteLine("=== Add user ===");

        var userTypeChoice = ReadChoice(
            "Choose user type (1 - Student, 2 - Employee): ",
            "1",
            "2");

        var firstName = ReadPersonName("First name: ");
        var lastName = ReadPersonName("Last name: ");
        var userId = _idGenerator.GenerateUserId();

        User user = userTypeChoice switch
        {
            "1" => new Student(userId, firstName, lastName),
            "2" => new Employee(userId, firstName, lastName),
            _ => throw new InvalidOperationException("Unexpected user type choice.")
        };

        var result = _userService.AddUser(user);
        Console.WriteLine(result.Message);
        Console.WriteLine($"Created user ID: {user.Id}");
    }

    private void AddEquipment()
    {
        Console.WriteLine("=== Add equipment ===");

        var equipmentTypeChoice = ReadChoice(
            "Choose equipment type (1 - Laptop, 2 - Projector, 3 - Camera): ",
            "1",
            "2",
            "3");

        var equipmentId = _idGenerator.GenerateEquipmentId();
        var name = ReadGeneralText("Equipment name: ");

        Equipment equipment = equipmentTypeChoice switch
        {
            "1" => CreateLaptop(equipmentId, name),
            "2" => CreateProjector(equipmentId, name),
            "3" => CreateCamera(equipmentId, name),
            _ => throw new InvalidOperationException("Unexpected equipment type choice.")
        };

        var result = _equipmentService.AddEquipment(equipment);
        Console.WriteLine(result.Message);
        Console.WriteLine($"Created equipment ID: {equipment.Id}");
    }

    private Laptop CreateLaptop(string equipmentId, string name)
    {
        var ramGb = ReadInt("RAM (GB): ");
        var operatingSystem = ReadGeneralText("Operating system: ");
        return new Laptop(equipmentId, name, ramGb, operatingSystem);
    }

    private Projector CreateProjector(string equipmentId, string name)
    {
        var lumens = ReadInt("Lumens: ");
        var resolution = ReadGeneralText("Resolution: ");
        return new Projector(equipmentId, name, lumens, resolution);
    }

    private Camera CreateCamera(string equipmentId, string name)
    {
        var megapixels = ReadInt("Megapixels: ");
        var lensType = ReadGeneralText("Lens type: ");
        return new Camera(equipmentId, name, megapixels, lensType);
    }

    private void RentEquipment()
    {
        Console.WriteLine("=== Rent equipment ===");
        var userId = ReadRequiredText("User ID: ");
        var equipmentId = ReadRequiredText("Equipment ID: ");
        var rentalDays = ReadInt("Rental days: ");

        var result = _rentalService.RentEquipment(userId, equipmentId, rentalDays);
        Console.WriteLine(result.Message);
    }

    private void ReturnEquipment()
    {
        Console.WriteLine("=== Return equipment ===");
        var equipmentId = ReadRequiredText("Equipment ID: ");
        var result = _rentalService.ReturnEquipment(equipmentId, DateTime.Now);
        Console.WriteLine(result.Message);
    }

    private void MarkEquipmentAsUnavailable()
    {
        Console.WriteLine("=== Mark equipment as unavailable ===");
        var equipmentId = ReadRequiredText("Equipment ID: ");
        var reason = ReadGeneralText("Reason: ");

        var result = _equipmentService.MarkAsUnavailable(equipmentId, reason);
        Console.WriteLine(result.Message);
    }

    private void ShowAllUsers()
    {
        Console.WriteLine("=== All users ===");

        var users = _userService.GetAllUsers();

        if (!users.Any())
        {
            Console.WriteLine("No users found.");
            return;
        }

        foreach (var user in users)
        {
            Console.WriteLine(user);
        }
    }

    private void ShowActiveRentalsForUser()
    {
        Console.WriteLine("=== Active rentals for user ===");
        var userId = ReadRequiredText("User ID: ");
        Console.WriteLine(_reportService.GenerateActiveRentalsForUserReport(userId));
    }

    private void SaveToJson()
    {
        Console.WriteLine("=== Save data to JSON ===");
        var filePath = GetDefaultJsonPath();

        _jsonStorageService.SaveToFile(
            filePath,
            _equipmentService.GetAllEquipment(),
            _userService.GetAllUsers(),
            _rentalService.GetAllRentals());

        Console.WriteLine($"Data saved to {filePath}");
    }

    private void LoadFromJson()
    {
        Console.WriteLine("=== Load data from JSON ===");
        var filePath = GetDefaultJsonPath();

        try
        {
            var appData = _jsonStorageService.LoadFromFile(filePath);

            var restoredEquipment = _jsonStorageService.RestoreEquipment(appData);
            var restoredUsers = _jsonStorageService.RestoreUsers(appData);
            var restoredRentals = _jsonStorageService.RestoreRentals(appData);

            _equipmentService.ReplaceAll(restoredEquipment);
            _userService.ReplaceAll(restoredUsers);
            _rentalService.ReplaceAll(restoredRentals);

            Console.WriteLine($"Data loaded from {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load data: {ex.Message}");
        }
    }

    private static string ReadChoice(string prompt, params string[] allowedChoices)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine()?.Trim();

            if (!string.IsNullOrWhiteSpace(input) && allowedChoices.Contains(input))
            {
                return input;
            }

            Console.WriteLine($"Please enter one of the allowed options: {string.Join(", ", allowedChoices)}.");
        }
    }

    private static string ReadPersonName(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Value cannot be empty.");
                continue;
            }

            if (Regex.IsMatch(input, @"^[A-Za-zÀ-ÿŻŹĆĄŚĘŁÓŃżźćńółęąś\- ]+$"))
            {
                return input;
            }

            Console.WriteLine("Please enter a valid name using letters, spaces, or hyphens only.");
        }
    }

    private static string ReadGeneralText(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Value cannot be empty.");
                continue;
            }

            if (Regex.IsMatch(input, @"^[A-Za-z0-9À-ÿŻŹĆĄŚĘŁÓŃżźćńółęąś\-\.\,\(\)/+ ]+$"))
            {
                return input;
            }

            Console.WriteLine("Please enter valid text using letters, numbers, spaces, and common punctuation only.");
        }
    }

    private static string ReadRequiredText(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                return input.Trim();
            }

            Console.WriteLine("Value cannot be empty.");
        }
    }

    private static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();

            if (int.TryParse(input, out var value) && value > 0)
            {
                return value;
            }

            Console.WriteLine("Please enter a valid positive number.");
        }
    }

    private static string GetDefaultJsonPath()
    {
        var projectRoot = Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
        var dataDirectory = Path.Combine(projectRoot, "Data");

        Directory.CreateDirectory(dataDirectory);

        return Path.Combine(dataDirectory, "appdata.json");
    }
}