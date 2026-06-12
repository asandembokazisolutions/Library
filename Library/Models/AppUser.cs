using Microsoft.AspNetCore.Identity;

namespace Library.Models
{
    public class AppUser : IdentityUser
    {
        public byte[]? AvatarImage { get; set; }
    }

}
