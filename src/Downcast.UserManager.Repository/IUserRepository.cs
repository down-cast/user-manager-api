using Downcast.UserManager.Model;
using Downcast.UserManager.Model.Input;

namespace Downcast.UserManager.Repository;

public interface IUserRepository
{
    Task<UserWithPassword> Get(string userId);
    Task<UserWithPassword> GetByEmail(string email);
    Task Delete(string userId);
    Task<int> CountByEmail(string email);
    Task<UserWithPassword> Create(CreateUser user);
    Task Update(string userId, UpdateUserInputModel userInputModel);
    Task UpdatePasswordInfo(string userId, PasswordInfo passwordInfo);
    Task AddRoles(string userId, params string[] roles);
    Task RemoveRoles(string userId, params string[] roles);
}