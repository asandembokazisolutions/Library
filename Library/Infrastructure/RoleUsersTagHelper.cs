using Library.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Library.Infrastructure
{
    // Targets: <td identity-role-id="@role.Id"></td>
    // Renders comma-separated list of users in that role
    [HtmlTargetElement("td", Attributes = "identity-role-id")]
    public class RoleUsersTagHelper : TagHelper
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleUsersTagHelper(UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HtmlAttributeName("identity-role-id")]
        public string RoleId { get; set; } = string.Empty;

        public override async Task ProcessAsync(TagHelperContext context,
            TagHelperOutput output)
        {
            List<string> names = new();
            IdentityRole? role = await _roleManager.FindByIdAsync(RoleId);

            if (role != null)
            {
                foreach (AppUser user in _userManager.Users)
                {
                    if (await _userManager.IsInRoleAsync(user, role.Name!))
                        names.Add(user.UserName!);
                }
            }

            output.Content.SetContent(
                names.Count == 0 ? "No Users" : string.Join(", ", names));
        }
    }
}
