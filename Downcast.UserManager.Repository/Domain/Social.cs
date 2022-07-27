using Google.Cloud.Firestore;

namespace Downcast.UserManager.Repository.Domain
{
    [FirestoreData]
    internal class Social
    {
        [FirestoreProperty]
        public string? GithubLink { get; set; }
        [FirestoreProperty]
        public string? LinkedInLink { get; set; }
        [FirestoreProperty]
        public string? FacebookLink { get; set; }
        [FirestoreProperty]
        public string? TwitterLink { get; set; }
    }
}
