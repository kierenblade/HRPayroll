using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlourishAPI.DTOs;
using HRPayroll.Classes.Models;

namespace FlourishAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Report")]
    public class ReportController : Controller
    {

        // GET api/Report/DashboardItem1
        [HttpGet("DashboardItem1/{id}")]
        public IEnumerable<DashboardData> DashboardItem1(string id)
        {

            List<DashboardData> dataToSend = new List<DashboardData>();

            return dataToSend;

        }

        [HttpGet("GetNotifications/{id}")]
        public IEnumerable<DesktopNotification> GetNotifications(string id)
        {

            List<DesktopNotification> notifications = new List<DesktopNotification>();

            var notes = notifications.GetRange(0, 5).OrderByDescending(x => x.NotificationId);

            foreach (var item in notes)
            {
                notifications.Add(item);
            }

            return notifications;
        }


        // POST api/Report/GenReportRequest
        [HttpPost("GenReportRequest")]
        public IEnumerable<Transaction> GeneralReportRequest([FromBody] GeneralReportRequestDTO reportRequestDetails)
        {
          

            List<Transaction> transactionsResults = new List<Transaction>();
            transactionsResults.Add(new Transaction() { Company = new Company() { Name = "Sybrin" }, Amount = 12000 });

            return transactionsResults;
        }



    }
}