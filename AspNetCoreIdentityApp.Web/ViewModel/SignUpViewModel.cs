using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModel
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "İstifadəçi adını daxil edin")]
        public string? UserName { get; set; }


        [EmailAddress(ErrorMessage = "Mail formatını düzgün daxil edin")]
        [Required(ErrorMessage = "Mail adresi daxil etmək vacibdir")]
        public string? Mail { get; set; }


        [Phone(ErrorMessage ="Telefon nömrəsini düzgün daxil edin")]
        [Required(ErrorMessage = "Telefon nömrəsini daxil edin")]
        public string? PhoneNumber { get; set; }



        [Required(ErrorMessage = "Şifrəni daxil edin")]
        public string? Password { get; set; }


        [Required(ErrorMessage = "Təkrar şifrəni daxil edin")]
        [Compare("Password", ErrorMessage = "Daxil etdiyiniz şifrə, təkrar şifrə ilə eyni deyil")]
        public string? ComfirmPassword { get; set; }
    }
}
