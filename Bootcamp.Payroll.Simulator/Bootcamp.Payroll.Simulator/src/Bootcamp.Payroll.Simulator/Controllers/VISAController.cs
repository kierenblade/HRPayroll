using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bootcamp.Payroll.Simulator.Classes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Bootcamp.Payroll.Simulator.Services;
using Bootcamp.Payroll.Simulator.Enums;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Bootcamp.Payroll.Simulator.Controllers
{
    [Route("api/[controller]")]
    public class VISAController : Controller
    {

        private ILogger logger;
        private IOptions<SimAppSettings> appSettings;
        private CardService crdService;


        public VISAController(ILoggerFactory loggerFactory, IOptions<SimAppSettings> appSettings)
        {
            this.appSettings = appSettings;
            this.logger = loggerFactory.CreateLogger<ABSAController>();
            this.crdService = new CardService(loggerFactory, appSettings);
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [HttpPost]
        [Route("ProccessPayment")]
        public ActionResult ProcessPayment([FromBody]VisaPaymentRequest payReq)
        {
            var amount = double.Parse(payReq.TransactionAmount, System.Globalization.CultureInfo.InvariantCulture);
            CardPayReq req = new CardPayReq() { Amount = amount, bCode = (int)payReq.OriginatorBankCode, CardClass = (int)CardClassification.Visa, CardNum = payReq.OriginatorCardNumber };
            var res = this.crdService.ProcessPayment(req);
            return new ObjectResult(new VisaPaymentResponse() { PaymentResultCode = res.PayRes, PaymentResultDescription = res.Message });
        }

        [HttpPost]
        [Route("VisaLookup")]
        public ActionResult Lookup([FromBody]VisaAccountRequest req)
        {
            CardLookup aReq = new CardLookup() { CardNum = req.CardNumber, BankCode = (int)req.OriginatorBankCode };

            var res = this.crdService.AccountLookup(aReq);

            return new ObjectResult(new VisaAccountResponse() { CardStatusCode = res.Status, CardStatusDescription = res.Message });
        }
    }
}
