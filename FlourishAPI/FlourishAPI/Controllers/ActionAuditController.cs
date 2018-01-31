using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlourishAPI.DTOs;
using FlourishAPI.Models.Audit;
using FlourishAPI.Models;

namespace FlourishAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/ActionAudit")]
    public class ActionAuditController : Controller
    {

        [HttpPost]
        public bool LogAction([FromBody] LogActionDTO log)
        {
            log.LogTime = DateTime.Now;
            new ActionsAud(log).InsertDocument("FlourishAUD_DB");
            return true;
        }
    }
}