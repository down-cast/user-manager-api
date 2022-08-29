using Downcast.UserManager.Model;

using CreateUserInputModel = Downcast.UserManager.Model.Input.CreateUserInputModel;

namespace Downcast.UserManager;

public interface IUserManager
{
    Task<User> CreateUser(CreateUserInputModel userInputModel);
    Task DeleteUser(string userId);
    Task<User> GetUser(string userId);
    Task<User> GetUserByEmail(string email);
    Task UpdateUser(string userId, UpdateUserInputModel updateUserInputModel);
    Task UpdateUserPassword(string userId, string password);

    Task AddRoles(string userId, string[] roles);
    Task RemoveRoles(string userId, string[] roles);
}