using HRPayroll.Classes.Models;
using System;
using System.Collections.Generic;
using System.Text;
using EmailNotifyer;

namespace EmailNotifyer
{
    class Test
    {
        static void Main(string[] args)
        {
            EmailNotification email = new EmailNotification()
            {
                ContactDetails = new ContactDetails()
                {
                    Email = "jason.vanheerden@sybrin.com",
                    ContactName = "Jason"
                },
                Message = "This is an attempt at a test message"
            };
            List<EmailNotification> output = new List<EmailNotification>();
            output.Add(email);
            Console.WriteLine(EmailHandler.SendEmail(output).GetAwaiter().GetResult());
            Console.ReadKey();
        }
    }
}
