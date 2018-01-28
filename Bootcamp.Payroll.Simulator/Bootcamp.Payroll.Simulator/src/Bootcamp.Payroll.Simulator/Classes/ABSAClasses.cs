using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bootcamp.Payroll.Simulator.Classes
{
    public class ABSAPaymentRequest
    {
        public string OriginationAccount { get; set; }
        public string DestinationAccount { get; set; }
        public int DestinationBankCode { get; set; }
        public string ClientID { get; set; }
        public string AmountToPay { get; set; }
        //public DateTime PaymentDate { get; set; }
        
    }

    public class ABSAPaymentResponse
    {
        public int SuccessCode { get; set; }
        public string Message { get; set; }
    }

    public class ABSAAccountRequest
    {
        public string AccountNumber { get; set; }
        public string ClientID { get; set; }
    }
    public class ABSAAccountResponse
    {
        public int AccountStatusCode { get; set; }
        public string AccountStatusDescription { get; set; }
    }

}
