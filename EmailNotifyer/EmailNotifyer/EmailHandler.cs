using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using HRPayroll.Classes.Models;
using Newtonsoft.Json;

namespace EmailNotifyer
{
    public class EmailHandler
    {
        public enum SuccessStatus
        {
            Sent = 0,
            ConnectionError,
            ServerError
        }
        public static async Task<SuccessStatus> SendEmail(EmailNotification email)
        {
            string url = "http://gateway5.sybrin.systems/Services/api/session";
            string sessionID;
            using (var client = new HttpClient())
            {
                var mycontent = JsonConvert.SerializeObject(new SessionRequest() { Password = "Sybr!n123", SecurityModel = "SYBRIN", UserID = "Admin"});
                var buffer = System.Text.Encoding.UTF8.GetBytes(mycontent);
                var bytecontent = new ByteArrayContent(buffer);
                bytecontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.PostAsync(url, bytecontent);

                if (response.IsSuccessStatusCode)
                {
                    SessionResponse sesh = JsonConvert.DeserializeObject<SessionResponse>(await response.Content.ReadAsStringAsync());
                    sessionID = sesh.SessionId;
                }
                else
                {
                    return SuccessStatus.ConnectionError;
                }
            }

            url = "http://gateway5.sybrin.systems/Services/api/addemailmessage";
            using (var client = new HttpClient())
            {
                EmailRequest emailRequest = new EmailRequest()
                {
                    SessionId = sessionID,
                    Messages = new EmailRequest.MessageContent()
                    {
                        To = email.ContactDetails.Email,
                        From = "test.mail@sybrin.com",
                        Subject = "Test Message",
                        Message64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(email.Message))
                        }
                };
                var mycontent = JsonConvert.SerializeObject(emailRequest);
                var buffer = System.Text.Encoding.UTF8.GetBytes(mycontent);
                var bytecontent = new ByteArrayContent(buffer);
                bytecontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.PostAsync(url, bytecontent);

                if (response.IsSuccessStatusCode)
                {
                    EmailResponse sesh = JsonConvert.DeserializeObject<EmailResponse>(await response.Content.ReadAsStringAsync());
                }
                else
                {
                    return SuccessStatus.ConnectionError;
                }
            }
            return SuccessStatus.Sent;
        }

        public class SessionRequest
        {
            public string SecurityModel { get; set; }
            public string UserID { get; set; }
            public string Password { get; set; }
        }
        public class SessionResponse
        {
            public string SessionId { get; set; }
            public SessionUser User { get; set; }
            public int SessionTimeout { get; set; }
            public string WebAssist { get; set; }
            public class SessionUser
            {
                public bool CanChangePassword { get; set; }
                public string[] Groups { get; set; }
                public bool IsAdministrator { get; set; }
                public bool IsAuthenticated { get; set; }
                public DateTime LastLogonDate { get; set; }
                public bool MustChangePassword { get; set; }
                public string Description { get; set; }
                public string Name { get; set; }
                public string UserId { get; set; }
                public string UserType { get; set; }

            }
        }
        public class EmailRequest
        {
            public string SessionId { get; set; }
            public MessageContent Messages{ get; set; }

            public class MessageContent
            {
                public string To { get; set; }
                public string CC { get; set; }
                public string BCC { get; set; }
                public string From { get; set; }
                public string Referenceid { get; set; }
                public string Subject { get; set; }
                public string Message64 { get; set; }
                public string MessageHtml64 { get; set; }
                public int Importance { get; set; }
                public int Priority { get; set; }
                public string Source { get; set; }
            }
        }
        public class EmailResponse
        {
            public string MessageID { get; set; }
            public string ReferenceID { get; set; }
        }

    }
}
