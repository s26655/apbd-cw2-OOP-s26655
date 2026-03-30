using Solution.Common;
using Solution.Domain.Rentals;
using Solution.Domain.Users;

namespace Solution.Services;

public class RentalService
{
    private readonly List<Rental> _rentals = new();
    private readonly EquipmentService _equipmentService;
    private readonly UserService _userService;
    private readonly RentalRulesService _rentalRulesService;
    private readonly PenaltyPolicy _penaltyPolicy;
    private readonly IdGenerator _idGenerator;

    public RentalService(
        EquipmentService equipmentService,
        UserService userService,
        RentalRulesService rentalRulesService,
        PenaltyPolicy penaltyPolicy,
        IdGenerator idGenerator)
    {
        _equipmentService = equipmentService;
        _userService = userService;
        _rentalRulesService = rentalRulesService;
        _penaltyPolicy = penaltyPolicy;
        _idGenerator = idGenerator;
    }

    public OperationResult RentEquipment(string userId, string equipmentId, int rentalDays)
    {
        var user = _userService.GetById(userId);
        if (user is null)
        {
            return OperationResult.Failure($"User with id {userId} was not found.");
        }

        var equipment = _equipmentService.GetById(equipmentId);
        if (equipment is null)
        {
            return OperationResult.Failure($"Equipment with id {equipmentId} was not found.");
        }

        if (equipment.IsMarkedUnavailable || !equipment.IsAvailable)
        {
            return OperationResult.Failure($"Equipment {equipmentId} is not available for rental.");
        }

        var activeRentalsCount = GetActiveRentalsByUser(userId).Count;
        var rentalLimit = _rentalRulesService.GetActiveRentalLimit(user);

        if (activeRentalsCount >= rentalLimit)
        {
            return OperationResult.Failure(
                $"User {user.FullName} has reached the active rental limit of {rentalLimit}.");
        }

        var rental = new Rental(
            _idGenerator.GenerateRentalId(),
            userId,
            equipmentId,
            DateTime.Now,
            DateTime.Now.AddDays(rentalDays));

        _rentals.Add(rental);
        equipment.SetAvailability(false);

        return OperationResult.Success(
            $"Equipment {equipmentId} was rented to {user.FullName} for {rentalDays} day(s).");
    }

    public OperationResult ReturnEquipment(string equipmentId, DateTime returnDate)
    {
        var equipment = _equipmentService.GetById(equipmentId);
        if (equipment is null)
        {
            return OperationResult.Failure($"Equipment with id {equipmentId} was not found.");
        }

        var rental = _rentals
            .LastOrDefault(r => r.EquipmentId == equipmentId && !r.IsReturned);

        if (rental is null)
        {
            return OperationResult.Failure($"No active rental found for equipment {equipmentId}.");
        }

        var penalty = _penaltyPolicy.CalculatePenalty(rental.DueDate, returnDate);
        rental.Return(returnDate, penalty);
        equipment.SetAvailability(true);

        return OperationResult.Success(
            $"Equipment {equipmentId} was returned. Penalty: {penalty}.");
    }

    public IReadOnlyList<Rental> GetAllRentals()
    {
        return _rentals.AsReadOnly();
    }

    public IReadOnlyList<Rental> GetActiveRentals()
    {
        return _rentals
            .Where(r => !r.IsReturned)
            .ToList()
            .AsReadOnly();
    }

    public IReadOnlyList<Rental> GetActiveRentalsByUser(string userId)
    {
        return _rentals
            .Where(r => r.UserId == userId && !r.IsReturned)
            .ToList()
            .AsReadOnly();
    }

    public IReadOnlyList<Rental> GetOverdueRentals()
    {
        return _rentals
            .Where(r => !r.IsReturned && r.DueDate < DateTime.Now)
            .ToList()
            .AsReadOnly();
    }
}