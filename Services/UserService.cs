using Solution.Common;
using Solution.Domain.Users;

namespace Solution.Services;

public class UserService
{
    private readonly List<User> _users = new();

    public OperationResult AddUser(User user)
    {
        if (_users.Any(u => u.Id == user.Id))
        {
            return OperationResult.Failure($"User with id {user.Id} already exists.");
        }

        _users.Add(user);
        return OperationResult.Success($"User {user.FullName} was added successfully.");
    }

    public IReadOnlyList<User> GetAllUsers()
    {
        return _users.AsReadOnly();
    }

    public User? GetById(string userId)
    {
        return _users.FirstOrDefault(u => u.Id == userId);
    }
}