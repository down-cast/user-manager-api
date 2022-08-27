using Downcast.UserManager.Model;

namespace Downcast.UserManager.Repository;

public interface IUserRepository
{
    Task<User> Get(string id);
    Task<User> GetByEmail(string email);
    Task Delete(string id);
    Task<int> CountByEmail(string email);
    Task Create(User user);
    Task Update(string userId, User user);
    Task UpdatePasswordInfo(string userId, PasswordInfo passwordInfo);
    Task SetEmailValidated(string userId, bool validated);
    Task AddRoles(string userId, params string[] roles);
    Task RemoveRoles(string userId, params string[] roles);
}