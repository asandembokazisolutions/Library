using Library.Models;
using Microsoft.AspNetCore.Identity;

namespace Library.Infrastructure
{
    // Custom rule: email must be from an allowed domain; username must be lowercase
    public class CustomUserValidator : IUserValidator<AppUser>
    {
        private static readonly string[] _allowedDomains = ["library.com", "ufs.ac.za", "gmail.com"];

        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            List<IdentityError> errors = new();

            if (user.Email != null &&
                !_allowedDomains.Any(d => user.Email.ToLower().EndsWith($"@{d}")))
            {
                errors.Add(new IdentityError
                {
                    Code = "InvalidEmailDomain",
                    Description = $"Email must end with one of: {string.Join(", ", _allowedDomains)}"
                });
            }

            if (user.UserName != null && user.UserName.Any(char.IsUpper))
            {
                errors.Add(new IdentityError
                {
                    Code = "UppercaseUsername",
                    Description = "Username must be all lowercase letters."
                });
            }

            return Task.FromResult(errors.Count == 0
                ? IdentityResult.Success
                : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
