using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModel
{
    public class SignInViewModel
    {
        [EmailAddress(ErrorMessage = "Mail formatını düzgün daxil edin")]
        [Required(ErrorMessage = "Mail adresi daxil etmək vacibdir")]
        public string? Mail { get; set; }
        [Required(ErrorMessage = "Şifrəni daxil edin")]
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
