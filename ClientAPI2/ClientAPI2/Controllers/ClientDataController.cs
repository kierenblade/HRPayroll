using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlourishAPI.Models.Classes;
using FlourishAPI.Models;


namespace ClientAPI2.Controllers
{
    [Produces("application/json")]
    [Route("api/ClientData")]
    public class ClientDataController : Controller
    {
        [HttpGet]
        public string CheckStatus()
        {

            return "You are connectec to Us";

        }

        [HttpGet("SyncAllEmployees")]

        public IEnumerable<Employee> SyncAllEmployees()
        {

            Employee e = new Employee();
            e.InsertDocument("ClientDB");
            List<Employee> emps = e.GetAllEmployees("ClientDB");
            emps.Remove(e);
            e.Delete("ClientDB");

            return emps;

        }

        [HttpPost("SyncEmployees")]
        public IEnumerable<Employee> SyncEmployees(string[] empIDs)
        {

            Employee e = new Employee();
            e.InsertDocument("ClientDB");
            List<Employee> emps = e.GetAllEmployees("ClientDB");
            emps.Remove(e);
            e.Delete("ClientDB");

            return emps.Where(x => x.EmployeeStatus == EmployeeStatus.Employed).ToList();

        }

        [HttpPost("SyncEmployees4Today")]
        public IEnumerable<Employee> GetEmployees4Today(string[] empIDs)
        {

            Employee e = new Employee();
            e.InsertDocument("ClientDB");
            List<Employee> emps = e.GetAllEmployees("ClientDB");
            emps.Remove(e);
            e.Delete("ClientDB");

            List<Employee> weekly = emps.Where(x => x.PayFrequency == PayFrequency.Weekly).ToList();
            List<Employee> monthly = emps.Where(x => x.PayFrequency == PayFrequency.Monthly).ToList();
            List<Employee> toSend = new List<Employee>();

            foreach (Employee item in weekly)
            {
                if (item.PayDate ==  (int)DateTime.Now.DayOfWeek)
                {
                    weekly.Add(item);
                }
            }

            foreach (Employee item in monthly)
            {
                if (item.PayDate == DateTime.Now.Day)
                {
                    monthly.Add(item);
                }
            }

            toSend = monthly;
            
            foreach (Employee item in weekly)
            {
                toSend.Add(item);
            }

            return toSend;

        }

    }
}