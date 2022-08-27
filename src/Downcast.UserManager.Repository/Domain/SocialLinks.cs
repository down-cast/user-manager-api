using Google.Cloud.Firestore;

namespace Downcast.UserManager.Repository.Domain;

[FirestoreData]
internal class SocialLinks
{
    [FirestoreProperty]
    internal string? LinkedIn { get; set; }

    [FirestoreProperty]
    internal string? Facebook { get; set; }

    [FirestoreProperty]
    internal string? GitHub { get; set; }

    [FirestoreProperty]
    internal string? Twitter { get; set; }

    [FirestoreProperty]
    internal string? StackOverflow { get; set; }
}