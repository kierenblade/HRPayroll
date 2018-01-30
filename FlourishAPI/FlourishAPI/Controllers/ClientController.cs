using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading;
using FlourishAPI.Models;
using FlourishAPI.Models.Classes;
using FlourishAPI.Models.Scheduler;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

namespace FlourishAPI.Controllers
{
    [Route("api/[controller]")]
    public class ClientController : Controller
    {
        //[HttpGet]
        //public string Get()
        //{
        //    return "test";
        //}

        [HttpPost]
        public async void Post()
        {
            Console.WriteLine("POST");
        }

        [HttpGet("SyncEmp")]
        public void SyncEmployees()
        {
            EmployeeSync.SyncEmployeeDetailsFromClient().Wait();

        }

        [HttpPost("RecieveEmployeeDetails")]
        public async void RecieveEmployeeDetails([FromBody] List<Employee> employeeDetails)
        {
            await EmployeeSync.InsertUpdateEmployeeDetails(employeeDetails);
        }

        [HttpPost("CompanyOnboarding")]
        public async void CompanyOnboarding([FromBody] Company newCompany)
        {
            newCompany.InsertDocument();
        }

        [HttpGet("SyncAllEmployees")]
        public async void SyncAllEmployees()
        {
            string url = "http://172.18.12.209/api/ClientData/SyncAllEmployees";
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    var rootresult = JsonConvert.DeserializeObject<List<Employee>>(result);

                    await EmployeeSync.InsertUpdateEmployeeDetails(rootresult);
                }
            }
        }
    }
}