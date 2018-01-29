using HRPayroll.Classes.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace PayswitchInitializer
{
    class Program
    {
        static List<Employee> employees;
        static List<Company> companies;
        static List<Bank> banks;
        static void Main(string[] args)
        {
            using (StreamReader r = new StreamReader("empOut.json"))
            {
                string json = r.ReadToEnd();
                employees = JsonConvert.DeserializeObject<List<Employee>>(json);
            }

            using (StreamReader r = new StreamReader("compOut.json"))
            {
                string json = r.ReadToEnd();
                companies = JsonConvert.DeserializeObject<List<Company>>(json);
            }

            using (StreamReader r = new StreamReader("banksOut.json"))
            {
                string json = r.ReadToEnd();
                banks = JsonConvert.DeserializeObject<List<Bank>>(json);
            }

            List<BankAccDetails> bankAcc = new List<BankAccDetails>();
            List<CardDetail> cards = new List<CardDetail>();

            foreach (var item in employees)
            {
               BankAccDetails acc = new BankAccDetails() {
                    AccountBalance = new Random().Next(100000000, 999999999),
                    AccountNum = item.AccountNumber,
                    AccStatus = AccountStatus.Active
                };

                CardDetail c = new CardDetail()
                {
                    AccountBalance = new Random().Next(100000000, 999999999),
                    CardNumber = item.CardNumber,
                    CardStatus = AccountStatus.Active,
                    Currency = CurrencyEnum.ZAR
                };

                switch (item.Bank.Name)
                {
                    case "ABSA":
                        acc.BankCode = BankCode.ABSA;
                        c.BankCode = BankCode.ABSA;
                        break;
                    case "FNB":
                        acc.BankCode = BankCode.FNB;
                        c.BankCode = BankCode.FNB;
                        break;
                    default:
                        acc.BankCode = BankCode.None;
                        c.BankCode = BankCode.None;
                        break;
                }

                switch (item.PaymentType)
                {
                    case PaymentType.VISA:
                        c.CardClass = CardClassification.Visa;
                        break;
                    default:
                        c.CardClass = CardClassification.None;
                        break;
                }
                bankAcc.Add(acc);
                cards.Add(c);
            }

            foreach (var item in companies)
            {
                BankAccDetails acc = new BankAccDetails()
                {
                    AccountBalance = new Random().Next(100000000, 999999999),
                    AccountNum = item.AccountNumber,
                    AccStatus = AccountStatus.Active
                };

                CardDetail c = new CardDetail()
                {
                    AccountBalance = new Random().Next(100000000, 999999999),
                    CardNumber = item.CardNumber,
                    CardStatus = AccountStatus.Active,
                    Currency = CurrencyEnum.ZAR
                };

                switch (item.Bank.Name)
                {
                    case "ABSA":
                        acc.BankCode = BankCode.ABSA;
                        c.BankCode = BankCode.ABSA;
                        break;
                    case "FNB":
                        acc.BankCode = BankCode.FNB;
                        c.BankCode = BankCode.FNB;
                        break;
                    default:
                        acc.BankCode = BankCode.None;
                        c.BankCode = BankCode.None;
                        break;
                }

                c.CardClass = CardClassification.None;

                bankAcc.Add(acc);
                cards.Add(c);
            }
        }
    }

    class BankAccDetails
    {
        public string AccountNum { get; set; }
        public double AccountBalance { get; set; }
        public BankCode BankCode { get; set; }
        public AccountStatus AccStatus { get; set; }
    }

    class CardDetail
    {
        public string CardNumber { get; set; }
        public double AccountBalance { get; set; }
        public BankCode BankCode { get; set; }
        public AccountStatus CardStatus { get; set; }
        public CurrencyEnum Currency { get; set; }
        public CardClassification CardClass { get; set; }
    }

    enum BankCode
    {
        None = 0,
        ABSA,
        FNB,
        Nedbank,
        AfricaBank,
        Barclays,
        NBC
    }

    enum AccountStatus
    {
        None = 0,
        Active,
        Deactivated,
        Closed,
        Dormant
    }

    enum CurrencyEnum
    {
        None = 0,
        ZAR,
        USD,
        GBP,
        EUR
    }

    enum CardClassification
    {
        None = 0,
        Visa,
        Mastercard
    }
}
