using Downcast.UserManager.Repository.Domain;
using Downcast.UserManager.Repository.Internal;

using MapsterMapper;

using PasswordInfo = Downcast.UserManager.Model.PasswordInfo;
using User = Downcast.UserManager.Model.User;

namespace Downcast.UserManager.Repository;

internal class UserRepository : IUserRepository
{
    private readonly IMapper _mapper;
    private readonly IUserRepositoryInternal _repo;

    public UserRepository(IMapper mapper, IUserRepositoryInternal repo)
    {
        _mapper                 = mapper;
        _repo = repo;
    }


    public async Task<User> Get(string id)
    {
        Domain.User user = await _repo.Get(id).ConfigureAwait(false);
        return _mapper.Map<User>(user);
    }

    public async Task<User> GetByEmail(string email)
    {
        Domain.User user = await _repo.GetByEmail(email).ConfigureAwait(false);
        return _mapper.Map<User>(user);
    }

    public Task Delete(string id)
    {
        return _repo.Delete(id);
    }

    public Task<int> CountByEmail(string email)
    {
        return _repo.CountByEmail(email);
    }

    public Task Create(User user)
    {
        Domain.User domainUser = _mapper.Map<Domain.User>(user);
        return _repo.Create(domainUser);
    }

    public Task Update(string userId, User user)
    {
        UpdateUser domainUser = _mapper.Map<UpdateUser>(user);
        return _repo.Update(userId, domainUser);
    }

    public Task UpdatePasswordInfo(string userId, PasswordInfo passwordInfo)
    {
        Domain.PasswordInfo domainPasswordInfo = _mapper.Map<Domain.PasswordInfo>(passwordInfo);
        return _repo.UpdatePasswordInfo(userId, domainPasswordInfo);
    }

    public Task SetEmailValidated(string userId, bool validated)
    {
        return _repo.SetEmailValidated(userId, validated);
    }

    public Task AddRoles(string userId, params string[] roles)
    {
        return _repo.AddRoles(userId, roles);
    }

    public Task RemoveRoles(string userId, params string[] roles)
    {
        return _repo.RemoveRoles(userId, roles);
    }
}