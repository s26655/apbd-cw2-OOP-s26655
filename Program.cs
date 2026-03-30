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

        var student = new Student(
            idGenerator.GenerateUserId(),
            "Adrian",
            "Nowacki");

        var employee = new Employee(
            idGenerator.GenerateUserId(),
            "Jan",
            "Kowalski");

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

        Console.WriteLine(userService.AddUser(student).Message);
        Console.WriteLine(userService.AddUser(employee).Message);

        Console.WriteLine(equipmentService.AddEquipment(laptop).Message);
        Console.WriteLine(equipmentService.AddEquipment(projector).Message);
        Console.WriteLine(equipmentService.AddEquipment(camera).Message);

        Console.WriteLine();
        Console.WriteLine("All users:");
        foreach (var user in userService.GetAllUsers())
        {
            Console.WriteLine(user);
        }

        Console.WriteLine();
        Console.WriteLine("All equipment:");
        foreach (var equipment in equipmentService.GetAllEquipment())
        {
            Console.WriteLine(equipment);
        }

        Console.WriteLine();
        Console.WriteLine("Available equipment:");
        foreach (var equipment in equipmentService.GetAvailableEquipment())
        {
            Console.WriteLine(equipment);
        }
    }
}