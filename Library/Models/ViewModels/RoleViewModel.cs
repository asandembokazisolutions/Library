using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Library.Models.ViewModels
{
    public class RoleEditViewModel
    {
        public IdentityRole Role { get; set; } = new();
        public IEnumerable<AppUser> Members { get; set; } = new List<AppUser>();
        public IEnumerable<AppUser> NonMembers { get; set; } = new List<AppUser>();
    }

    public class RoleModificationViewModel
    {
        [Required]
        public string RoleName { get; set; } = string.Empty;
        public string RoleId { get; set; } = string.Empty;
        public string[]? IdsToAdd { get; set; }
        public string[]? IdsToDelete { get; set; }
    }

}
