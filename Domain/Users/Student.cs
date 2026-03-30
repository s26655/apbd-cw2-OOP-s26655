namespace Solution.Domain.Users;

public class Student : User
{
    public Student(string id, string firstName, string lastName)
        : base(id, firstName, lastName)
    {
    }

    public override string UserType => "Student";
}