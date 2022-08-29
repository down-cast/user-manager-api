using Downcast.UserManager.Repository.Domain;

namespace Downcast.UserManager.Repository.Internal;

internal interface IUserRepositoryInternal
{
    Task<User> Get(string id);
    Task<User> GetByEmail(string email);
    Task Delete(string id);
    Task<int> CountByEmail(string email);
    Task<User> Create(User user);
    Task Update(string id, UpdateUser user);
    Task UpdatePasswordInfo(string id, PasswordInfo passwordInfo);
    Task AddRoles(string id, params string[] roles);
    Task RemoveRoles(string id, params string[] roles);
}