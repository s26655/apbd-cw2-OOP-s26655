namespace Solution.Common;

public class IdGenerator
{
    private int _equipmentCounter = 1;
    private int _userCounter = 1;
    private int _rentalCounter = 1;

    public string GenerateEquipmentId()
    {
        var id = $"EQ-{_equipmentCounter:000}";
        _equipmentCounter++;
        return id;
    }

    public string GenerateUserId()
    {
        var id = $"USR-{_userCounter:000}";
        _userCounter++;
        return id;
    }

    public string GenerateRentalId()
    {
        var id = $"RNT-{_rentalCounter:000}";
        _rentalCounter++;
        return id;
    }
}