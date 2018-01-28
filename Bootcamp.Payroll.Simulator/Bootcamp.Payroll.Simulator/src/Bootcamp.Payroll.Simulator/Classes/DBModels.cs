using Bootcamp.Payroll.Simulator.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bootcamp.Payroll.Simulator.Classes
{
    public class BankAccDetail
    {
        public string AccountNum { get; set; }
        public double AccountBalance { get; set; }
        public BankCode BankCode { get; set; }
        public AccountStatus AccStatus { get; set; }
    }

    public class CardDetail
    {
        public string CardNumber { get; set; }
        public double AccountBalance { get; set; }
        public BankCode BankCode { get; set; }
        public AccountStatus CardStatus { get; set; }
        public CurrencyEnum Currency { get; set; }
        public CardClassification CardClass { get; set; }

    }
}
