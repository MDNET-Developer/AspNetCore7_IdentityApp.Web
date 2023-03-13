using AspNetCoreIdentityApp.Web.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModel
{
    public class UserEditViewModel
    {
        [Required(ErrorMessage = "Şifrəni daxil edin")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "İstifadəçi adını daxil edin")]
        public string? UserName { get; set; } = null!;


        [EmailAddress(ErrorMessage = "Mail formatını düzgün daxil edin")]
        [Required(ErrorMessage = "Mail adresi daxil etmək vacibdir")]
        public string? Mail { get; set; } = null!;


        [Phone(ErrorMessage = "Telefon nömrəsini düzgün daxil edin")]
        [Required(ErrorMessage = "Telefon nömrəsini daxil edin")]
        public string PhoneNumber { get; set; } = null!; //Null ola bilmez
        public DateTime? BirthDay { get; set; }
        public string? City { get; set; }
        public IFormFile? PictureUrl { get; set; }
        public Gender? Gender { get; set; }
    }
}
