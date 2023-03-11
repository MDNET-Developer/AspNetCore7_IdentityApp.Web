using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModel
{
    public class ResetPasswordViewModel
    {
        [EmailAddress(ErrorMessage = "Mail formatını düzgün daxil edin")]
        [Required(ErrorMessage = "Mail adresi daxil etmək vacibdir")]
        public string? Mail { get; set; }
    }
}
