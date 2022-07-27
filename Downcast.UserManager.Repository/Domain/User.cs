
using Google.Cloud.Firestore;

namespace Downcast.UserManager.Repository.Domain
{
    [FirestoreData]
    internal class User
    {
        [FirestoreDocumentId]
        public string UserId { get; set; } = null!;
        [FirestoreDocumentCreateTimestamp]
        public DateTime Created { get; set; }
        [FirestoreDocumentUpdateTimestamp]
        public DateTime Updated { get; set; }
        [FirestoreProperty]
        public string Name { get; set; } = null!;
        [FirestoreProperty]
        public string Email { get; set; } = null!;
        [FirestoreProperty]
        public string? GithubLink { get; set; }
        [FirestoreProperty]
        public string? LinkedInLink { get; set; }
        [FirestoreProperty]
        public string? FacebookLink { get; set; }
        [FirestoreProperty]
        public string? TwitterLink { get; set; }
        [FirestoreProperty]
        public string PasswordHash { get; set; } = null!;
        [FirestoreProperty]
        public string PasswordSalt { get; set; } = null!;
        [FirestoreProperty]
        public int PasswordHashingIterations { get; set; }
        //public Social Links { get; set; } = new();
        //public PasswordInfo PasswordInfo { get; set; } = new();
    }
}
