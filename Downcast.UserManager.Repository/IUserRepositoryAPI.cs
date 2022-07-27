using Downcast.UserManager.Model;
using Downcast.UserManager.Model.Dtos;
using Downcast.UserManager.PasswordManager;

namespace Downcast.UserManager.Repository
{
    public interface IUserRepositoryAPI
    {
        Task<string> CreateUser(CreateUser userInfo, PasswordInfo passwordInfo);
        Task<UserDto> GetUserById(string userId);
        Task<UserDto> GetUserByEmail(string email);
        Task<PasswordInfo> UserPasswordInfo(AuthenticateUser authenticateUser);
        Task DeleteUserById(string userId);
        Task UpdateUser(UserDto userInfo);
        Task UpdateUser(string email, PasswordInfo passwordInfo);
    }
}
