using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using FluentScheduler;
using HRPayroll.Classes.Models;
using Newtonsoft.Json;

namespace HRPayroll.Scheduler
{
    class EmployeeSync : IJob
    {
        public EmployeeSync()
        {
            
        }
        public void Execute()
        {
            Console.WriteLine("Sync started");
            SyncEmployeeDetailsFromClient();
        }

        public async void SyncEmployeeDetailsFromClient()
        {
            //Get list of employees from DB
            List<string> employeeIdList = new List<string>();

            //Send employee Id list to client to check for any changes
            string url = "http://localhost:51422/api/employee";
            using (var client = new HttpClient())
            {
                var mycontent = JsonConvert.SerializeObject(employeeIdList);
                var buffer = Encoding.UTF8.GetBytes(mycontent);
                var bytecontent = new ByteArrayContent(buffer);
                bytecontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.PostAsync(url, bytecontent);
                if (response.IsSuccessStatusCode)
                {
                    //Get the list of employee details from Client API
                    string result = await response.Content.ReadAsStringAsync();
                    List<Employee> rootresult = JsonConvert.DeserializeObject<List<Employee>>(result);

                    //Wade's code here
                }
            }
        }
    }
}
