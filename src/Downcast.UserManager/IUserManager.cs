using Downcast.UserManager.Model;
using Downcast.UserManager.Model.Input;

namespace Downcast.UserManager;

public interface IUserManager
{
    Task<User> CreateUser(CreateUserInputModel userInputModel);
    Task DeleteUser(string id);

    Task<User> GetUser(string id);
}