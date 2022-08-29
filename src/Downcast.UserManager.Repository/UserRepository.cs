using AutoMapper;

using Downcast.UserManager.Model;
using Downcast.UserManager.Repository.Internal;

using UpdateUser = Downcast.UserManager.Repository.Domain.UpdateUser;

namespace Downcast.UserManager.Repository;

internal class UserRepository : IUserRepository
{
    private readonly IMapper _mapper;
    private readonly IUserRepositoryInternal _repo;

    public UserRepository(IMapper mapper, IUserRepositoryInternal repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<User> Get(string userId)
    {
        Domain.User user = await _repo.Get(userId).ConfigureAwait(false);
        return _mapper.Map<User>(user);
    }

    public async Task<User> GetByEmail(string email)
    {
        Domain.User user = await _repo.GetByEmail(email).ConfigureAwait(false);
        return _mapper.Map<User>(user);
    }

    public Task Delete(string userId)
    {
        return _repo.Delete(userId);
    }

    public Task<int> CountByEmail(string email)
    {
        return _repo.CountByEmail(email);
    }

    public async Task<User> Create(CreateUserInputModel userInputModel)
    {
        Domain.User domainUser = _mapper.Map<Domain.User>(userInputModel);
        Domain.User createdUser = await _repo.Create(domainUser).ConfigureAwait(false);
        return _mapper.Map<User>(createdUser);
    }

    public Task Update(string userId, UpdateUserInputModel userInputModel)
    {
        UpdateUser domainUser = _mapper.Map<UpdateUser>(userInputModel);
        return _repo.Update(userId, domainUser);
    }

    public Task UpdatePasswordInfo(string userId, PasswordInfo passwordInfo)
    {
        Domain.PasswordInfo domainPasswordInfo = _mapper.Map<Domain.PasswordInfo>(passwordInfo);
        return _repo.UpdatePasswordInfo(userId, domainPasswordInfo);
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