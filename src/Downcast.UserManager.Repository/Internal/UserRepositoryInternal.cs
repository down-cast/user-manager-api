using Downcast.Common.Errors;
using Downcast.UserManager.Repository.Domain;

using Firestore.Typed.Client;

using Google.Cloud.Firestore;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Downcast.UserManager.Repository.Internal;

internal class UserRepositoryInternal : IUserRepositoryInternal
{
    private readonly ILogger<UserRepositoryInternal> _logger;
    private readonly TypedCollectionReference<User> _collection;

    public UserRepositoryInternal(
        IOptions<UserRepositoryOptions> options,
        ILogger<UserRepositoryInternal> logger,
        FirestoreDb firestoreDb)
    {
        _logger = logger;
        _collection = firestoreDb.TypedCollection<User>(options.Value.Collection);
    }


    public async Task<User> Get(string id)
    {
        TypedDocumentSnapshot<User> user = await _collection.Document(id).GetSnapshotAsync().ConfigureAwait(false);
        if (user.Exists)
        {
            return user.Object;
        }

        _logger.LogWarning("User with id {Id} not found", id);
        throw new DcException(ErrorCodes.EntityNotFound, "User not found");
    }

    public async Task<User> GetByEmail(string email)
    {
        TypedQuerySnapshot<User> users = await _collection
            .WhereEqualTo(user => user.Email, email)
            .Limit(1)
            .GetSnapshotAsync()
            .ConfigureAwait(false);

        if (users.Count is 1)
        {
            return users.Documents[0].Object;
        }

        _logger.LogDebug("User with email {Email} not found", email);
        throw new DcException(ErrorCodes.EntityNotFound, "User not found");
    }

    public async Task<int> CountByEmail(string email)
    {
        TypedQuerySnapshot<User> snapshot = await _collection
            .WhereEqualTo(user => user.Email, email)
            .GetSnapshotAsync()
            .ConfigureAwait(false);

        return snapshot.Count;
    }

    public Task Delete(string id)
    {
        return _collection.Document(id).DeleteAsync();
    }

    public Task Create(User user)
    {
        return _collection.Document(user.Id).CreateAsync(user);
    }

    public Task Update(string userId, UpdateUser user)
    {
        var updateDefinition = new UpdateDefinition<User>();
        SetProfilePictureUri(user, updateDefinition);
        SetDisplayName(user, updateDefinition);
        SetFacebookLink(user, updateDefinition);
        SetLinkedInLink(user, updateDefinition);
        SetTwitterLink(user, updateDefinition);
        SetStackOverflowLink(user, updateDefinition);
        SetGitHubLink(user, updateDefinition);

        return _collection.Document(userId).UpdateAsync(updateDefinition, Precondition.MustExist);
    }

    public Task UpdatePasswordInfo(string userId, PasswordInfo passwordInfo)
    {
        return _collection
            .Document(userId)
            .UpdateAsync(user => user.PasswordInfo, passwordInfo, Precondition.MustExist);
    }

    public Task SetEmailValidated(string userId, bool validated)
    {
        return _collection
            .Document(userId)
            .UpdateAsync(user => user.EmailValidated, validated, Precondition.MustExist);
    }

    public Task AddRoles(string userId, params string[] roles)
    {
        return _collection
            .Document(userId)
            .UpdateAsync(user => user.Roles, FieldValue.ArrayUnion(roles), Precondition.MustExist);
    }

    public Task RemoveRoles(string userId, params string[] roles)
    {
        return _collection
            .Document(userId)
            .UpdateAsync(user => user.Roles, FieldValue.ArrayRemove(roles), Precondition.MustExist);
    }

    private static void SetGitHubLink(UpdateUser user, UpdateDefinition<User> updateDefinition)
    {
        if (user is { SocialLinks.GitHub: not null })
        {
            updateDefinition.Set(u => u.SocialLinks!.GitHub, user.SocialLinks.GitHub);
        }
    }

    private static void SetStackOverflowLink(UpdateUser user, UpdateDefinition<User> updateDefinition)
    {
        if (user is { SocialLinks.StackOverflow: not null })
        {
            updateDefinition.Set(u => u.SocialLinks!.StackOverflow, user.SocialLinks.StackOverflow);
        }
    }

    private static void SetTwitterLink(UpdateUser user, UpdateDefinition<User> updateDefinition)
    {
        if (user is { SocialLinks.Twitter: not null })
        {
            updateDefinition.Set(u => u.SocialLinks!.Twitter, user.SocialLinks.Twitter);
        }
    }

    private static void SetLinkedInLink(UpdateUser user, UpdateDefinition<User> updateDefinition)
    {
        if (user is { SocialLinks.LinkedIn: not null })
        {
            updateDefinition.Set(u => u.SocialLinks!.LinkedIn, user.SocialLinks.LinkedIn);
        }
    }

    private static void SetFacebookLink(UpdateUser user, UpdateDefinition<User> updateDefinition)
    {
        if (user is { SocialLinks.Facebook: not null })
        {
            updateDefinition.Set(u => u.SocialLinks!.Facebook, user.SocialLinks.Facebook);
        }
    }

    private static void SetDisplayName(UpdateUser user, UpdateDefinition<User> updateDefinition)
    {
        if (user.DisplayName is not null)
        {
            updateDefinition.Set(u => u.DisplayName, user.DisplayName);
        }
    }

    private static void SetProfilePictureUri(UpdateUser user, UpdateDefinition<User> updateDefinition)
    {
        if (user.ProfilePictureUri is not null)
        {
            updateDefinition.Set(u => u.ProfilePictureUri, user.ProfilePictureUri);
        }
    }
}