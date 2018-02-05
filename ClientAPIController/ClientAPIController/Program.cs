using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using HRPayroll.Classes.Models;

namespace ClientAPIController
{
    class Program
    {
        //demoBoye
        private static readonly HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            var results = GetPosts().GetAwaiter().GetResult();
            //foreach (var item in results)
            //{
            //    Console.WriteLine(item);
            //}
            Console.WriteLine(results); 
            Console.ReadLine();
        }

        public static async Task<string> GetPosts()
        {
            List<Employee> employees = new List<Employee>();
            Employee employee = new Employee()
            {
                IdNumber = "1111111",
                FirstName = "test",
                LastName = "test",
                AccountNumber = "12345",
                Bank = new Bank() { BankId = 1, Name = "Standard" },
                BusinessUnitName = "Sales",
                Position = "pleb",
                Salary = 123.2m,
                PayFrequency = PayFrequency.Weekly,
                PayDate = DateTime.Today,
                PaymentType = new PaymentType { Name = "testType" },
                EmployeeStatus = EmployeeStatus.Employed,
                SyncDate = DateTime.Now
            };

            employees.Add(employee);

            //string URL = "http://localhost:63796/api/client";
            //using (var client = new HttpClient())
            //{
            //    HttpResponseMessage response = await client.GetAsync(URL);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        string result = await response.Content.ReadAsStringAsync();
            //        var rootresult = JsonConvert.DeserializeObject<List<Test>>(result);
            //        return rootresult;
            //    }
            //    else
            //    {
            //        return null;
            //    }
            //}


            string url = "http://localhost:63796/api/client";
            using (var client = new HttpClient())
            {
                var mycontent = JsonConvert.SerializeObject(employees);
                var buffer = System.Text.Encoding.UTF8.GetBytes(mycontent);
                var bytecontent = new ByteArrayContent(buffer);
                bytecontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.PostAsync(url, bytecontent);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    //var rootresult = JsonConvert.DeserializeObject<List<string>>(result);
                    return result;
                }
                else
                {
                    return null;
                }
            }

            //string url = "http://localhost:63796/api/Login/AlternativeGet";
            //using (var client = new HttpClient())
            //{
            //    var response = await client.GetAsync(url);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        string result = await response.Content.ReadAsStringAsync();
            //        //var rootresult = JsonConvert.DeserializeObject<List<string>>(result);
            //        return result;
            //    }
            //    else
            //    {
            //        return null;
            //    }
            //}
        }
        public class Tshief
        {
            public string name { get; set; }
            public string status { get; set; }
        }
        public class Test
        {
            public int Userid { get; set; }
            public int Id { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
        }
    }
}
