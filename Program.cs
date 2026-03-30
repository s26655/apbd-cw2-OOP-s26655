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

        consoleMenu.Run();
    }
}