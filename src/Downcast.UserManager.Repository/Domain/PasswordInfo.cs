using Google.Cloud.Firestore;

namespace Downcast.UserManager.Repository.Domain;

[FirestoreData]
internal class PasswordInfo
{
    [FirestoreProperty]
    internal required int Iterations { get; set; }

    [FirestoreProperty]
    internal required string Salt { get; set; }

    [FirestoreProperty]
    internal required string Hash { get; set; }
}