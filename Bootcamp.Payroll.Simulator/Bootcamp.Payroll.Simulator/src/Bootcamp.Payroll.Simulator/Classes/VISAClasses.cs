using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bootcamp.Payroll.Simulator.Classes
{
    public class VisaPaymentRequest
    {
        public string DestinationCardNumber { get; set; }
        public string OriginatorCardNumber { get; set; }
        public int OriginatorCVV { get; set;}
        public string TransactionAmount { get; set; }
        public string Currency { get; set; }
        public string VendorID { get; set; }
        public int OriginatorBankCode { get; set; }
    }
    public class VisaPaymentResponse
    {
        public int PaymentResultCode { get; set; }
        public string PaymentResultDescription { get; set; }
    }

    public class VisaAccountRequest
    {
        public string VendorID { get; set; }
        public string CardNumber { get; set; }
        public int OriginatorBankCode { get; set; }
    }
    public class VisaAccountResponse
    {
        public int CardStatusCode { get; set; }
        public string CardStatusDescription { get; set; }
    }
}
