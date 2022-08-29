using Downcast.UserManager.Model;

namespace Downcast.UserManager.Repository;

public interface IUserRepository
{
    Task<User> Get(string userId);
    Task<User> GetByEmail(string email);
    Task Delete(string userId);
    Task<int> CountByEmail(string email);
    Task<User> Create(CreateUserInputModel userInputModel);
    Task Update(string userId, UpdateUserInputModel userInputModel);
    Task UpdatePasswordInfo(string userId, PasswordInfo passwordInfo);
    Task AddRoles(string userId, params string[] roles);
    Task RemoveRoles(string userId, params string[] roles);
}