using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModel
{
    public class NewPasswordViewModel
    {

        [Required(ErrorMessage = "Şifrəni daxil edin")]
        public string? Password { get; set; }


        [Required(ErrorMessage = "Təkrar şifrəni daxil edin")]
        [Compare("Password", ErrorMessage = "Daxil etdiyiniz şifrə, təkrar şifrə ilə eyni deyil")]
        public string? ComfirmPassword { get; set; }
    }
}
