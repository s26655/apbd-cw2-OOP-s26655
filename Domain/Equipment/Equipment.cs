namespace Solution.Domain.Equipment;

public abstract class Equipment
{
    protected Equipment(string id, string name)
    {
        Id = id;
        Name = name;
        IsAvailable = true;
        IsMarkedUnavailable = false;
        UnavailableReason = string.Empty;
    }

    public string Id { get; }
    public string Name { get; private set; }
    public bool IsAvailable { get; private set; }
    public bool IsMarkedUnavailable { get; private set; }
    public string UnavailableReason { get; private set; }

    public void MarkAsUnavailable(string reason)
    {
        IsAvailable = false;
        IsMarkedUnavailable = true;
        UnavailableReason = reason;
    }

    public void MarkAsAvailable()
    {
        IsAvailable = true;
        IsMarkedUnavailable = false;
        UnavailableReason = string.Empty;
    }

    public void SetAvailability(bool isAvailable)
    {
        IsAvailable = isAvailable;
    }

    public override string ToString()
    {
        var status = IsAvailable ? "Available" : "Unavailable";
        return $"{Id} | {Name} | {status}";
    }
}