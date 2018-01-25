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
    [Produces("application/json")]
    [Route("api/Client")]
    public class ClientController : Controller
    {
        [HttpGet]
        public static async void SyncAllEmployees()
        {
            string url = "";
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

        [HttpPost]
        public async void Post()
        {
            Console.WriteLine("POST");
        }
    }
}