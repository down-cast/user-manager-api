using Google.Cloud.Firestore;

namespace Downcast.UserManager.Repository.Domain;

[FirestoreData]
internal class User
{
    [FirestoreDocumentId]
    public required string Id { get; set; }

    [FirestoreProperty]
    internal required string Email { get; set; }

    [FirestoreProperty]
    internal string? DisplayName { get; set; }

    [FirestoreProperty]
    internal string? ProfilePictureUri { get; set; }

    [FirestoreProperty]
    internal bool EmailValidated { get; set; }

    [FirestoreProperty]
    internal PasswordInfo? PasswordInfo { get; set; }

    [FirestoreProperty]
    internal SocialLinks? SocialLinks { get; set; }

    [FirestoreProperty]
    internal IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();

    [FirestoreDocumentCreateTimestamp]
    public DateTime Created { get; set; }

    [FirestoreDocumentUpdateTimestamp]
    public DateTime Updated { get; set; }
}