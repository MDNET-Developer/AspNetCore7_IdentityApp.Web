using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModel
{
    public class PasswordChangeViewModel
    {
        [Required(ErrorMessage = "Köhnə şifrəni daxil edin")]
        public string? OldPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifrəni daxil edin")]
        public string? NewPassword { get; set; }
        [Required(ErrorMessage = "Yeni təkrar şifrəni daxil edin")]
        [Compare("NewPassword", ErrorMessage = "Daxil etdiyiniz şifrə, təkrar şifrə ilə eyni deyil")]
        public string? NewConfirmPassword { get; set; }

        [Required(ErrorMessage = "Doğrulama kodunu daxil edin!!!")]
        public string? Captcha { get; set; }
    }
}
