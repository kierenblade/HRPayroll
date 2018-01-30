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
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress("test@example.com", "Example User");
            var to = new EmailAddress(_email.To.First().Address, _email.To.First().DisplayName);
            var subject = _email.Subject;
            var message = _email.Body;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
