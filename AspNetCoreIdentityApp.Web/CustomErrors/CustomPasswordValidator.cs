using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace AspNetCoreIdentityApp.Web.CustomErrors
{
    public class CustomPasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string? password)
        {

            var errors = new List<IdentityError>();
            var userName = new List<IdentityUser>();
            if (password!.ToUpper().Contains(user.UserName!.ToUpper()))
            {
                errors.Add(new IdentityError(){
                    Code="NotEqeualUserNameAndPass",
                    Description="İstifadəçi adı ilə şifrəniz eyni ola bilməz"
                });
            }
            if (errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
            else
            {
                return Task.FromResult(IdentityResult.Success);
            }
        }
    }
}
