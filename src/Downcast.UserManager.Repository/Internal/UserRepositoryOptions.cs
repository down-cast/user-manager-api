using System.ComponentModel.DataAnnotations;

namespace Downcast.UserManager.Repository.Internal;

public class UserRepositoryOptions
{
    public const string SectionName = "RepositoryOptions";

    [Required(AllowEmptyStrings = false, ErrorMessage = "Collection name cannot be null or empty")]
    public required string Collection { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "ProjectId is required")]
    public required string ProjectId { get; set; }
}