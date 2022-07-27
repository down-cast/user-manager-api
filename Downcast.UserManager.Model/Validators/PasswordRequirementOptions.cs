using System.ComponentModel.DataAnnotations;

namespace Downcast.UserManager.Model.Validators
{
    public class PasswordRequirementOptions : IValidatableObject
    {
        public const string SectionName = "PasswordRequirements";
        public Dictionary<string, string> Requirements { get; set; } = new();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Requirements.Any())
            {
                yield return new ValidationResult("Password requirements dictionary cannot be empty", new[] { nameof(Requirements) });
            }
        }
    }
}
