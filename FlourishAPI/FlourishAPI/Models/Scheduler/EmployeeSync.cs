using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using FlourishAPI.Models.Classes;
using FluentScheduler;
using HRPayroll.EmailService;
using Newtonsoft.Json;
using System.Threading;

namespace FlourishAPI.Models.Scheduler
{
    public class EmployeeSync : IJob
    {
        public EmployeeSync()
        {
            
        }
        public void Execute()
        {
            Console.WriteLine("Sync started");
            SyncEmployeeDetailsFromClient().Start();
        }

        public static async Task SyncEmployeeDetailsFromClient()
        {
            bool retryFlag = true;
            int retryCount = 0;

            string url = "http://localhost:54497/api/values";
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                while (retryFlag && retryCount <= 3)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        var employeeResult = JsonConvert.DeserializeObject<List<Employee>>(result);

                        Employee e = new Employee();
                        e.InsertDocument();
                        List<CRUDAble> existingEmployees = e.SearchDocument(new Dictionary<string, object>());
                        existingEmployees.Remove(e);

                        List<CRUDAble> crud = new List<CRUDAble>();
                        foreach (Employee employee in employeeResult)
                        {
                            Dictionary<string, object> filterList = new Dictionary<string, object>();
                            filterList.Add("IdNumber", employee.IdNumber);
                            if (e.SearchDocument(filterList).LongCount() > 0)
                            {
                                filterList.Clear();
                                filterList.Add("HashCode", employee.HashCode);
                                List<CRUDAble> preFilter = e.SearchDocument(filterList).ToList();
                                preFilter.Remove(e);
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
                        new DesktopNotification() { Company = employeeResult[0].Company, CreationDate = DateTime.Now, Message = (crud.Count + " records have been updated from the previous list") }.InsertDocument();
                        //==================================================
                        //  new Thread(UpdateAndCreateTransactions).Start();
                        UpdateAndCreateTransactions();
                        retryFlag = false;
                    }
                    else
                    {
                        //Send an email notification about the response
                        MailMessage emailMessage = new MailMessage
                        {
                            Subject = "Failed to sync Employee details",
                            Body = "Failed to sync Employee details from Client with error code \"" + response.StatusCode + "\"",
                            To = { new MailAddress("kieren.gerling@sybrin.co.za") }
                        };

                        EmailHandler.SendMail(emailMessage);

                        //Wait X amount of time before retrying
                        Thread.Sleep(200);
                        retryCount++;
                    }
                }
            }
        }

        public static void UpdateAndCreateTransactions()
        {
            Employee e = new Employee() { Company = new Company(), BusinessUnit = new BusinessUnit() };
            Transaction t = new Transaction() { Employee = new Employee(), Company = new Company() };
            e.InsertDocument();
            t.InsertDocument();
            List<CRUDAble> existingEmployees = e.SearchDocument(new Dictionary<string, object>());
            existingEmployees.Remove(e);
            List<Transaction> toUpdateTransactions = new List<Transaction>();

            foreach (Employee item in existingEmployees)
            {
                Dictionary<string, object> filterList = new Dictionary<string, object>();
                filterList.Add("Employee.HashCode", item.HashCode);
                List<CRUDAble> result = t.SearchDocument(filterList);
                result.Remove(t);
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
