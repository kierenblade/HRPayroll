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
