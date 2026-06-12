using Library.Models;
using Library.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Library.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleAdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleAdminController(RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index() => View(_roleManager.Roles.ToList());

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded) return RedirectToAction("Index");
                foreach (IdentityError e in result.Errors)
                    ModelState.AddModelError("", e.Description);
            }
            return View(name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole? role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded) return RedirectToAction("Index");
                foreach (IdentityError e in result.Errors)
                    ModelState.AddModelError("", e.Description);
            }
            else ModelState.AddModelError("", "Role not found.");

            return View("Index", _roleManager.Roles.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole? role = await _roleManager.FindByIdAsync(id);
            if (role == null) return RedirectToAction("Index");

            List<AppUser> members = new();
            List<AppUser> nonMembers = new();

            foreach (AppUser user in _userManager.Users)
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name!)
                    ? members : nonMembers;
                list.Add(user);
            }

            return View(new RoleEditViewModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleModificationViewModel model)
        {
            if (!ModelState.IsValid)
                return await Edit(model.RoleId);

            foreach (string userId in model.IdsToAdd ?? Array.Empty<string>())
            {
                AppUser? user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                    await _userManager.AddToRoleAsync(user, model.RoleName);
            }

            foreach (string userId in model.IdsToDelete ?? Array.Empty<string>())
            {
                AppUser? user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                    await _userManager.RemoveFromRoleAsync(user, model.RoleName);
            }

            return RedirectToAction("Index");
        }
    }
}
