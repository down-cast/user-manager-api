using Downcast.UserManager.Model;

using CreateUserInputModel = Downcast.UserManager.Model.Input.CreateUserInputModel;

namespace Downcast.UserManager;

public interface IUserManager
{
    Task<User> CreateUser(CreateUserInputModel userInputModel);
    Task DeleteUser(string id);
    Task<User> GetUser(string id);
    Task<User> GetUserByEmail(string email);
    Task UpdateUser(string id, UpdateUserInputModel updateUserInputModel);
    Task UpdateUserPassword(string id, string password);

    Task AddRoles(string id, string[] roles);
    Task RemoveRoles(string id, string[] roles);
}