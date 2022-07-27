using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Downcast.UserManager.Model.Validators
{
    public class PasswordValidator : RequiredAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Dictionary<string, string>? _passwordRequirementOptions = validationContext
                .GetRequiredService<IOptions<PasswordRequirementOptions>>()
                .Value
                .Requirements;
            if (value is null)
            {
                return new ValidationResult("Password field must not be empty");
            }

            else if (value is string)
            {
                string password = value as string;

                foreach (KeyValuePair<string, string> requirement in _passwordRequirementOptions)
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
