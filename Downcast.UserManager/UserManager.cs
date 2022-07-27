
using Downcast.Common.Errors;
using Downcast.SessionManager.SDK.Client;
using Downcast.UserManager.Model;
using Downcast.UserManager.Model.Dtos;
using Downcast.UserManager.PasswordManager;
using Downcast.UserManager.Repository;

using Microsoft.Extensions.Logging;

namespace Downcast.UserManager
{
    public class UserManager : IUserManager
    {
        private readonly IPasswordManager _passwordManager;
        private readonly IUserRepositoryAPI _userRepositoryAPI;
        private readonly ILogger<UserManager> _logger;
        private readonly ISessionManagerClient _client;

        public UserManager(
            IPasswordManager passwordManager,
            IUserRepositoryAPI userRepositoryAPI,
            ILogger<UserManager> logger,
            ISessionManagerClient client)
        {
            _passwordManager = passwordManager;
            _userRepositoryAPI = userRepositoryAPI;
            _logger = logger;
            _client = client;
        }
        public Task<string> CreateUser(CreateUser userInfo)
        {
            PasswordInfo passwordInfo = _passwordManager.HashPassword(userInfo.Password);
            return _userRepositoryAPI.CreateUser(userInfo, passwordInfo);
        }

        public Task<UserDto> GetUserById(string userId)
        {
            return _userRepositoryAPI.GetUserById(userId);
        }

        public Task DeleteUserById(string userId)
        {
            return _userRepositoryAPI.DeleteUserById(userId);
        }

        public Task UpdateUser(UserDto userInfo)
        {
            return _userRepositoryAPI.UpdateUser(userInfo);
        }

        public async Task<string> AuthenticateUser(AuthenticateUser authenticateUser)
        {
            PasswordInfo userPasswordInfo = await _userRepositoryAPI.UserPasswordInfo(authenticateUser);
            (bool verified, bool upgradeNeeded) = _passwordManager.CheckPasswordHash(authenticateUser.Password, userPasswordInfo);
            if (!verified)
            {
                _logger.LogDebug("Password is incorrect");
                throw new DcException(ErrorCodes.BadRequest, "Password is incorrect");
            }
            UserDto userDto = await _userRepositoryAPI.GetUserByEmail(authenticateUser.Email);
            Dictionary<string, string> userClaims = new Dictionary<string, string>()
                {
                    { nameof(userDto.UserId), userDto.UserId },
                    { nameof(userDto.Name), userDto.Name },
                    { nameof(userDto.Email), userDto.Email },
                    { "Role", "EXAMPLE" }
                };
            if (upgradeNeeded)
            {
                PasswordInfo? updatedPasswordInfo = _passwordManager.HashPassword(authenticateUser.Password);
                await _userRepositoryAPI.UpdateUser(authenticateUser.Email, updatedPasswordInfo);               
                string token = await _client.CreateSessionToken(userClaims);
                return token;
            }
            else
            {
                string? token = await _client.CreateSessionToken(userClaims);
                return token;
            }
        }
    }
}
