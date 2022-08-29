using Downcast.UserManager.Cryptography;
using Downcast.UserManager.Model;
using Downcast.UserManager.Model.Input;
using Downcast.UserManager.Repository;

using Microsoft.Extensions.Logging;

namespace Downcast.UserManager.Authentication;

public class AuthenticationManager : IAuthenticationManager
{
    private readonly IPasswordManager _passwordManager;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<AuthenticationManager> _logger;

    public AuthenticationManager(
        IPasswordManager passwordManager,
        IUserRepository userRepository,
        ILogger<AuthenticationManager> logger)
    {
        _passwordManager = passwordManager;
        _userRepository = userRepository;
        _logger = logger;
    }


    public async Task<bool> ValidateCredentials(AuthenticationRequest auth)
    {
        User? user = await GetUserByEmailSafe(auth.Email).ConfigureAwait(false);

        if (user is not { PasswordInfo: { } } || !_passwordManager.IsPasswordValid(auth.Password, user.PasswordInfo))
        {
            return false;
        }

        await UpdatePasswordIfNeeded(user.Id, auth.Password, user.PasswordInfo).ConfigureAwait(false);
        return true;
    }

    private async Task UpdatePasswordIfNeeded(string userId, string password, PasswordInfo passwordInfo)
    {
        if (_passwordManager.IsPasswordSecurityOutdated(passwordInfo))
        {
            PasswordInfo newPasswordInfo = _passwordManager.HashPassword(password);
            await _userRepository.UpdatePasswordInfo(userId, newPasswordInfo).ConfigureAwait(false);
        }
    }


    private async Task<User?> GetUserByEmailSafe(string email)
    {
        try
        {
            return await _userRepository.GetByEmail(email).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while trying to get user with {Email}", email);
            return null;
        }
    }
}