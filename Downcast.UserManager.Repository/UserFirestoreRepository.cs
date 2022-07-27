using Downcast.Common.Errors;
using Downcast.UserManager.Model;
using Downcast.UserManager.Model.Dtos;
using Downcast.UserManager.PasswordManager;
using Downcast.UserManager.Repository.Domain;

using Google.Cloud.Firestore;

using Mapster;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Downcast.UserManager.Repository
{
    internal class UserFirestoreRepository : IUserRepository
    {
        private readonly CollectionReference _dbCollection;
        private readonly ILogger<UserFirestoreRepository> _logger;
        private readonly IPasswordManager _passwordManager;
        //private readonly ISessionManagerClient _passwordManager;

        public UserFirestoreRepository(
            IOptions<FirestoreRepositoryOptions> repositoryOptions,
            FirestoreDb database,
            ILogger<UserFirestoreRepository> logger,
            IPasswordManager passwordManager)
        {
            _dbCollection = database.Collection(repositoryOptions.Value.Collection);
            _logger = logger;
            _passwordManager = passwordManager;
        }

        public async Task<string> CreateUser(User user, Domain.PasswordInfo passwordInfo)
        {
            QuerySnapshot snapshot = await _dbCollection
                .WhereEqualTo(nameof(user.Email), user.Email)
                .GetSnapshotAsync()
                .ConfigureAwait(false);
            if (snapshot.Any())
            {
                _logger.LogDebug("User with {email} already exists", user.Email);
                throw new DcException(ErrorCodes.ClaimNotFound, $"User with email {user.Email} already exists"); //TODO
            }
            //user.PasswordInfo = passwordInfo;
            user.PasswordHash = passwordInfo.PasswordHash;
            user.PasswordSalt = passwordInfo.PasswordSalt;
            user.PasswordHashingIterations = passwordInfo.Iterations;
            DocumentReference userDocument = await _dbCollection.AddAsync(user).ConfigureAwait(false);
            _logger.LogDebug("Created user with {UserId} successfully", userDocument.Id);
            return userDocument.Id;
        }

        public async Task<UserDto> GetUserById(string userId)
        {
            DocumentSnapshot snapshot = await _dbCollection
                .Document(userId)
                .GetSnapshotAsync()
                .ConfigureAwait(false);

            if (snapshot.Exists)
            {
                return snapshot.ConvertTo<User>().Adapt<UserDto>();
            }

            _logger.LogDebug("User with {UserId} was not found", userId);
            throw new DcException(ErrorCodes.EntityNotFound, "Could not find user");
        }

        public async Task<User> GetUserByEmail(string email)
        {
            QuerySnapshot? snapshot = await _dbCollection
                .WhereEqualTo("Email", email) //TODO
                .GetSnapshotAsync()
                .ConfigureAwait(false);
            if (!snapshot.Any())
            {
                _logger.LogDebug("User with {Email} was not found", email);
                throw new DcException(ErrorCodes.EntityNotFound, "Could not find user");
            }
            return snapshot.Single().ConvertTo<User>();
        }

        public async Task DeleteUserById(string userId)
        {
            DocumentSnapshot snapshot = await _dbCollection
                .Document(userId)
                .GetSnapshotAsync()
                .ConfigureAwait(false);

            if (snapshot.Exists)
            {
                await snapshot.Reference.DeleteAsync().ConfigureAwait(false);
                _logger.LogDebug("Deleted user with {UserId} successfully", userId);
                return;
            }
            _logger.LogDebug("User with {UserId} was not found", userId);
            throw new DcException(ErrorCodes.EntityNotFound, "Could not find user");
        }

        public async Task UpdateUser(UserDto userInfo)
        {
            DocumentReference documentReference = _dbCollection
                .Document(userInfo.UserId);

            Dictionary<string, object> dataToUpdate = GetNotNullProperties(userInfo);

            if (documentReference is not null)
            {
                await documentReference.UpdateAsync(dataToUpdate);
                return;
            }

            _logger.LogDebug("User with {UserId} was not found", userInfo.UserId);
            throw new DcException(ErrorCodes.EntityNotFound, "Could not find user");
        }

        public async Task UpdateUser(string email, Domain.PasswordInfo passwordInfo)
        {
            QuerySnapshot? snapshot = await _dbCollection
                .WhereEqualTo("Email", email) //TODO
                .GetSnapshotAsync()
                .ConfigureAwait(false);
            
            Dictionary<string, object> dataToUpdate = new()
            {
                { nameof(passwordInfo.PasswordHash), passwordInfo.PasswordHash },
                { nameof(passwordInfo.PasswordSalt), passwordInfo.PasswordSalt },
                { nameof(passwordInfo.Iterations), passwordInfo.Iterations }
            };
            await snapshot.Single().Reference.UpdateAsync(dataToUpdate);
        }

        private Dictionary<string, object> GetNotNullProperties(UserDto useDto)
        {
            return useDto
                  .GetType()
                  .GetProperties()
                  .Where(prop => prop.GetValue(useDto) is not null && prop.Name is not nameof(useDto.UserId))
                  .ToDictionary(prop => prop.Name, prop => prop.GetValue(useDto));
        }
    }
}
