namespace Solution.Domain.Users;

public class Employee : User
{
    public Employee(string id, string firstName, string lastName)
        : base(id, firstName, lastName)
    {
    }

    public override string UserType => "Employee";
}