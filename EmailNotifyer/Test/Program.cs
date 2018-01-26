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
                Message = "Test Message"
            };
            Console.WriteLine(EmailHandler.SendEmail(email).GetAwaiter().GetResult());
            Console.ReadKey();
        }
    }
}
