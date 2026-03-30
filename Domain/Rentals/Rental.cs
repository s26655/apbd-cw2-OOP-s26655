namespace Solution.Domain.Rentals;

public class Rental
{
    public Rental(
        string id,
        string userId,
        string equipmentId,
        DateTime rentalDate,
        DateTime dueDate)
    {
        Id = id;
        UserId = userId;
        EquipmentId = equipmentId;
        RentalDate = rentalDate;
        DueDate = dueDate;
        ReturnedAt = null;
        Penalty = 0m;
    }

    public string Id { get; }
    public string UserId { get; }
    public string EquipmentId { get; }
    public DateTime RentalDate { get; }
    public DateTime DueDate { get; }
    public DateTime? ReturnedAt { get; private set; }
    public decimal Penalty { get; private set; }

    public bool IsReturned => ReturnedAt.HasValue;
    public bool IsOverdue => !IsReturned && DateTime.Now > DueDate;
    public bool WasReturnedOnTime => IsReturned && ReturnedAt.Value <= DueDate;

    public void Return(DateTime returnDate, decimal penalty)
    {
        ReturnedAt = returnDate;
        Penalty = penalty;
    }

    public override string ToString()
    {
        var returnInfo = IsReturned
            ? $"Returned: {ReturnedAt:yyyy-MM-dd}"
            : "Returned: not yet";

        return $"{Id} | User: {UserId} | Equipment: {EquipmentId} | Rental date: {RentalDate:yyyy-MM-dd} | Due date: {DueDate:yyyy-MM-dd} | {returnInfo} | Penalty: {Penalty}";
    }
}