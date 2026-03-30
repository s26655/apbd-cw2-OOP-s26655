namespace Solution.Services;

public class PenaltyPolicy
{
    private const decimal DailyPenaltyRate = 10m;

    public decimal CalculatePenalty(DateTime dueDate, DateTime returnDate)
    {
        var dueDateOnly = dueDate.Date;
        var returnDateOnly = returnDate.Date;

        if (returnDateOnly <= dueDateOnly)
        {
            return 0m;
        }

        var lateDays = (returnDateOnly - dueDateOnly).Days;
        return lateDays * DailyPenaltyRate;
    }
}