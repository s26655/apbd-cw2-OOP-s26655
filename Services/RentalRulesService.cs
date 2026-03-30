using Solution.Domain.Users;

namespace Solution.Services;

public class RentalRulesService
{
    public int GetActiveRentalLimit(User user)
    {
        return user switch
        {
            Student => 2,
            Employee => 5,
            _ => 0
        };
    }
}