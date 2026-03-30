using Solution.Common;
using Solution.Domain.Equipment;
using Solution.Domain.Users;
using Solution.Persistence;
using Solution.Services;
using System.IO;

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
        var jsonStorageService = new JsonStorageService();

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

        rentalService.RentEquipment(student.Id, laptop.Id, 7);
        rentalService.RentEquipment(student.Id, projector.Id, 5);

        rentalService.ReturnEquipment(projector.Id, DateTime.Now.AddDays(5));
        rentalService.ReturnEquipment(laptop.Id, DateTime.Now.AddDays(10));

        Console.WriteLine("=== Original summary ===");
        Console.WriteLine(reportService.GenerateSummaryReport());

        var projectRoot = Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
        var filePath = Path.Combine(projectRoot, "Data", "appdata.json");

        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

        jsonStorageService.SaveToFile(
            filePath,
            equipmentService.GetAllEquipment(),
            userService.GetAllUsers(),
            rentalService.GetAllRentals());

        Console.WriteLine($"Data saved to {filePath}");

        var loadedData = jsonStorageService.LoadFromFile(filePath);

        var restoredEquipment = jsonStorageService.RestoreEquipment(loadedData);
        var restoredUsers = jsonStorageService.RestoreUsers(loadedData);
        var restoredRentals = jsonStorageService.RestoreRentals(loadedData);

        var restoredEquipmentService = new EquipmentService();
        var restoredUserService = new UserService();
        var restoredRentalService = new RentalService(
            restoredEquipmentService,
            restoredUserService,
            rentalRulesService,
            penaltyPolicy,
            idGenerator);

        restoredEquipmentService.ReplaceAll(restoredEquipment);
        restoredUserService.ReplaceAll(restoredUsers);
        restoredRentalService.ReplaceAll(restoredRentals);

        var restoredReportService = new ReportService(
            restoredEquipmentService,
            restoredUserService,
            restoredRentalService);

        Console.WriteLine();
        Console.WriteLine("=== Restored summary ===");
        Console.WriteLine(restoredReportService.GenerateSummaryReport());
    }
}