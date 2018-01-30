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
        public async Task<bool> SyncEmployees()
        {
            //Get list of employees from DB
            Employee emp = new Employee();
            emp.InsertDocument();
            List<Employee> employeeList = emp.GetAllEmployees();
            emp.Delete();
            employeeList.Remove(emp);

            return await EmployeeSync.SyncEmployeeDetailsFromClient(employeeList);
        }

        [HttpPost("RecieveEmployeeDetails")]
        public async void RecieveEmployeeDetails([FromBody] List<Employee> employeeDetails)
        {
            await EmployeeSync.InsertUpdateEmployeeDetails(employeeDetails);
        }

        [HttpPost("CompanyOnboarding")]
        public async void CompanyOnboarding([FromBody] LoginDetails newCompany)
        {
            newCompany.Role = new Role() {
                Name = "admin"
            };
            Company c = new Company() {
                Bank = new Bank()
            };
            c.InsertDocument();
            List<CRUDAble> clist = c.SearchDocument(new Dictionary<string, object>());
            clist.Remove(c);
            c.Delete();
            int companyID = 0;
            List<Company> comp = new List<Company>();
            foreach (Company item in clist)
            {
                comp.Add(item);
            }
            for (int i = 0; i < 1000; i++)
            {
                if (comp.AsQueryable().Where(p => p.CompanyId == i).LongCount() < 1)
                {
                    companyID = i;
                    break;
                }
            }
            newCompany.Company.CompanyId = companyID;
            newCompany.InsertDocument();
            newCompany.Company.InsertDocument();
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