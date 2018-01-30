using System;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using FlourishAPI.Models.Classes;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FlourishAPI.Models
{
    public class EmailHandler
    {
        private const string _base64ApiKey =
            "U0cuMHJoWk1xMVpSU0tHcXp1VWg5VnRfQS5TUFVOOTVsOEM5S1NUT0pYVHNqa3hEaFhYRkNrZF9QZ2prb3k4ckJ3MHIw";
        private static MailMessage _email;

        public static void SendMail(MailMessage emailMessage)
        {
            _email = emailMessage;

            //Execute().Wait();
        }

        static async Task Execute()
        {
            var sendGridMessage = new SendGridMessage();
            byte[] data = Convert.FromBase64String(_base64ApiKey);
            string decodedString = Encoding.UTF8.GetString(data);
            var client = new SendGridClient(decodedString);

            sendGridMessage.From = new EmailAddress("kieren.gerling@sybrin.co.za", "HRPayroll");
            foreach (MailAddress mailAddress in _email.To)
            {
                sendGridMessage.AddTo(mailAddress.Address, mailAddress.DisplayName);
                sendGridMessage.AddSubstitution("#Name#", mailAddress.DisplayName);
            }

            sendGridMessage.Subject = _email.Subject;
            sendGridMessage.HtmlContent = _email.Body;
            sendGridMessage.TemplateId = "661fbfd5-e6ed-42c7-b436-adc9651c0db8";

            var response = await client.SendEmailAsync(sendGridMessage);
            
            foreach (MailAddress emailAddress in _email.To)
            {
                new EventLogger(
                    string.Format("Email logged to \"{0}\" with subject \"{1}\" with status code \"{2}\"",
                        emailAddress.DisplayName, _email.Subject, response.StatusCode.ToString()), Severity.Event);
            }
        }
    }
}
