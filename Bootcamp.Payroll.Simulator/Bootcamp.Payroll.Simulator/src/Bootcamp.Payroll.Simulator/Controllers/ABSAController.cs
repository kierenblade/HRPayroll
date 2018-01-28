using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Bootcamp.Payroll.Simulator.Classes;
using Bootcamp.Payroll.Simulator.Services;
using Bootcamp.Payroll.Simulator.Enums;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Bootcamp.Payroll.Simulator.Controllers
{
    [Route("api/[controller]")]
    public class ABSAController : Controller
    {
        private ILogger logger;
        private IOptions<SimAppSettings> appSettings;
        private BankAccountService bankService;
        private BankCode bCode;

        public ABSAController(ILoggerFactory loggerFactory, IOptions<SimAppSettings> appSettings)
        {
            this.appSettings = appSettings;
            this.logger = loggerFactory.CreateLogger<ABSAController>();
            this.bankService = new BankAccountService(loggerFactory, appSettings);
            this.bCode = BankCode.ABSA;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [Route("ProccessPayment")]
        public ActionResult ProcessPayment([FromBody]ABSAPaymentRequest payReq)
        {
            var amount = double.Parse(payReq.AmountToPay, System.Globalization.CultureInfo.InvariantCulture);
            BankPayReq req = new BankPayReq() { AccountNum = payReq.OriginationAccount, Amount = amount, BCode = (int)this.bCode };
            var res = this.bankService.ProcessPayment(req);
            return new ObjectResult( new ABSAPaymentResponse() {SuccessCode = res.PayRes, Message = res.Message });
        }

        [HttpPost]
        [Route("ABSALookup")]
        public ActionResult Lookup([FromBody]ABSAAccountRequest req)
        {
            AccountLookupReq aReq = new AccountLookupReq() { AccountNum = req.AccountNumber, BankCode = (int)this.bCode };

            var res = this.bankService.AccountLookup(aReq);

            return new ObjectResult(new ABSAAccountResponse() { AccountStatusCode = res.Status, AccountStatusDescription = res.Message});
        }
    }
}
