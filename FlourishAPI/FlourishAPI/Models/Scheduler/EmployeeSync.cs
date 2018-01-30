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
            
            string url = "http://localhost:51422/api/Employee";
            using (var client = new HttpClient())
            {
                while (retryFlag && retryCount <= 3)
                {
                    //Get list of employees from DB
                    Employee emp = new Employee();
                    emp.InsertDocument();
                    List<Employee> employeeList = emp.GetAllEmployees();
                    emp.Delete();
                    employeeList.Remove(emp);
                    List<string> employeeIdList = new List<string>();
                    foreach (Employee employee in employeeList)
                    {
                        employeeIdList.Add(employee.IdNumber);
                    }

                    //Send the list of employee IDs to the client
                    var mycontent = JsonConvert.SerializeObject(employeeIdList);
                    var buffer = Encoding.UTF8.GetBytes(mycontent);
                    var bytecontent = new ByteArrayContent(buffer);
                    bytecontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    HttpResponseMessage responsePost = null;

                    try
                    {
                        //Try connect to the client
                        responsePost = await client.PostAsync(url, bytecontent);
                    }
                    catch (Exception e)
                    {
                        //Send an email notification about the response
                        MailMessage emailMessage = new MailMessage
                        {
                            Subject = "Failed to sync Employee details",
                            Body = "Failed to sync Employee details from Client with error \"" + e.Message + "\"",
                            To = { new MailAddress("kieren.gerling@sybrin.co.za") }
                        };

                        EmailHandler.SendMail(emailMessage);

                        //Wait X amount of time before retrying
                        Thread.Sleep(200);
                        retryCount++;
                        continue;
                    }

                    if (responsePost.IsSuccessStatusCode)
                    {
                        //Get the list of employee objects from the Client
                        string result = await responsePost.Content.ReadAsStringAsync();
                        var employeeResult = JsonConvert.DeserializeObject<List<Employee>>(result);

                        await InsertUpdateEmployeeDetails(employeeResult);
                        //=================Testing notifications===================
                       // new DesktopNotification() { Company = employeeResult[0].Company, CreationDate = DateTime.Now, Message = (crud.Count + " records have been updated from the previous list") }.InsertDocument();
                        //==================================================
                        //  new Thread(UpdateAndCreateTransactions).Start();
                        await UpdateAndCreateTransactions();
                        retryFlag = false;
                    }
                }
            }
        }

        public static async Task InsertUpdateEmployeeDetails(List<Employee> employeeList)
        {
            Employee e = new Employee();
            e.InsertDocument();
            List<CRUDAble> existingEmployees = e.SearchDocument(new Dictionary<string, object>());
            existingEmployees.Remove(e);

            List<CRUDAble> crud = new List<CRUDAble>();
            foreach (Employee employee in employeeList)
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
        }

        public static async Task UpdateAndCreateTransactions()
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
                filterList.Add("Employee.IdNumber", item.IdNumber);
                List<CRUDAble> result = t.SearchDocument(filterList);
                result.Remove(t);
                if (result.LongCount() > 0)
                {
                    List<CRUDAble> emp = new List<CRUDAble>(); 
                    foreach (Transaction i in result)
                    {
                        i.Employee = item;
                        emp.Add(i);
                    }
                    emp.UpdateManyDocument();
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
