namespace Solution.Domain.Equipment;

public class Projector : Equipment
{
    public Projector(string id, string name, int lumens, string resolution)
        : base(id, name)
    {
        Lumens = lumens;
        Resolution = resolution;
    }

    public int Lumens { get; }
    public string Resolution { get; }

    public override string ToString()
    {
        return $"{base.ToString()} | Type: Projector | Lumens: {Lumens} | Resolution: {Resolution}";
    }
}