using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace HRPayroll.EmailService
{
    public class EmailHandler
    {
        private const string _apiKey = "";
        private static MailMessage _email;

        public static void SendMail(MailMessage emailMessage)
        {
            _email = emailMessage;

            Execute().Wait();
        }

        static async Task Execute()
        {
            var sendGridMessage = new SendGridMessage();
            var client = new SendGridClient(_apiKey);

            sendGridMessage.From = new EmailAddress("test@example.com", "Example User");
            foreach (MailAddress mailAddress in _email.To)
            {
                sendGridMessage.AddTo(mailAddress.Address, mailAddress.DisplayName);
            }

            sendGridMessage.Subject = _email.Subject;
            sendGridMessage.HtmlContent = _email.Body;
            sendGridMessage.TemplateId = "3a7d307b-598d-4eb0-90c2-c16b3b82c806";

            var response = await client.SendEmailAsync(sendGridMessage);
        }
    }
}
