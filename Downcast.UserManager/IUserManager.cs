using Downcast.UserManager.Model;
using Downcast.UserManager.Model.Dtos;

namespace Downcast.UserManager
{
    public interface IUserManager
    {
        Task<string> CreateUser(CreateUser userInfo);
        Task<UserDto> GetUserById(string userId);
        Task DeleteUserById(string userId);
        Task UpdateUser(UserDto userInfo);
        Task<string> AuthenticateUser(AuthenticateUser authenticateUser);
    }
}
