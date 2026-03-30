using Solution.Common;
using Solution.Domain.Equipment;
using Solution.Domain.Users;
using Solution.Services;

namespace Solution;

public class Program
{
    public static void Main(string[] args)
    {
        var idGenerator = new IdGenerator();
        var equipmentService = new EquipmentService();
        var userService = new UserService();
        var rentalRulesService = new RentalRulesService();
        var penaltyPolicy = new PenaltyPolicy();
        var rentalService = new RentalService(
            equipmentService,
            userService,
            rentalRulesService,
            penaltyPolicy,
            idGenerator);
        var reportService = new ReportService(
            equipmentService,
            userService,
            rentalService);

        var student = new Student(
            idGenerator.GenerateUserId(),
            "Adrian",
            "Nowacki");

        var employee = new Employee(
            idGenerator.GenerateUserId(),
            "Jan",
            "Kowalski");

        userService.AddUser(student);
        userService.AddUser(employee);

        var laptop = new Laptop(
            idGenerator.GenerateEquipmentId(),
            "Dell Latitude 5440",
            16,
            "Windows 11");

        var projector = new Projector(
            idGenerator.GenerateEquipmentId(),
            "Epson EB-FH52",
            4000,
            "1920x1080");

        var camera = new Camera(
            idGenerator.GenerateEquipmentId(),
            "Canon EOS R10",
            24,
            "Zoom");

        equipmentService.AddEquipment(laptop);
        equipmentService.AddEquipment(projector);
        equipmentService.AddEquipment(camera);

        equipmentService.MarkAsUnavailable(camera.Id, "Damaged lens");

        Console.WriteLine("=== Rental operations ===");
        Console.WriteLine(rentalService.RentEquipment(student.Id, laptop.Id, 7).Message);
        Console.WriteLine(rentalService.RentEquipment(student.Id, projector.Id, 5).Message);
        Console.WriteLine(rentalService.RentEquipment(student.Id, camera.Id, 3).Message);

        Console.WriteLine();
        Console.WriteLine(reportService.GenerateAllEquipmentReport());

        Console.WriteLine(reportService.GenerateAvailableEquipmentReport());

        Console.WriteLine(reportService.GenerateActiveRentalsForUserReport(student.Id));

        Console.WriteLine("=== Return operations ===");
        Console.WriteLine(rentalService.ReturnEquipment(projector.Id, DateTime.Now.AddDays(5)).Message);
        Console.WriteLine(rentalService.ReturnEquipment(laptop.Id, DateTime.Now.AddDays(10)).Message);

        Console.WriteLine();
        Console.WriteLine(reportService.GenerateOverdueRentalsReport());

        Console.WriteLine(reportService.GenerateSummaryReport());
    }
}