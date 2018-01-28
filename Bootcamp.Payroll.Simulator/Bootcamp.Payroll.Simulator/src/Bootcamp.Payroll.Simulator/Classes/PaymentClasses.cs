using Bootcamp.Payroll.Simulator.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bootcamp.Payroll.Simulator.Classes
{
    public class BankPayReq
    {
        public string AccountNum { get; set; }
        public int BCode { get; set; }
        public double Amount { get; set; }
    }

    public class BankPayResponse
    {
        public int PayRes { get; set; }
        public string Message { get; set; }
    }

    public class CardPayReq
    {
        public string CardNum { get; set; }
        public int bCode { get; set; }
        public int CardClass { get; set; }
        public double Amount { get; set; }
    }


    public class CardPayResponse
    {
        public int PayRes { get; set; }
        public string Message { get; set; }
    }
}
