namespace Solution.Domain.Users;

public abstract class User
{
    protected User(string id, string firstName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }

    public string Id { get; }
    public string FirstName { get; }
    public string LastName { get; }

    public abstract string UserType { get; }

    public string FullName => $"{FirstName} {LastName}";

    public override string ToString()
    {
        return $"{Id} | {FullName} | Type: {UserType}";
    }
}