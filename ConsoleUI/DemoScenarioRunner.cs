using Solution.Common;
using Solution.Domain.Equipment;
using Solution.Domain.Users;
using Solution.Services;

namespace Solution.ConsoleUI;

public class DemoScenarioRunner
{
    private readonly IdGenerator _idGenerator;
    private readonly EquipmentService _equipmentService;
    private readonly UserService _userService;
    private readonly RentalService _rentalService;
    private readonly ReportService _reportService;

    public DemoScenarioRunner(
        IdGenerator idGenerator,
        EquipmentService equipmentService,
        UserService userService,
        RentalService rentalService,
        ReportService reportService)
    {
        _idGenerator = idGenerator;
        _equipmentService = equipmentService;
        _userService = userService;
        _rentalService = rentalService;
        _reportService = reportService;
    }

    public void Run()
    {
        Console.Clear();
        Console.WriteLine("=== Demo scenario ===");
        Console.WriteLine();

        // 1. Add users
        var student = new Student(
            _idGenerator.GenerateUserId(),
            "Adrian",
            "Nowacki");

        var employee = new Employee(
            _idGenerator.GenerateUserId(),
            "Jan",
            "Kowalski");

        Console.WriteLine("Adding users:");
        Console.WriteLine(_userService.AddUser(student).Message);
        Console.WriteLine(_userService.AddUser(employee).Message);

        Console.WriteLine();

        // 2. Add equipment
        var laptop = new Laptop(
            _idGenerator.GenerateEquipmentId(),
            "Dell Latitude 5440",
            16,
            "Windows 11");

        var projector = new Projector(
            _idGenerator.GenerateEquipmentId(),
            "Epson EB-FH52",
            4000,
            "1920x1080");

        var camera = new Camera(
            _idGenerator.GenerateEquipmentId(),
            "Canon EOS R10",
            24,
            "Zoom");

        Console.WriteLine("Adding equipment:");
        Console.WriteLine(_equipmentService.AddEquipment(laptop).Message);
        Console.WriteLine(_equipmentService.AddEquipment(projector).Message);
        Console.WriteLine(_equipmentService.AddEquipment(camera).Message);

        Console.WriteLine();

        // 3. Correct rental
        Console.WriteLine("Correct rental:");
        Console.WriteLine(_rentalService.RentEquipment(student.Id, laptop.Id, 7).Message);

        Console.WriteLine();

        // 4. Invalid operation: unavailable equipment
        Console.WriteLine("Marking equipment as unavailable:");
        Console.WriteLine(_equipmentService.MarkAsUnavailable(camera.Id, "Damaged lens").Message);

        Console.WriteLine();

        Console.WriteLine("Invalid rental attempt:");
        Console.WriteLine(_rentalService.RentEquipment(student.Id, camera.Id, 3).Message);

        Console.WriteLine();

        // 5. Second rental for later return on time
        Console.WriteLine("Second correct rental:");
        Console.WriteLine(_rentalService.RentEquipment(employee.Id, projector.Id, 5).Message);

        Console.WriteLine();

        // 6. Show active rentals
        Console.WriteLine(_reportService.GenerateActiveRentalsForUserReport(student.Id));
        Console.WriteLine(_reportService.GenerateActiveRentalsForUserReport(employee.Id));

        // 7. Return on time
        Console.WriteLine("Return on time:");
        Console.WriteLine(_rentalService.ReturnEquipment(projector.Id, DateTime.Now.AddDays(5)).Message);

        Console.WriteLine();

        // 8. Late return with penalty
        Console.WriteLine("Late return with penalty:");
        Console.WriteLine(_rentalService.ReturnEquipment(laptop.Id, DateTime.Now.AddDays(10)).Message);

        Console.WriteLine();

        // 9. Final reports
        Console.WriteLine(_reportService.GenerateAllEquipmentReport());
        Console.WriteLine(_reportService.GenerateAvailableEquipmentReport());
        Console.WriteLine(_reportService.GenerateOverdueRentalsReport());
        Console.WriteLine(_reportService.GenerateSummaryReport());

        Console.WriteLine("Demo scenario finished.");
    }
}