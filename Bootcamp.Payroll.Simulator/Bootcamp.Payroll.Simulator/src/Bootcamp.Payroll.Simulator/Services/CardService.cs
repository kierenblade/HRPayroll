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
    public class CardService
    {
        private IOptions<SimAppSettings> appSettings;
        private ILogger logger;
        private DBService dbService;
        public CardService(ILoggerFactory loggerFactory, IOptions<SimAppSettings> appSettings)
        {
            this.logger = loggerFactory.CreateLogger<BankAccountService>();
            this.appSettings = appSettings;
            this.dbService = new DBService(loggerFactory, appSettings);
        }


        public CardPayResponse ProcessPayment(CardPayReq payReq)
        {
            var bankAccs = getCardAccByFullDetails(payReq.bCode, payReq.CardNum, payReq.CardClass);
            if (bankAccs.Count <= 0)
                return GetPayResponse("Account does not exist", PaymentResult.Failure);

            var bankAcc = bankAccs[0];
            if (bankAcc.AccountBalance < payReq.Amount)
                return GetPayResponse("Insufficent Funds", PaymentResult.Failure);

            return GetPayResponse("Transaction Sucessfull", PaymentResult.Success);
        }

        public LookUpRes AccountLookup(CardLookup req)
        {
            var accs = getCardAccByFullDetails(req.BankCode, req.CardNum, req.CardClass);

            if (accs.Count <= 0)
                return GetLookupRes(AccountStatus.None, $"Account does not exist.");

            return GetLookupRes(accs[0].CardStatus, accs[0].CardStatus.ToString());
        }

        private LookUpRes GetLookupRes(AccountStatus stat, string message)
        {
            return new LookUpRes() { Message = message, Status = (int)stat };
        }


        private CardPayResponse GetPayResponse(string message, PaymentResult payRes)
        {
            return new CardPayResponse() { PayRes = (int)payRes, Message = $"{message}" };
        }


        public List<CardDetail> getCardAccByBankCode(int bankCode)
        {
            return this.dbService.GetCardDetails($" WHERE [BankCode] = {bankCode}");
        }
        public List<CardDetail> getCardAccByCardNum(string cardNum)
        {
            return this.dbService.GetCardDetails($" WHERE [CardNumber] = {cardNum}");
        }
        public List<CardDetail> getCardAccByCardClassDetails(int cardClass)
        {
            return this.dbService.GetCardDetails($" WHERE [CardClassification] = {cardClass} ");
        }
        public List<CardDetail> getCardAccByFullDetails(int bankCode, string crdNum, int cardClass)
        {
            return this.dbService.GetCardDetails($" WHERE [BankCode] = {bankCode} AND [CardNumber] = {crdNum} AND [CardClassification] = {cardClass}");
        }


        public void SaveCardDetail(CardDetail cardDet)
        {
            this.dbService.SaveCardDetails(cardDet);
        }
    }
}
