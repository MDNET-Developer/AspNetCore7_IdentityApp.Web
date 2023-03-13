using AspNetCoreIdentityApp.Web.Enums;
using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.Services;
using AspNetCoreIdentityApp.Web.ViewModel;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFileProvider _fileProvider;
        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IFileProvider fileProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _fileProvider = fileProvider;
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
        [HttpGet]
        public async Task<IActionResult> UserEdit()
        {
            TempData["Bootstrap"] = "";
            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));
            var user = User.Identity!.Name;
            var currentUser = await _userManager.FindByNameAsync(user!);
            UserEditViewModel viewModel = new()
            {
                UserName = currentUser!.UserName,
                PhoneNumber = currentUser.PhoneNumber!,
                Mail = currentUser.Email!,
                City = currentUser.City,
                Gender = currentUser.Gender,
                BirthDay = currentUser.BirthDay
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var user = User.Identity!.Name;
            var currentUser = await _userManager.FindByNameAsync(user);
            var PasswordCheck = await _userManager.CheckPasswordAsync(currentUser, viewModel.Password);
            if (!PasswordCheck)
            {
                TempData["Bootstrap"] = "alert alert-danger";
                ModelState.AddModelError("", $"Məlumatlar üzərində dəyişiklik etmək üçün düzgün şifrəni  daxil etmək lazımdır");
                ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));
            }
            else
            {
                currentUser.UserName = viewModel.UserName;
                currentUser.Email = viewModel.Mail;
                currentUser.PhoneNumber = viewModel.PhoneNumber;
                currentUser.BirthDay = viewModel.BirthDay;
                currentUser.City = viewModel.City;
                currentUser.Gender = viewModel.Gender;

                if (viewModel.PictureUrl != null && viewModel.PictureUrl.Length > 0)
                {
                    var wwwrootFolder = _fileProvider.GetDirectoryContents("wwnroot");

                    var randoomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension
                    (viewModel.PictureUrl.FileName)}";

                    var newPicturePath = Path.Combine(wwwrootFolder!.First(x => x.Name == "userPictures").PhysicalPath!, randoomFileName);

                    using var stream = new FileStream(newPicturePath, FileMode.Create);
                    await viewModel.PictureUrl.CopyToAsync(stream);

                    currentUser.Picture = randoomFileName;
                }
                var result = await _userManager.UpdateAsync(currentUser);
                if (result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(currentUser);
                    await _signInManager.SignOutAsync();
                    await _signInManager.PasswordSignInAsync(currentUser, viewModel.Password, true, false);
                    TempData["Message"] = $"{currentUser!.UserName} sizin məlumatlarınız uğurla yeniləndi.";
                    TempData["Bootstrap"] = "alert alert-success";
                    UserEditViewModel viewModelUser = new()
                    {
                        UserName = currentUser!.UserName,
                        PhoneNumber = currentUser.PhoneNumber!,
                        Mail = currentUser.Email!,
                        City = currentUser.City,
                        Gender = currentUser.Gender,
                        BirthDay = currentUser.BirthDay
                    };
                    return View(viewModelUser);
                }
                else
                {
                    TempData["Bootstrap"] = "alert alert-danger";
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View();
                }
            }
            return View();
        }



        [HttpGet]
        public IActionResult PasswordChange()
        {
            TempData["Bootstrap"] = "";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var user = User.Identity.Name;
            var hasUser = await _userManager.FindByNameAsync(user);
            var oldPasswordCheck = await _userManager.CheckPasswordAsync(hasUser, viewModel.OldPassword);
            if (!oldPasswordCheck)
            {
                ModelState.AddModelError(viewModel.OldPassword, "Köhnə şifrəniz düzgün deyil");
            }

            var result = await _userManager.ChangePasswordAsync(hasUser, viewModel.OldPassword, viewModel.NewPassword);
            if (result.Succeeded)
            {
                await _userManager.UpdateSecurityStampAsync(hasUser);
                await _signInManager.SignOutAsync();
                await _signInManager.PasswordSignInAsync(hasUser, viewModel.NewPassword, true, false);
                TempData["Message"] = $"{hasUser!.UserName} sizin şifrəniz uğurla yeniləndi.";
                TempData["Bootstrap"] = "alert alert-success";
                return View();
            }
            else
            {
                TempData["Bootstrap"] = "alert alert-danger";
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }


        }

    }
}
