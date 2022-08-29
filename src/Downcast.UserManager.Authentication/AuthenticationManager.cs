using Downcast.Common.Errors;
using Downcast.SessionManager.SDK.Authentication.Extensions;
using Downcast.SessionManager.SDK.Client;
using Downcast.SessionManager.SDK.Client.Model;
using Downcast.UserManager.Authentication.Model;
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
    private readonly ISessionManagerClient _sessionManagerClient;
    private readonly ILogger<AuthenticationManager> _logger;

    public AuthenticationManager(
        IPasswordManager passwordManager,
        IUserRepository userRepository,
        ISessionManagerClient sessionManagerClient,
        ILogger<AuthenticationManager> logger)
    {
        _passwordManager = passwordManager;
        _userRepository = userRepository;
        _sessionManagerClient = sessionManagerClient;
        _logger = logger;
    }


    public async Task<AuthenticationResult> Authenticate(AuthenticationRequest auth)
    {
        User? user = await GetUserByEmailSafe(auth.Email).ConfigureAwait(false);

        if (user is not { PasswordInfo: not null })
        {
            _logger.LogInformation("User with {Email} does not have a password defined", auth.Email);
            throw new DcException(ErrorCodes.AuthenticationFailed);
        }

        if (!_passwordManager.IsPasswordValid(auth.Password, user.PasswordInfo))
        {
            _logger.LogInformation("User with {Email} failed to authenticate", auth.Email);
            throw new DcException(ErrorCodes.AuthenticationFailed);
        }

        await UpdatePasswordIfNeeded(user.Id, auth.Password, user.PasswordInfo).ConfigureAwait(false);
        return await CreateAuthenticationResult(user).ConfigureAwait(false);
    }

    private async Task UpdatePasswordIfNeeded(string userId, string password, PasswordInfo passwordInfo)
    {
        if (_passwordManager.IsPasswordSecurityOutdated(passwordInfo))
        {
            PasswordInfo newPasswordInfo = _passwordManager.HashPassword(password);
            await _userRepository.UpdatePasswordInfo(userId, newPasswordInfo).ConfigureAwait(false);
        }
    }

    private async Task<AuthenticationResult> CreateAuthenticationResult(User user)
    {
        try
        {
            TokenResult tokenResult = await _sessionManagerClient
                .CreateSessionToken(GetClaims(user))
                .ConfigureAwait(false);

            return new AuthenticationResult
            {
                ExpirationDate = tokenResult.ExpirationDate,
                Token = tokenResult.Token
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error has occurred while creating a session token for {Email}", user.Email);
            throw new DcException(ErrorCodes.AuthenticationFailed);
        }
    }

    private static Dictionary<string, object> GetClaims(User user)
    {
        return new Dictionary<string, object>
        {
            { ClaimNames.Email, user.Email },
            { ClaimNames.Role, user.Roles },
            { ClaimNames.UserId, user.Id },
            { ClaimNames.DisplayName, user.DisplayName ?? "" },
            { ClaimNames.ProfilePictureUri, user.ProfilePictureUri ?? "" }
        };
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