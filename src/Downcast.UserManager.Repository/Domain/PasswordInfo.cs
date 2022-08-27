using Google.Cloud.Firestore;

namespace Downcast.UserManager.Repository.Domain;

[FirestoreData]
internal class PasswordInfo
{
    [FirestoreProperty]
    internal int Iterations { get; set; }

    [FirestoreProperty]
    internal string Salt { get; set; } = null!;

    [FirestoreProperty]
    internal string Hash { get; set; } = null!;
}