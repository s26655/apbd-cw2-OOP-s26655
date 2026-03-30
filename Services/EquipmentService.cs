using Solution.Common;
using Solution.Domain.Equipment;

namespace Solution.Services;

public class EquipmentService
{
    private readonly List<Equipment> _equipment = new();

    public OperationResult AddEquipment(Equipment equipment)
    {
        if (_equipment.Any(e => e.Id == equipment.Id))
        {
            return OperationResult.Failure($"Equipment with id {equipment.Id} already exists.");
        }

        _equipment.Add(equipment);
        return OperationResult.Success($"Equipment {equipment.Name} was added successfully.");
    }

    public IReadOnlyList<Equipment> GetAllEquipment()
    {
        return _equipment.AsReadOnly();
    }

    public IReadOnlyList<Equipment> GetAvailableEquipment()
    {
        return _equipment
            .Where(e => e.IsAvailable && !e.IsMarkedUnavailable)
            .ToList()
            .AsReadOnly();
    }

    public Equipment? GetById(string equipmentId)
    {
        return _equipment.FirstOrDefault(e => e.Id == equipmentId);
    }

    public OperationResult MarkAsUnavailable(string equipmentId, string reason)
    {
        var equipment = GetById(equipmentId);

        if (equipment is null)
        {
            return OperationResult.Failure($"Equipment with id {equipmentId} was not found.");
        }

        equipment.MarkAsUnavailable(reason);
        return OperationResult.Success($"Equipment {equipmentId} was marked as unavailable.");
    }

    public void ReplaceAll(IEnumerable<Solution.Domain.Equipment.Equipment> equipmentItems)
    {
        _equipment.Clear();
        _equipment.AddRange(equipmentItems);
    }
}