using Solution.Common;
using Solution.ConsoleUI;
using Solution.Persistence;
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
        var jsonStorageService = new JsonStorageService();

        var consoleMenu = new ConsoleMenu(
            idGenerator,
            equipmentService,
            userService,
            rentalService,
            reportService,
            jsonStorageService);

        var demoScenarioRunner = new DemoScenarioRunner(
            idGenerator,
            equipmentService,
            userService,
            rentalService,
            reportService);

        RunStartupMenu(consoleMenu, demoScenarioRunner);
    }

    private static void RunStartupMenu(ConsoleMenu consoleMenu, DemoScenarioRunner demoScenarioRunner)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== University Equipment Rental Service ===");
            Console.WriteLine("1. Run demo scenario");
            Console.WriteLine("2. Run interactive menu");
            Console.WriteLine("0. Exit");
            Console.WriteLine();
            Console.Write("Choose an option: ");

            var choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    demoScenarioRunner.Run();
                    WaitForEnter();
                    return;
                case "2":
                    consoleMenu.Run();
                    return;
                case "0":
                    Console.WriteLine("Exiting application.");
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    WaitForEnter();
                    break;
            }
        }
    }

    private static void WaitForEnter()
    {
        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}