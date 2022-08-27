using Downcast.UserManager.Repository.Domain;

namespace Downcast.UserManager.Repository.Internal;

internal interface IUserRepositoryInternal
{
    Task<User> Get(string id);
    Task<User> GetByEmail(string email);
    Task Delete(string id);
    Task<int> CountByEmail(string email);
    Task Create(User user);
    Task Update(string userId, UpdateUser user);
    Task UpdatePasswordInfo(string userId, PasswordInfo passwordInfo);
    Task SetEmailValidated(string userId, bool validated);
    Task AddRoles(string userId, params string[] roles);
    Task RemoveRoles(string userId, params string[] roles);
}