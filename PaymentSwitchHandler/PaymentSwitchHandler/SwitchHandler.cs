using HRPayroll.Classes.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSwitchHandler
{

    public class SwitchHandler
    {
        //public async task that will handle the processing of a list of transactions
        public static async Task<List<ProcessResults>> ProcessTransaction(List<Transaction> transactionsIn)
        {
            //represents the list of failed processes that will be returned to the caller for notification purposese
            List<ProcessResults> output = new List<ProcessResults>();
            foreach (var transaction in transactionsIn)
            {
                //using the appropraite payment switch for the appropriate payment type
                switch (transaction.Employee.PaymentType.Name.ToUpper())
                {
                    case "ABSA":
                        //processes the transaction in accordance with the ABSA payswitch
                        var processABSA = await ProcessAbsa(transaction);
                        //if the payswitch responds with an anomoly, log it to the list of output values that failed.
                        if (processABSA != null)
                        {
                            output.Add(processABSA);
                        }
                        break;
                    case "VISA":
                        var processVisa = await ProcessVisa(transaction);
                        if (processVisa != null)
                        {
                            output.Add(processVisa);
                        }
                        break;
                    default:
                        //report that the payment method is not supported
                        output.Add(new ProcessResults() { TransactionId = transaction.TransactionId, FailReason = "Invalid Payment Type. Only [Absa] and [Visa] supported" , Code = FailCode.Other});
                        break;
                }
            }
            //if there are erroneous output values, reutnr it to the caller
            if (output.Count > 0)
            {
                return output;
            }
            return null;
        }

        private static async Task<ProcessResults> ProcessAbsa(Transaction t)
        {
            
            string paymentURL = "[ip]/api/ABSA/Payment";
            //create an object matching how the ABSA Api accepts requests
            ABSARequest req;
            try
            {
                req = new ABSARequest()
                {
                    AmountToPay = t.Amount.ToString(),
                    ClientID = t.Company.CompanyId.ToString(),
                    DestinationAccount = t.Employee.AccountNumber,
                    DestinationBankCode = t.Employee.Bank.BankId,
                    OriginationAccount = t.Company.AccountNumber
                };

            }
            catch (Exception e)
            {
                return new ProcessResults() { TransactionId = t.TransactionId, FailReason = e.Message, Code = FailCode.Other};
            }

            using (var client = new HttpClient())
            {
                //convert the transaction request to an applicable type
                var request = JsonConvert.SerializeObject(req);
                var buffer = Encoding.UTF8.GetBytes(request);
                var bytecontent = new ByteArrayContent(buffer);
                bytecontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                //sends the transaction to the API via POSt
                HttpResponseMessage res = await client.PostAsync(paymentURL, bytecontent);
                PaymentResponse result;
                if (res.IsSuccessStatusCode)
                {
                    //converts the API response into the expected response object
                    result = JsonConvert.DeserializeObject<PaymentResponse>(await res.Content.ReadAsStringAsync());
                }
                else
                {
                    //returns that the connection to the API failed. 
                    return new ProcessResults() { TransactionId = t.TransactionId, FailReason = "Post request failed" , Code = FailCode.Network};
                }
                
                //if the result of the transaction wasn't succesfull, returns the results along with the fialure reason
                if (result.SuccessCode != 1)
                {
                    return new ProcessResults() { TransactionId = t.TransactionId, FailReason = result.Message , Code = FailCode.Network};
                }
                return null;
            }
        }

        private static async Task<ProcessResults> ProcessVisa(Transaction t)
        {
            string paymentURL = "[ip]/api/ProcessPayment";
            VisaRequest req;
            try
            {
                req = new VisaRequest()
                {
                    Currency = "ZAR",
                    DestinationCardNumber = t.Employee.AccountNumber, //add
                    OriginatorBankCode = t.Company.Bank.BankId,
                    OriginatorCardNumber = "", //add
                    OriginatorCVV = 0,//add
                    TransactionAmount = t.Amount.ToString(),
                    VendorID = t.Company.CompanyId
                };
            }
            catch (Exception e)
            {
                return new ProcessResults() { TransactionId = t.TransactionId, FailReason = e.Message , Code = FailCode.Other};
            }

            using (var client = new HttpClient())
            {
                var request = JsonConvert.SerializeObject(req);
                var buffer = Encoding.UTF8.GetBytes(request);
                var bytecontent = new ByteArrayContent(buffer);
                bytecontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage res = await client.PostAsync(paymentURL, bytecontent);
                PaymentResponse result;
                if (res.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<PaymentResponse>(await res.Content.ReadAsStringAsync());
                }
                else
                {
                    return new ProcessResults() { TransactionId = t.TransactionId, FailReason = "Post request failed" , Code = FailCode.Network};
                }

                if (result.SuccessCode != 1)
                {
                    return new ProcessResults() { TransactionId = t.TransactionId, FailReason = result.Message , Code = FailCode.Other};
                }
                return null;
            }
        }
    }

    //the format of JSON objects that are expected from the APIs
    public class PaymentResponse
    {
        public int SuccessCode { get; set; }
        public string Message { get; set; }
    }
    //the reply that will be sent back to the caller to handle failed transactions
    public class ProcessResults
    {
        public int TransactionId { get; set; }
        public string FailReason { get; set; }

        public FailCode Code { get; set; }
    }
    //format of an ABSA transaction request
    class ABSARequest
    {
        public string OriginationAccount { get; set; }
        public string DestinationAccount { get; set; }
        public int DestinationBankCode { get; set; }
        public string ClientID { get; set; }
        public string AmountToPay { get; set; }
    }
    //format of a Visa Transaction Request
    class VisaRequest
    {
        public string DestinationCardNumber { get; set; }
        public string OriginatorCardNumber { get; set; }
        public int OriginatorCVV { get; set; }
        public string TransactionAmount { get; set; }
        public string Currency { get; set; }
        public int VendorID { get; set; }
        public int OriginatorBankCode { get; set; }
    }
    public enum FailCode
    {
        Network = 1,
        Other
    }
}

