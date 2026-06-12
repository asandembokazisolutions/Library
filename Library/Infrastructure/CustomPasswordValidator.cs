using Library.Models;
using Microsoft.AspNetCore.Identity;

namespace Library.Infrastructure
{
    // Custom rule: password cannot contain username, cannot be purely numeric
    public class CustomPasswordValidator : IPasswordValidator<AppUser>
    {
        private static readonly string[] _bannedPasswords =
            ["password", "123456", "qwerty", "abc123", "letmein", "admin123"];

        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager,
            AppUser user, string? password)
        {
            List<IdentityError> errors = new();

            if (password != null && user.UserName != null &&
                password.ToLower().Contains(user.UserName.ToLower()))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordContainsUsername",
                    Description = "Password may not contain your username."
                });
            }

            if (password != null && _bannedPasswords.Contains(password.ToLower()))
            {
                errors.Add(new IdentityError
                {
                    Code = "CommonPassword",
                    Description = "This password is too common. Please choose a stronger password."
                });
            }

            if (password != null && password.All(char.IsDigit))
            {
                errors.Add(new IdentityError
                {
                    Code = "NumericOnly",
                    Description = "Password cannot consist entirely of numbers."
                });
            }

            return Task.FromResult(errors.Count == 0
                ? IdentityResult.Success
                : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
