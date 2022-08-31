using Downcast.Common.Errors;
using Downcast.UserManager.Cryptography;
using Downcast.UserManager.Model;
using Downcast.UserManager.Model.Input;
using Downcast.UserManager.Repository;

using Microsoft.Extensions.Logging;

using CreateUserInputModel = Downcast.UserManager.Model.Input.CreateUserInputModel;

namespace Downcast.UserManager;

public class UserManager : IUserManager
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordManager _passwordManager;
    private readonly ILogger<UserManager> _logger;

    public UserManager(IUserRepository userRepository, IPasswordManager passwordManager, ILogger<UserManager> logger)
    {
        _userRepository = userRepository;
        _passwordManager = passwordManager;
        _logger = logger;
    }

    public async Task<User> CreateUser(CreateUserInputModel userInputModel)
    {
        bool emailAlreadyTaken = await EmailAlreadyTaken(userInputModel).ConfigureAwait(false);
        if (emailAlreadyTaken)
        {
            _logger.LogInformation("{Email} already taken", userInputModel.Email);
            throw new DcException(ErrorCodes.EmailAlreadyTaken);
        }

        var userToCreate = new Model.CreateUser
        {
            Email = userInputModel.Email,
            DisplayName = userInputModel.DisplayName,
            PasswordInfo = _passwordManager.HashPassword(userInputModel.Password)
        };
        return await _userRepository.Create(userToCreate).ConfigureAwait(false);
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _userRepository.GetByEmail(email).ConfigureAwait(false);
    }

    private async Task<bool> EmailAlreadyTaken(CreateUserInputModel createUserInputModel)
    {
        int count = await _userRepository.CountByEmail(createUserInputModel.Email).ConfigureAwait(false);
        return count > 0;
    }

    public Task DeleteUser(string userId)
    {
        return _userRepository.Delete(userId);
    }

    public async Task<User> GetUser(string userId)
    {
        return await _userRepository.Get(userId).ConfigureAwait(false);
    }

    public Task UpdateUser(string userId, UpdateUserInputModel updateUserInputModel)
    {
        return _userRepository.Update(userId, updateUserInputModel);
    }

    public Task UpdateUserPassword(string userId, string password)
    {
        PasswordInfo passwordInfo = _passwordManager.HashPassword(password);
        return _userRepository.UpdatePasswordInfo(userId, passwordInfo);
    }

    public Task AddRoles(string userId, string[] roles)
    {
        return _userRepository.AddRoles(userId, roles);
    }

    public Task RemoveRoles(string userId, string[] roles)
    {
        return _userRepository.RemoveRoles(userId, roles);
    }
}