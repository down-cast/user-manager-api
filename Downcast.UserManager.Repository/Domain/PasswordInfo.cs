
using Google.Cloud.Firestore;

namespace Downcast.UserManager.Repository.Domain
{
    [FirestoreData]
    internal class PasswordInfo
    {
        [FirestoreProperty]
        public string PasswordHash { get; set; } = null!;
        [FirestoreProperty]
        public string PasswordSalt { get; set; } = null!;
        [FirestoreProperty]
        public int Iterations { get; set; }
    }
}
