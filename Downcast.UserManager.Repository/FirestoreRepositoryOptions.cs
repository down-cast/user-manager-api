using System.ComponentModel.DataAnnotations;

namespace Downcast.UserManager.Repository
{
    public class FirestoreRepositoryOptions
    {
        public const string SectionName = "RepositoryOptions";

        [Required(AllowEmptyStrings = false, ErrorMessage = "Collection name must not be empty")]
        public string Collection { get; set; } = null!;
        [Required(AllowEmptyStrings = false, ErrorMessage = "ProjectID name must not be empty")]
        public string ProjectID { get; set; } = null!;
    }
}
