using Library.Models;
using Library.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserValidator<AppUser> _userValidator;
        private readonly IPasswordValidator<AppUser> _passwordValidator;

        public AdminController(UserManager<AppUser> userManager,
            IUserValidator<AppUser> userValidator,
            IPasswordValidator<AppUser> passwordValidator)
        {
            _userManager = userManager;
            _userValidator = userValidator;
            _passwordValidator = passwordValidator;
        }

        public IActionResult Index() => View(_userManager.Users.ToList());

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                    return RedirectToAction("Index");

                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            AppUser? user = await _userManager.FindByIdAsync(id);
            if (user == null) return RedirectToAction("Index");
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string email, string? password)
        {
            AppUser? user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return View();
            }

            user.Email = email;
            IdentityResult validEmail = await _userValidator.ValidateAsync(_userManager, user);
            if (!validEmail.Succeeded) AddErrors(validEmail);

            IdentityResult? validPass = null;
            if (!string.IsNullOrEmpty(password))
            {
                if (await _userManager.HasPasswordAsync(user))
                    await _userManager.RemovePasswordAsync(user);

                validPass = await _userManager.AddPasswordAsync(user, password);
                if (!validPass.Succeeded) AddErrors(validPass);
            }

            bool emailOk = validEmail.Succeeded;
            bool passOk = validPass == null || validPass.Succeeded;

            if (emailOk && passOk)
            {
                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded) return RedirectToAction("Index");
                AddErrors(result);
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            AppUser? user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded) return RedirectToAction("Index");
                AddErrors(result);
            }
            else ModelState.AddModelError("", "User not found.");

            return View("Index", _userManager.Users.ToList());
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (IdentityError e in result.Errors)
                ModelState.AddModelError("", e.Description);
        }
    }
}
