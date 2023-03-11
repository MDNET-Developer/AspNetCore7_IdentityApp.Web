using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.Services;
using AspNetCoreIdentityApp.Web.ViewModel;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult NewPassword(string userId,string token)
        {
            TempData["Bootstrap"] = "";
            TempData["Message"] = "";
            TempData["userId"] = userId;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NewPassword(NewPasswordViewModel viewModel)
        {
            var userId = TempData["userId"]!.ToString();
            var token = TempData["token"]!.ToString();
            if(userId==null && token == null)
            {
                throw new Exception("Xəta!!");
            }

            var hasUser = await _userManager.FindByIdAsync(userId!);

            var result = await _userManager.ResetPasswordAsync(hasUser!, token!,viewModel!.Password);

            if (result.Succeeded)
            {
                TempData["Message"] = $"{hasUser!.UserName} sizin şifrəniz uğurla yeniləndi.";
                TempData["Bootstrap"] = "alert alert-success";
            }
            else
            {
                TempData["Bootstrap"] = "alert alert-danger";
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

               
            }
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            TempData["Bootstrap"] = "";
            TempData["Message"] = "";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
        {
            var hasUser = await _userManager.FindByEmailAsync(request.Mail);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, $" '{request.Mail}'  e-poçt ünvanına sahib istifadəçi tapılmadı");
                TempData["Bootstrap"] = "alert alert-danger";
                return View();
            }
            else
            {
                //Burada olan new{} anonymus sinifdir. Bele sinif eslinde yoxdur yalandan isimizi gorsun deyene yaradilib
                var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);
                var resetPasswordLink = Url.Action("NewPassword", "Home", new { userId = hasUser.Id, Token = passwordResetToken },HttpContext.Request.Scheme);
                /*Daha sonra program.cs de Token-nin omrunu teyin edirik*/

                await _emailService.SendResetPasswordMail(resetPasswordLink, hasUser.Email);

                TempData["Message"] = $"{request.Mail} e-poçt ünvanına yeniləmə linki göndərildi.";
                TempData["Bootstrap"] = "alert alert-success";
                return View();
            }

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            TempData["Message"] = "";
            TempData["Bootstrap"] = "";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            AppUser appUser = new AppUser()
            {
                UserName = viewModel.UserName,
                PhoneNumber = viewModel.PhoneNumber,
                Email = viewModel.Mail,

            };
            var createUserResault = await _userManager.CreateAsync(appUser, viewModel.Password);
            if (createUserResault.Succeeded)
            {
                TempData["Message"] = "Qeydiyyat uğurla tamamlandı";
                TempData["Bootstrap"] = "alert alert-success";
                SignUpViewModel returnModel = new()
                {
                    UserName = null,
                    PhoneNumber = null,
                    Mail = null,
                };
                return View(returnModel);
            }
            else
            {
                foreach (var item in createUserResault.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

                return View();
            }
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel signInViewModel, string? returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Action("Index", "Home");
            var hasUser = await _userManager.FindByEmailAsync(signInViewModel.Mail);
            if (hasUser == null)
            {
                ModelState.AddModelError("", "E-poçt və ya şifrə doğru deyil");
                return View(signInViewModel);
            }
            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, signInViewModel.Password, signInViewModel.RememberMe, true);
            if (signInResult.Succeeded)
            {
                return Redirect(returnUrl);
            }
            else if (signInResult.IsLockedOut)
            {
                //Hesab kilidli
                var lockEndTime = await _userManager.GetLockoutEndDateAsync(hasUser);
                ModelState.AddModelError("", $"{hasUser} adlı hesabınız {(lockEndTime.Value.UtcDateTime - DateTime.UtcNow).Minutes} dəqiqə  {(lockEndTime.Value.UtcDateTime - DateTime.UtcNow).Seconds} saniyə  müddəti ərzində bloka düşdü");
            }

            else
            {

                if (hasUser != null)
                {
                    var failedCount = await _userManager.GetAccessFailedCountAsync(hasUser);
                    var maxFailedCount = _userManager.Options.Lockout.MaxFailedAccessAttempts;

                    ModelState.AddModelError("", $"{maxFailedCount - failedCount} dəfə  səhv etsəz hesabınız ban olacaq keçici olaraq");

                }
                else
                {
                    ModelState.AddModelError("", $"E-poçt və ya şifrə doğru deyil");
                }
            }
            ModelState.AddModelError("", "E-poçt və ya şifrə doğru deyil");
            return View(signInViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}