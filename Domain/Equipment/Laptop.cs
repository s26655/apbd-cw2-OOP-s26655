namespace Solution.Domain.Equipment;

public class Laptop : Equipment
{
    public Laptop(string id, string name, int ramGb, string operatingSystem)
        : base(id, name)
    {
        RamGb = ramGb;
        OperatingSystem = operatingSystem;
    }

    public int RamGb { get; }
    public string OperatingSystem { get; }

    public override string ToString()
    {
        return $"{base.ToString()} | Type: Laptop | RAM: {RamGb} GB | OS: {OperatingSystem}";
    }
}