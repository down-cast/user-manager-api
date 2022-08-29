using Downcast.UserManager.Model;
using Downcast.UserManager.Model.Input;

namespace Downcast.UserManager.Repository;

public interface IUserRepository
{
    Task<User> Get(string id);
    Task<User> GetByEmail(string email);
    Task Delete(string id);
    Task<int> CountByEmail(string email);
    Task<User> Create(CreateUser user);
    Task Update(string userId, User user);
    Task UpdatePasswordInfo(string userId, PasswordInfo passwordInfo);
    Task SetEmailValidated(string userId, bool validated);
    Task AddRoles(string userId, params string[] roles);
    Task RemoveRoles(string userId, params string[] roles);
}