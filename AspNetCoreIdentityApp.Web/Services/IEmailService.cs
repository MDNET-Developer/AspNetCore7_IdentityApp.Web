namespace AspNetCoreIdentityApp.Web.Services
{
    public interface IEmailService
    {
        Task SendResetPasswordMail(string resetEmailLink, string toEmail);
    }
}
