using Downcast.UserManager.Repository.Domain;

namespace Downcast.UserManager.Repository.Internal;

internal interface IUserRepositoryInternal
{
    Task<User> Get(string userId);
    Task<User> GetByEmail(string email);
    Task Delete(string userId);
    Task<int> CountByEmail(string email);
    Task<User> Create(User user);
    Task Update(string userId, UpdateUser user);
    Task UpdatePasswordInfo(string userId, PasswordInfo passwordInfo);
    Task AddRoles(string userId, params string[] roles);
    Task RemoveRoles(string userId, params string[] roles);
}