using Downcast.Common.Errors;
using Downcast.UserManager.Cryptography;
using Downcast.UserManager.Model;
using Downcast.UserManager.Model.Input;
using Downcast.UserManager.Repository;

using Microsoft.Extensions.Logging;

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

        var userToCreate = new CreateUser
        {
            Email = userInputModel.Email,
            DisplayName = userInputModel.DisplayName,
            PasswordInfo = _passwordManager.HashPassword(userInputModel.Password)
        };
        return await _userRepository.Create(userToCreate).ConfigureAwait(false);
    }

    private async Task<bool> EmailAlreadyTaken(CreateUserInputModel createUserInputModel)
    {
        int count = await _userRepository.CountByEmail(createUserInputModel.Email).ConfigureAwait(false);
        return count > 0;
    }

    public Task DeleteUser(string id)
    {
        return _userRepository.Delete(id);
    }

    public Task<User> GetUser(string id)
    {
        return _userRepository.Get(id);
    }
}