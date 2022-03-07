using FileManager.IRepository;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;

namespace FileManager.Services
{
    public class SendGridService : ISendGridService
    {
        public async Task<bool> SendEmail(string token, string email)
        {
            var apiKey = Environment.GetEnvironmentVariable("SGKEY");
            var senderEmail = Environment.GetEnvironmentVariable("SGEMAIL");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(senderEmail, "FILE MANAGER API");
            var subject = "Password Recovery";
            var to = new EmailAddress(email, "User of FILE MANAGER");
            var plainTextContent = "";
            var htmlContent = $"<strong>Click <a href=\"https://localhost:44369/api/Auth/changepassword/{token}\">Here</a> </strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);


            return true;

        }
    }
}
