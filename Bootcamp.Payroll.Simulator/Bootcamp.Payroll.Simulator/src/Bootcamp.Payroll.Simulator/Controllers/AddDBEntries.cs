using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bootcamp.Payroll.Simulator.Classes;
using Bootcamp.Payroll.Simulator.Services;
using Bootcamp.Payroll.Simulator.Enums;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Bootcamp.Payroll.Simulator.Controllers
{
    [Route("api/[controller]")]
    public class AddDBEntries : Controller
    {
        private ILogger logger;
        private IOptions<SimAppSettings> appSettings;
        private BankAccountService bankService;
        private CardService cardService;

        public AddDBEntries(ILoggerFactory loggerFactory, IOptions<SimAppSettings> appSettings)
        {
            this.appSettings = appSettings;
            this.logger = loggerFactory.CreateLogger<AddDBEntries>();
            this.bankService = new BankAccountService(loggerFactory, appSettings);
            this.cardService = new CardService(loggerFactory, appSettings);
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [Route("AddBankAccount")]
        public void AddBankAccount([FromBody] BankAccDetail bankDet)
        {
            this.bankService.SaveBankDetail(bankDet);
        }

        [HttpPost]
        [Route("AddCardAccount")]
        public void AddCardAccount([FromBody] CardDetail cardDet)
        {
            this.cardService.SaveCardDetail(cardDet);
        }
    }
}
