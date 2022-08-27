using Downcast.Common.Errors;
using Downcast.SessionManager.SDK.Authentication.Extensions;
using Downcast.SessionManager.SDK.Client;
using Downcast.SessionManager.SDK.Client.Model;
using Downcast.UserManager.Authentication.Model;
using Downcast.UserManager.Cryptography;
using Downcast.UserManager.Model;
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
        _passwordManager      = passwordManager;
        _userRepository       = userRepository;
        _sessionManagerClient = sessionManagerClient;
        _logger               = logger;
    }


    public async Task<AuthenticationResult> Authenticate(string email, string password)
    {
        User? user = await GetUserByEmailSafe(email).ConfigureAwait(false);
        if (user is not { PasswordInfo: not null } || _passwordManager.IsPasswordValid(password, user.PasswordInfo))
        {
            _logger.LogInformation("User with {Email} does not have a password defined", email);
            throw new DcException(ErrorCodes.AuthenticationFailed);
        }

        return await CreateAuthenticationResult(email, user).ConfigureAwait(false);
    }

    private async Task<AuthenticationResult> CreateAuthenticationResult(string email, User user)
    {
        TokenResult tokenResult = await _sessionManagerClient.CreateSessionToken(new Dictionary<string, object>
        {
            { ClaimNames.Email, email },
            { ClaimNames.Name, user.DisplayName ?? "" },
            { ClaimNames.Role, user.Roles },
            { ClaimNames.UserId, user.Id },
            { ClaimNames.DisplayName, user.DisplayName ?? "" },
            { ClaimNames.ProfilePictureUri, user.ProfilePictureUri ?? "" }
        }).ConfigureAwait(false);

        return new AuthenticationResult
        {
            ExpirationDate = tokenResult.ExpirationDate,
            Token          = tokenResult.Token
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