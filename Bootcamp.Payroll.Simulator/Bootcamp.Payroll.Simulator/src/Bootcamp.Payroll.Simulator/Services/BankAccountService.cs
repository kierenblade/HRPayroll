using Bootcamp.Payroll.Simulator.Classes;
using Bootcamp.Payroll.Simulator.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bootcamp.Payroll.Simulator.Services
{
    public class BankAccountService
    {
        private IOptions<SimAppSettings> appSettings;
        private ILogger logger;
        private DBService dbService;
        public BankAccountService(ILoggerFactory loggerFactory, IOptions<SimAppSettings> appSettings)
        {
            this.logger = loggerFactory.CreateLogger<BankAccountService>();
            this.appSettings = appSettings;
            this.dbService = new DBService(loggerFactory, appSettings);
        }

        public BankPayResponse ProcessPayment(BankPayReq payReq)
        {
            var bankAccs = getBankAccByFullDetails(payReq.BCode, payReq.AccountNum);
            if (bankAccs.Count <= 0)
                return GetPayResponse("Account does not exist", PaymentResult.Failure);

            var bankAcc = bankAccs[0];
            if (bankAcc.AccountBalance < payReq.Amount)
                return GetPayResponse("Insufficent Funds", PaymentResult.Failure);

            return GetPayResponse("Transaction Sucessfull", PaymentResult.Success);
        }

        public LookUpRes AccountLookup(AccountLookupReq req)
        {
            var accs = getBankAccByFullDetails(req.BankCode, req.AccountNum);

            if (accs.Count <= 0)
                return GetLookupRes(AccountStatus.None, $"Account does not exist.");

            return GetLookupRes(accs[0].AccStatus, accs[0].AccStatus.ToString());
        }

        private LookUpRes GetLookupRes(AccountStatus stat, string message)
        {
            return new LookUpRes() { Message = message, Status = (int)stat };
        }

        private BankPayResponse GetPayResponse(string message, PaymentResult payRes)
        {
            return new BankPayResponse() { PayRes = (int)payRes, Message = $"{message}" };
        }


        private List<BankAccDetail> getBankAccByBankCode(int bankCode)
        {
            return this.dbService.GetBankDetails($" WHERE [BankCode] = {bankCode}");
        }
        private List<BankAccDetail> getBankAccByAccNum(string accNum)
        {
            return this.dbService.GetBankDetails($" WHERE [AccountNum] = {accNum}");
        }
        private List<BankAccDetail> getBankAccByFullDetails(int bankCode, string accNum)
        {
            return this.dbService.GetBankDetails($" WHERE [BankCode] = {bankCode} AND [AccountNum] = {accNum}");
        }

        public void SaveBankDetail(BankAccDetail bankDet)
        {
            this.dbService.SaveBankDetails(bankDet);
        }
    }
}
