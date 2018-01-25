using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HRPayroll.Classes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using FlorishTestEnviroment;

namespace FlourishAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ClientController : Controller
    {
        [HttpGet]
        public void Get()
        {
            SyncAllEmployees();
        }

        [HttpPost]
        public async void Post()
        {
            Console.WriteLine("POST");
        }

        [HttpPost("{query}")]
        public async void Post(string query)
        {
            switch (query)
            {
                case "SyncEmp":

                    break;
            }
        }

        [HttpPost]
        public async void SyncEmp()
        {
            
        }

        public async void SyncAllEmployees()
        {
            string url = "http://localhost:54497/api/values";
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    var employeeResult = JsonConvert.DeserializeObject<List<Employee>>(result);

                    List<CRUDAble> crud = new List<CRUDAble>();
                    foreach (Employee employee in employeeResult)
                    {
                        crud.Add(employee);
                    }

                    crud.UpdateManyDocument();
                }
            }
        }
    }
}