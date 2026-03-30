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

        Console.WriteLine("=== Correct rental ===");
        Console.WriteLine(rentalService.RentEquipment(student.Id, laptop.Id, 7).Message);

        Console.WriteLine();
        Console.WriteLine("=== Second correct rental ===");
        Console.WriteLine(rentalService.RentEquipment(student.Id, projector.Id, 5).Message);

        Console.WriteLine();
        Console.WriteLine("=== Invalid rental: student exceeds limit ===");
        Console.WriteLine(rentalService.RentEquipment(student.Id, camera.Id, 3).Message);

        Console.WriteLine();
        Console.WriteLine("=== Active rentals for selected user ===");
        foreach (var rental in rentalService.GetActiveRentalsByUser(student.Id))
        {
            Console.WriteLine(rental);
        }

        Console.WriteLine();
        Console.WriteLine("=== Return on time ===");
        var onTimeReturnDate = DateTime.Now.AddDays(5);
        Console.WriteLine(rentalService.ReturnEquipment(projector.Id, onTimeReturnDate).Message);

        Console.WriteLine();
        Console.WriteLine("=== Return late with penalty ===");
        var lateReturnDate = DateTime.Now.AddDays(10);
        Console.WriteLine(rentalService.ReturnEquipment(laptop.Id, lateReturnDate).Message);

        Console.WriteLine();
        Console.WriteLine("=== Overdue rentals ===");
        foreach (var rental in rentalService.GetOverdueRentals())
        {
            Console.WriteLine(rental);
        }

        Console.WriteLine();
        Console.WriteLine("=== All rentals ===");
        foreach (var rental in rentalService.GetAllRentals())
        {
            Console.WriteLine(rental);
        }
    }
}