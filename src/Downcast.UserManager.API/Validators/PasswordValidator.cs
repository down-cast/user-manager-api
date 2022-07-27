using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

using Microsoft.Extensions.Options;

namespace Downcast.UserManager.API.Validators
{
    public class PasswordValidator : RequiredAttribute
    {
        private readonly PasswordRequirementOptions _passwordRequirementOptions;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            validationContext.GetRequiredService<IOptions<PasswordRequirementOptions>>();
            if (value is null)
            {
                return new ValidationResult("Password field must not be empty");
            }

            else if (value is string)
            {
                string password = value as string;

                foreach (KeyValuePair<string, string> requirement in _passwordRequirementOptions.Requirements)
                {
                    Regex regex = new Regex(requirement.Value);
                    if (!regex.IsMatch(password))
                    {
                        return new ValidationResult(requirement.Key);
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}
