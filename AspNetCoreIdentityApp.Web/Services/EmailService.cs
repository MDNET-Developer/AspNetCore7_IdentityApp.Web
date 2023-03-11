using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.SettingsModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AspNetCoreIdentityApp.Web.Services
{
    public class EmailService : IEmailService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> options, UserManager<AppUser> userManager)
        {
            _emailSettings = options.Value;
            _userManager = userManager;
        }

        public async Task SendResetPasswordMail(string resetEmailLink, string toEmail)
        {

            var userName = await _userManager.FindByEmailAsync(toEmail);
            
            SmtpClient smtpClient= new SmtpClient();
            smtpClient.Host = _emailSettings.Host;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
            smtpClient.EnableSsl = true;

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_emailSettings.Email);
            mailMessage.To.Add(toEmail);
            mailMessage.Subject = "Şifrə yeniləmə linki";
            mailMessage.Body = @$"
<p style='text-align:center;' ><img height='100'width='auto' style='text-align:center'  src='https://i.hizliresim.com/14fnzmw.png' /></p>
<h3 style='text-align:center;'>Hörmətli  '{userName.UserName}'  şifrənizi yeniləmək üçün aşağıdakı linkə tıklayın</h3>
<p style='text-align:center;cursor:pointer' ><a type='button'  href='{resetEmailLink}'>Şifrə yeniləmə linki</a></p>
<hr/>
<p style='text-align:center;' >Murad Aliyev</p>";
            mailMessage.IsBodyHtml=true;
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
