using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace CinemaSystem.Utilies
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
             var Client = new SmtpClient("smtp.gmail.com", 587)
              {
                  EnableSsl = true,
                  UseDefaultCredentials = false,
                  Credentials = new NetworkCredential("lolo1622005@gmail.com", "tgux yzzi vdjw dvbh")

              };

           /*  return Client.SendMailAsync(new MailMessage (from: "nour.mohamed.91735@gmail.com",to:email, subject,
                              htmlMessage)
             {
                 IsBodyHtml = true
             });*/
            var message = new MailMessage(
                "nour.mohamed.91735@gmail.com",
                email,
                subject,
                htmlMessage
            )
            {
                IsBodyHtml = true
            };

            return Client.SendMailAsync(message);

        }
    }
}
