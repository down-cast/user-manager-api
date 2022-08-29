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

    public async Task<User> Create(CreateUserInputModel userInputModel)
    {
        Domain.User domainUser = _mapper.Map<Domain.User>(userInputModel);
        Domain.User createdUser = await _repo.Create(domainUser).ConfigureAwait(false);
        return _mapper.Map<User>(createdUser);
    }

    public Task Update(string id, UpdateUserInputModel userInputModel)
    {
        UpdateUser domainUser = _mapper.Map<UpdateUser>(userInputModel);
        return _repo.Update(id, domainUser);
    }

    public Task UpdatePasswordInfo(string id, PasswordInfo passwordInfo)
    {
        Domain.PasswordInfo domainPasswordInfo = _mapper.Map<Domain.PasswordInfo>(passwordInfo);
        return _repo.UpdatePasswordInfo(id, domainPasswordInfo);
    }

    public Task AddRoles(string id, params string[] roles)
    {
        return _repo.AddRoles(id, roles);
    }

    public Task RemoveRoles(string id, params string[] roles)
    {
        return _repo.RemoveRoles(id, roles);
    }
}