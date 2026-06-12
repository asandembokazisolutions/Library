using Library.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Library.Infrastructure
{
    // Targets: <img profile-user="@User.Identity.Name" />
    [HtmlTargetElement("img", Attributes = "profile-user")]
    public class ProfileImageTagHelper : TagHelper
    {
        private readonly UserManager<AppUser> _userManager;

        public ProfileImageTagHelper(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HtmlAttributeName("profile-user")]
        public string UserName { get; set; } = string.Empty;

        public override async Task ProcessAsync(TagHelperContext context,
            TagHelperOutput output)
        {
            AppUser? user = await _userManager.FindByNameAsync(UserName);

            if (user != null && user.AvatarImage != null && user.AvatarImage.Length > 0)
            {
                string base64 = Convert.ToBase64String(user.AvatarImage);
                string src = $"data:image/jpeg;base64,{base64}";
                output.Attributes.SetAttribute("src", src);
            }
            else
            {
                // Fallback to default avatar if no image uploaded
                output.Attributes.SetAttribute("src", "/images/default-avatar.png");
            }

            output.Attributes.SetAttribute("class", "img-thumbnail rounded-circle");
            output.Attributes.SetAttribute("style", "height:32px; width:32px; object-fit:cover;");
            output.Attributes.SetAttribute("alt", UserName);
        }
    }
}
