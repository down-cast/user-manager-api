using Downcast.Common.Errors;
using Downcast.UserManager.Repository.Domain;

using Firestore.Typed.Client;
using Firestore.Typed.Client.Extensions;

using Google.Cloud.Firestore;

using Grpc.Core;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Downcast.UserManager.Repository.Internal;

internal class UserRepositoryInternal : IUserRepositoryInternal
{
    private readonly ILogger<UserRepositoryInternal> _logger;
    private readonly FirestoreDb _firestoreDb;
    private readonly TypedCollectionReference<User> _collection;
    private readonly CollectionReference _emailsCollection;

    public UserRepositoryInternal(
        IOptions<UserRepositoryOptions> options,
        ILogger<UserRepositoryInternal> logger,
        FirestoreDb firestoreDb)
    {
        _logger = logger;
        _firestoreDb = firestoreDb;
        _emailsCollection = firestoreDb.Collection(options.Value.EmailsCollection);
        _collection = firestoreDb.TypedCollection<User>(options.Value.UsersCollection);
    }


    public async Task<User> Get(string userId)
    {
        TypedDocumentSnapshot<User> snapshot = await _collection
            .Document(userId)
            .GetSnapshotAsync()
            .ConfigureAwait(false);

        if (snapshot.Exists)
        {
            return snapshot.RequiredObject;
        }

        _logger.LogWarning("User with {Id} not found", userId);
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

    public async Task Delete(string userId)
    {
        User user = await Get(userId).ConfigureAwait(false);
        await ExecuteDbOperation(() =>
        {
            WriteBatch batch = _firestoreDb.StartBatch();
            batch.Delete(_collection.Document(userId), Precondition.MustExist);
            batch.Delete(_emailsCollection.Document(user.Email), Precondition.MustExist);
            return batch.CommitAsync();
        }).ConfigureAwait(false);
    }


    public async Task<User> Create(User user)
    {
        TypedDocumentReference<User> userDoc = _collection.Document();
        DocumentReference emailDoc = _emailsCollection.Document(user.Email);
        await _firestoreDb.RunTransactionAsync(async transaction =>
        {
            DocumentSnapshot? emailSnap = await transaction
                .GetSnapshotAsync(emailDoc)
                .ConfigureAwait(false);

            if (emailSnap.Exists)
            {
                throw new DcException(ErrorCodes.EmailAlreadyTaken, $"Email {user.Email} already taken");
            }

            transaction.Create(userDoc, user);
            transaction.Create(emailDoc, new { Created = DateTime.UtcNow });
        }).ConfigureAwait(false);

        TypedDocumentSnapshot<User> snapshot = await userDoc.GetSnapshotAsync().ConfigureAwait(false);
        return snapshot.RequiredObject;
    }

    public Task Update(string userId, UpdateUser user)
    {
        var updateDefinition = new UpdateDefinition<User>();
        SetProfilePictureUri(user, updateDefinition);
        SetEmailValidated(user, updateDefinition);
        SetDisplayName(user, updateDefinition);
        SetFacebookLink(user, updateDefinition);
        SetLinkedInLink(user, updateDefinition);
        SetTwitterLink(user, updateDefinition);
        SetStackOverflowLink(user, updateDefinition);
        SetGitHubLink(user, updateDefinition);
        SetDescription(user, updateDefinition);

        return ExecuteDbOperation(() => _collection.Document(userId).UpdateAsync(updateDefinition));
    }

    private async Task<T> ExecuteDbOperation<T>(Func<Task<T>> operation)
    {
        try
        {
            return await operation().ConfigureAwait(false);
        }
        catch (RpcException e)
        {
            _logger.LogError("Database operation failed {StatusCode}, {Message}", e.StatusCode, e.Status.Detail);
            throw new DcException(ErrorCodes.DatabaseError, "Failed to execute operation");
        }
    }


    public Task UpdatePasswordInfo(string userId, PasswordInfo passwordInfo)
    {
        return ExecuteDbOperation(
            () => _collection
                .Document(userId)
                .UpdateAsync(user => user.PasswordInfo, passwordInfo));
    }

    public Task AddRoles(string userId, params string[] roles)
    {
        return ExecuteDbOperation(
            () => _collection
                .Document(userId)
                .UpdateAsync(user => user.Roles, FieldValue.ArrayUnion(roles)));
    }

    public Task RemoveRoles(string userId, params string[] roles)
    {
        return ExecuteDbOperation(
            () => _collection
                .Document(userId)
                .UpdateAsync(user => user.Roles, FieldValue.ArrayRemove(roles)));
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

    private static void SetEmailValidated(UpdateUser user, UpdateDefinition<User> updateDefinition)
    {
        if (user.EmailValidated is not null)
        {
            updateDefinition.Set(u => u.EmailValidated, user.EmailValidated.Value);
        }
    }

    private static void SetDescription(UpdateUser user, UpdateDefinition<User> updateDefinition)
    {
        if (user.Description is not null)
        {
            updateDefinition.Set(u => u.Description, user.Description);
        }
    }
}