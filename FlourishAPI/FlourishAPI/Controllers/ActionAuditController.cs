using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlourishAPI.DTOs;
using FlourishAPI.Models.Audit;

namespace FlourishAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/ActionAudit")]
    public class ActionAuditController : Controller
    {

        [HttpPost]
        public void LogAction([FromBody] LogActionDTO log)
        {
          //  new ActionsAud() { }
        }
    }
}