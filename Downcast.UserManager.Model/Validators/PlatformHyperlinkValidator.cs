using System.ComponentModel.DataAnnotations;

namespace Downcast.UserManager.Model.Validators
{
    public class PlatformHyperlinkValidator : ValidationAttribute
    {
        private const int MaximumCharactersAllowed = 500;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {     
            if (value is Uri { OriginalString.Length: < MaximumCharactersAllowed } link && Uri.IsWellFormedUriString(link.OriginalString, UriKind.Absolute) || value is null)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult($"Invalid URI or exceeded {MaximumCharactersAllowed} character limit");
        }
    }
}