using Bootcamp.Payroll.Simulator.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bootcamp.Payroll.Simulator.Classes
{
    public class AccountLookupReq
    {
        public string AccountNum { get; set; }
        public int BankCode { get; set; }
    }

    public class CardLookup
    {
        public string CardNum { get; set; }
        public int BankCode { get; set; }
        public int CardClass { get; set; }
    }

    public class LookUpRes
    {
        public int Status { get; set; }
        public string Message { get; set; }
    }
}
