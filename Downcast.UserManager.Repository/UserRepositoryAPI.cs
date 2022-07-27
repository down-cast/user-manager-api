using Downcast.UserManager.Model;
using Downcast.UserManager.Model.Dtos;
using Downcast.UserManager.Repository.Domain;

using Mapster;

namespace Downcast.UserManager.Repository
{
    internal class UserRepositoryAPI : IUserRepositoryAPI
    {
        private readonly IUserRepository _userFirestoreRepository;

        public UserRepositoryAPI(IUserRepository userFirestoreRepository)
        {
            _userFirestoreRepository = userFirestoreRepository;
        }

        public Task<string> CreateUser(CreateUser userInfo, PasswordManager.PasswordInfo passwordInfo)
        {
            return _userFirestoreRepository.CreateUser(userInfo.Adapt<User>(), passwordInfo.Adapt<PasswordInfo>());
        }

        public Task<UserDto> GetUserById(string userId)
        {
            return _userFirestoreRepository.GetUserById(userId);
        }
        public async Task<UserDto> GetUserByEmail(string email)
        {
            User user = await _userFirestoreRepository.GetUserByEmail(email);
            UserDto? temp = user.Adapt<UserDto>();
            return temp;
        }

        public Task DeleteUserById(string userId)
        {
            return _userFirestoreRepository.DeleteUserById(userId);
        }

        public Task UpdateUser(UserDto userInfo)
        {
            return _userFirestoreRepository.UpdateUser(userInfo);
        }

        public Task UpdateUser(string email, PasswordManager.PasswordInfo passwordInfo)
        {
            return _userFirestoreRepository.UpdateUser(email, passwordInfo.Adapt<PasswordInfo>());
        }

        public async Task<PasswordManager.PasswordInfo> UserPasswordInfo(AuthenticateUser authenticateUser)
        {
            User user = await _userFirestoreRepository.GetUserByEmail(authenticateUser.Email);
            PasswordManager.PasswordInfo passwordInfo = new()
            {
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt,
                Iterations = user.PasswordHashingIterations
            };
            return passwordInfo;
        }
    }
}
