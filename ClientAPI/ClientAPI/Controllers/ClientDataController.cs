using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlourishAPI.Models.Classes;
using FlourishAPI.Models;

namespace ClientAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/ClientData")]
    public class ClientDataController : Controller
    {
        [HttpGet]
        public string CheckStatus() {

            return "You are connectec to Us";

        }

        [HttpGet("SyncAllEmployees")]
        
        public IEnumerable<Employee> SyncAllEmployees() {

            Employee e = new Employee();
            e.InsertDocument("ClientDB");
            List<Employee> emps = e.GetAllEmployees("ClientDB");
            emps.Remove(e);
            e.Delete("ClientDB");

            return emps;

        }

        [HttpPost("SyncEmployees")]
        public IEnumerable<Employee> SyncEmployees(string[] empIDs) {

            Employee e = new Employee();
            e.InsertDocument("ClientDB");
            List<Employee> emps = e.GetAllEmployees("ClientDB");
            emps.Remove(e);
            e.Delete("ClientDB");

            return emps.Where(x => x.EmployeeStatus == EmployeeStatus.Employed).ToList();

        }






    }
}