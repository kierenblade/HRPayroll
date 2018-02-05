using FlourishAPI.Models.Classes;
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
    /*
     * ===================================
     * Author: Jason van Heerden
     * Create date: 2018/01/29
     * Description: 
     * Takes in transactions to be processed by the selected payment switch. 
     * Payments are processed using the relevant web service. A list of  transactiton process results are returned.
     * This will represent he transaction IDs along with their success status.
     * Success represents a successful payment
     * Network represents an error in reaching the relevant service.
     * Other represents an error in processing the transacion. This will usually also contain a reason for failure, as provided by the web service.
     * Returning an empty list will represent the API not being reachable after 3 attempts (Subject for improvement)
     * 
     * NOTE: As per simulator restrictions, only VISA and ABSA payments are supported
     * ===================================
     */
    public class SwitchHandler
    {
        private string _apiUrl { get; set; }
        public SwitchHandler(string apiUrl)
        {
            this._apiUrl = apiUrl;
        }
        public async Task<List<TransactionProcessResult>> ProcessTransaction(List<Transaction> transactionsIn)
        {
            //Console.WriteLine("PayeBoi");
            List<TransactionProcessResult> output = new List<TransactionProcessResult>();
            //Attempt to reach the APi using a basic call. After 3 attempts, return an empty object, representing the api being unreachable
            for (int i = 0; i < 3; i++)
            {
                if (await TestApi())
                {
                    break;
                }
                if (i == 2)
                {
                    return output;
                }
            }
            //Reverting each transaction to its relevant payment method
            foreach (var transaction in transactionsIn)
            {
                switch (transaction.Company.PaymentType)
                {
                    case PaymentType.ABSA:
                        output.Add(await ProcessAbsa(transaction));
                        break;
                    case PaymentType.VISA:
                        output.Add(await ProcessVisa(transaction));
                        break;
                    default:
                        output.Add(new TransactionProcessResult() { TransactionId = transaction.Id, FailReason = "Invalid Payment Type. Only [ABSA] and [Visa] supported" , Code = StatusCode.Other});
                        break;
                }
            }
            return output;
        }

        //Processing ABSA transactions in accordance with the API specifications
        private async Task<TransactionProcessResult> ProcessAbsa(Transaction t)
        {
            string paymentURL = _apiUrl + "/api/ABSA/ProccessPayment";
            ABSARequest req;
            try
            {
                req = new ABSARequest()
                {
                    AmountToPay = t.Amount.ToString(),
                    ClientID = t.Company.CompanyId.ToString(),
                    DestinationAccount = t.Employee.AccountNumber,
                    OriginationAccount = t.Company.AccountNumber,
                    DestinationBankCode = t.Employee.Bank.BankId
                };
            }
            catch (Exception e)
            {
                return new TransactionProcessResult() { TransactionId = t.Id, FailReason = e.Message, Code = StatusCode.Other};
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
                    return new TransactionProcessResult() { TransactionId = t.Id, FailReason = "Post request failed" , Code = StatusCode.Network};
                }
                
                if (result.SuccessCode != 1)
                {
                    return new TransactionProcessResult() { TransactionId = t.Id, FailReason = result.Message , Code = StatusCode.Other};
                }
            }

            return new TransactionProcessResult() { TransactionId = t.Id, Code = StatusCode.Success };
        }

        //Processing VISA transactions in accordance with the API specifications
        private async Task<TransactionProcessResult> ProcessVisa(Transaction t)
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
                    OriginatorCVV = int.Parse(t.Company.CardCVV), //add
                    TransactionAmount = t.Amount.ToString(),
                    VendorID = t.Company.CompanyId,
                    OriginatorBankCode = t.Company.Bank.BankId
                };

            }
            catch (Exception e)
            {
                return new TransactionProcessResult() { TransactionId = t.Id, FailReason = e.Message , Code = StatusCode.Other};
            }

            using (var client = new HttpClient())
            {
                var request = JsonConvert.SerializeObject(req);
                var buffer = Encoding.UTF8.GetBytes(request);
                var bytecontent = new ByteArrayContent(buffer);
                bytecontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage res = await client.PostAsync(paymentURL, bytecontent);
                VisaPaymentResponse result;
                if (res.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<VisaPaymentResponse>(await res.Content.ReadAsStringAsync());
                }
                else
                {
                    return new TransactionProcessResult() { TransactionId = t.Id, FailReason = "Post request failed" , Code = StatusCode.Network};
                }

                if (result.PaymentResultCode != 1)
                {
                    return new TransactionProcessResult() { TransactionId = t.Id, FailReason = result.PaymentResultDescription , Code = StatusCode.Other};
                }
            }

            return new TransactionProcessResult() { TransactionId = t.Id, Code = StatusCode.Success };
        }

        //calling a simple get method in order to test if the service is reachable
        private async Task<bool> TestApi()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage res = await client.GetAsync(_apiUrl + "/api/values");
                return res.IsSuccessStatusCode;
            }
        }
    }

    //the format of response expected from processing an ABSA transaction
    class PaymentResponse
    {
        public int SuccessCode { get; set; }
        public string Message { get; set; }
    }

    //the format of response expected from processing a VISA transaction
    class VisaPaymentResponse
    {
        public int PaymentResultCode { get; set; }
        public string PaymentResultDescription { get; set; }
    }

    //The format of a payment request sent to the ABSA payment service
    class ABSARequest
    {
        public string OriginationAccount { get; set; }
        public string DestinationAccount { get; set; }
        public int DestinationBankCode { get; set; }
        public string ClientID { get; set; }
        public string AmountToPay { get; set; }
    }

    //The format of a payment request sent to the VISA payment service
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

