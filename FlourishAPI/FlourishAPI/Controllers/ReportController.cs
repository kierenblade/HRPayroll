using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlourishAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Report")]
    public class ReportController : Controller
    {

        // PUT api/Report/GenReportRequest
        [HttpPost("GenReportRequest")]
        public string GeneralReportRequest(int id, [FromBody] string request)
        {
            string reportResponseJSON = "";

            return reportResponseJSON;
        }

        
    }
}