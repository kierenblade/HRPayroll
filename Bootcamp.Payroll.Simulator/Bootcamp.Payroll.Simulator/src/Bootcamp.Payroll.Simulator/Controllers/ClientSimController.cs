using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Bootcamp.Payroll.Simulator.Controllers
{
    [Route("api/[controller]")]
    public class ClientSimController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        

        [HttpGet]
        [Route("CheckClientStatus")]
        public ActionResult CheckClientStatus()
        {
            return new ObjectResult("Success");
        }

        [HttpPost]
        [Route("ActionClientPushEmployee")]
        public void ActionClientPushEmployee([FromBody]List<string> empIDs)
        {

        }

    }
}
