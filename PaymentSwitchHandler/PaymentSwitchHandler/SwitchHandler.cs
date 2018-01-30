using HRPayroll.Classes.Models;
using MongoDB.Bson;
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
        private static string _apiUrl = "http://localhost:62337";
        //public async task that will handle the processing of a list of transactions
        public static async Task<List<ProcessResults>> ProcessTransaction(List<Transaction> transactionsIn)
        {
            //represents the list of all processed transactions and their completion status
            List<ProcessResults> output = new List<ProcessResults>();
            foreach (var transaction in transactionsIn)
            {
                //using the appropraite payment switch for the appropriate payment type
                switch (transaction.Employee.PaymentType)
                {
                    case PaymentType.ABSA:
                        //processes the transaction in accordance with the ABSA payswitch
                        output.Add(await ProcessAbsa(transaction));
                        break;
                    case PaymentType.VISA:
                        output.Add(await ProcessVisa(transaction));
                        break;
                    default:
                        output.Add(new ProcessResults() { TransactionId = transaction.Id, FailReason = "Invalid Payment Type. Only [ABSA] and [Visa] supported" , Code = StatusCode.Other});
                        break;
                }
            }
            return output;
        }

        private static async Task<ProcessResults> ProcessAbsa(Transaction t)
        {
            
            string paymentURL = _apiUrl + "/api/ABSA/ProccessPayment";
            //create an object matching how the ABSA Api accepts requests
            ABSARequest req;
            try
            {
                req = new ABSARequest()
                {
                    AmountToPay = t.Amount.ToString(),
                    ClientID = t.Company.CompanyId.ToString(),
                    DestinationAccount = t.Employee.AccountNumber,
                    OriginationAccount = t.Company.AccountNumber
                };

                switch (t.Employee.Bank.Name)
                {
                    case "ABSA":
                        req.DestinationBankCode = (int)BankCode.ABSA;
                        break;
                    case "FNB":
                        req.DestinationBankCode = (int)BankCode.FNB;
                        break;
                    default:
                        req.DestinationBankCode = (int)BankCode.None;
                        break;
                }

                //req.DestinationBankCode = (int)BankCode.ABSA;

            }
            catch (Exception e)
            {
                return new ProcessResults() { TransactionId = t.Id, FailReason = e.Message, Code = StatusCode.Other};
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
                    return new ProcessResults() { TransactionId = t.Id, FailReason = "Post request failed" , Code = StatusCode.Network};
                }
                
                //if the result of the transaction wasn't succesfull, returns the results along with the fialure reason
                if (result.SuccessCode != 1)
                {
                    return new ProcessResults() { TransactionId = t.Id, FailReason = result.Message , Code = StatusCode.Other};
                }
            }

            return new ProcessResults() { TransactionId = t.Id, Code = StatusCode.Success };
        }

        private static async Task<ProcessResults> ProcessVisa(Transaction t)
        {
            string paymentURL = _apiUrl + "/api/VISA/ProccessPayment";
            VisaRequest req;
            try
            {
                req = new VisaRequest()
                {
                    Currency = Currency.ZAR,
                    DestinationCardNumber = t.Employee.CardNumber, //add
                    OriginatorCardNumber = t.Company.CardNumber, //add
                    OriginatorCVV = int.Parse(t.Company.CardCVV),//add
                    TransactionAmount = t.Amount.ToString(),
                    VendorID = t.Company.CompanyId
                };

                switch (t.Company.Bank.Name)
                {
                    case "ABSA":
                        req.OriginatorBankCode = (int)BankCode.ABSA;
                        break;
                    case "FNB":
                        req.OriginatorBankCode = (int)BankCode.FNB;
                        break;
                    default:
                        req.OriginatorBankCode = (int)BankCode.None;
                        break;
                }

            }
            catch (Exception e)
            {
                return new ProcessResults() { TransactionId = t.Id, FailReason = e.Message , Code = StatusCode.Other};
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
                    return new ProcessResults() { TransactionId = t.Id, FailReason = "Post request failed" , Code = StatusCode.Network};
                }

                if (result.SuccessCode != 1)
                {
                    return new ProcessResults() { TransactionId = t.Id, FailReason = result.Message , Code = StatusCode.Other};
                }
            }

            return new ProcessResults() { TransactionId = t.Id, Code = StatusCode.Success };
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
        public ObjectId TransactionId { get; set; }
        public string FailReason { get; set; }

        public StatusCode Code { get; set; }
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
        public Currency Currency { get; set; }
        public int VendorID { get; set; }
        public int OriginatorBankCode { get; set; }
    }
    public enum StatusCode
    {
        Network = 1,
        Other,
        Success
    }

    enum Currency
    {
        None = 0,
        ZAR,
        USD,
        GBP,
        EUR
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

}

