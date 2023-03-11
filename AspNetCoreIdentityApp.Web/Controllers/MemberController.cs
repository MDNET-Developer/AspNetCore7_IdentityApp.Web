using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = User.Identity.Name;
            var hasUser = await _userManager.FindByNameAsync(user);
            TempData["Mail"] = hasUser.Email.ToString();
            TempData["Phonenumber"] = hasUser.PhoneNumber.ToString();
            return View();
        }

        public async Task/*<IActionResult>*/ SignOut()
        {
           await _signInManager.SignOutAsync();
            //return RedirectToAction("Index","Home");
        }

        public async Task<IActionResult> PasswordChange()
        {
            return View();
        }
    }
}
