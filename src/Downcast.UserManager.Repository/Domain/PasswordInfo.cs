using Google.Cloud.Firestore;

namespace Downcast.UserManager.Repository.Domain;

[FirestoreData]
internal class PasswordInfo
{
    [FirestoreProperty]
    internal int Iterations { get; init; }

    [FirestoreProperty]
    internal string Salt { get; init; } = null!;

    [FirestoreProperty]
    internal string Hash { get; init; } = null!;
}