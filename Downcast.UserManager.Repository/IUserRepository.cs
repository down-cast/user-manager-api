using Downcast.UserManager.Model;
using Downcast.UserManager.Model.Dtos;
using Downcast.UserManager.Repository.Domain;

namespace Downcast.UserManager.Repository
{
    internal interface IUserRepository
    {
        Task<string> CreateUser(User user, Domain.PasswordInfo passwordInfo);
        Task<UserDto> GetUserById(string userId);
        Task<User> GetUserByEmail(string email);
        Task DeleteUserById(string userId);
        Task UpdateUser(UserDto userInfo);
        Task UpdateUser(string email, PasswordInfo passwordInfo);
    }
}
