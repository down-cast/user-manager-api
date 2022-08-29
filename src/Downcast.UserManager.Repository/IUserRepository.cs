using Downcast.UserManager.Model;

namespace Downcast.UserManager.Repository;

public interface IUserRepository
{
    Task<User> Get(string id);
    Task<User> GetByEmail(string email);
    Task Delete(string id);
    Task<int> CountByEmail(string email);
    Task<User> Create(CreateUserInputModel userInputModel);
    Task Update(string id, UpdateUserInputModel userInputModel);
    Task UpdatePasswordInfo(string id, PasswordInfo passwordInfo);
    Task AddRoles(string id, params string[] roles);
    Task RemoveRoles(string id, params string[] roles);
}