using Microsoft.AspNetCore.Mvc;
using Support.Models;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Support.Controllers
{
    public class EmailController : Controller
    {
        private IConfigurationRoot _configurationRoot;

        public EmailController(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Send(string name, string email, string subject, string message)
        {
            try
            {
                //var configurationRoot = new ConfigurationBuilder()
                //    .SetBasePath(Directory.GetCurrentDirectory())
                //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                //    .Build();

                var emailSettings = _configurationRoot.GetSection("EmailSettings").Get<EmailSettings>();

                if (emailSettings != null)
                {
                    using var mail = new MailMessage();
                    mail.From = new MailAddress(emailSettings.SmtpUsername, emailSettings.SenderDisplayName);
                    mail.To.Add(emailSettings.RecipientEmail);
                    mail.Subject = subject;
                    var body = new StringBuilder();
                    body.AppendLine($"{emailSettings.CaptionName} : {name}");
                    body.AppendLine($"{emailSettings.CaptionEmail} : {email}");
                    body.AppendLine("----------");
                    body.AppendLine($"{emailSettings.CaptionSubject} :");
                    body.AppendLine(subject);
                    body.AppendLine("----------");
                    body.AppendLine($"{emailSettings.CaptionMessage} :");
                    body.AppendLine(message);
                    mail.Body = body.ToString();
                    mail.IsBodyHtml = false;

                    using var smtp = new SmtpClient(emailSettings.SmtpServer, emailSettings.SmtpPort);
                    smtp.EnableSsl = emailSettings.EnableSsl;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(
                        emailSettings.SmtpUsername,
                        emailSettings.SmtpPassword
                    );
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Timeout = 30000; // 30 ثانیه

                    smtp.Send(mail);

                    //return "پیام ارسال شد";
                    return View("../Home/Thanks");
                }
                else
                {
                    //return "تنظیمات ایمیل خوانده نشد";
                    return null;
                }


            }
            catch (Exception ex)
            {
                var error = new StringBuilder();
                error.AppendLine("خطایی رخ داد");
                error.AppendLine(ex.Message);
                //return error.ToString();
                return null;
            }
        }
    }
}
