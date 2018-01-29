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
using System.Threading;

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

                    Employee e = new Employee();
                    e.InsertDocument();
                    List<CRUDAble> existingEmployees = e.SearchDocument(new Dictionary<string, object>());

                    List<CRUDAble> crud = new List<CRUDAble>();
                    //List<Employee> emp = new List<Employee>();
                    //foreach (Employee item in existingEmployees)
                    //{
                    //    emp.Add(item);
                    //}
                    foreach (Employee employee in employeeResult)
                    {
                        Dictionary<string, object> filterList = new Dictionary<string, object>();
                        filterList.Add("IdNumber", employee.IdNumber);
                        if (e.SearchDocument(filterList).LongCount() > 0)
                        {
                            filterList.Clear();
                            filterList.Add("HashCode", employee.HashCode);
                            List<CRUDAble> preFilter = e.SearchDocument(filterList).ToList();
                            if (preFilter.Count < 1)
                            {
                                crud.Add(employee);
                            }
                        }
                        else
                        {
                            employee.InsertDocument();
                        }
                    }
                    crud.UpdateManyDocument();
                    e.Delete();
                    //=================Testing notifications===================
                    new DesktopNotification() {Company = employeeResult[0].Company,CreationDate = DateTime.Now,Message = (crud.Count+ " records have been updated from the previous list") }.InsertDocument();
                    //==================================================
                    //  new Thread(UpdateAndCreateTransactions).Start();
                    UpdateAndCreateTransactions();
                }
            }
        }


        public void UpdateAndCreateTransactions()
        {
            Employee e = new Employee() { Company = new Company(), BusinessUnit = new BusinessUnit()};
            Transaction t = new Transaction() { Employee = new Employee(), Company = new Company() };
            e.InsertDocument();
            t.InsertDocument();
            List<CRUDAble> existingEmployees = e.SearchDocument(new Dictionary<string, object>());
            List<Transaction> toUpdateTransactions = new List<Transaction>();

            foreach (Employee item in existingEmployees)
            {
                Dictionary<string, object> filterList = new Dictionary<string, object>();
                filterList.Add("Employee.HashCode", item.HashCode);
                List<CRUDAble> result = t.SearchDocument(filterList);
                if (result.LongCount() > 0)
                {
                    result.UpdateManyDocument();
                }
                else
                {
                    new Transaction() { Employee = item, Company = item.Company, Amount = item.Salary, DateCreated = DateTime.Now, Status = Status.Pending }.InsertDocument();
                }
            }

            e.Delete();
            t.Delete();
            
        }
    }
}