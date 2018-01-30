using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using FlourishAPI.Models.Classes;
using FluentScheduler;
using Newtonsoft.Json;
using System.Threading;

namespace FlourishAPI.Models.Scheduler
{

    public class EmployeeSync : IJob
    {
        private const string _clientEmpSyncURL = "http://localhost:51422/api/Employee";
        public EmployeeSync()
        {
            
        }
        public async void Execute()
        {
            new EventLogger("STARTED: Scheduled Employee Sync Tasks", Severity.Event).Log();
            await SyncEmployeeDetailsFromClient();
            new EventLogger("COMPLETED: Scheduled Employee Sync Tasks", Severity.Event).Log();
        }

        public static async Task SyncEmployeeDetailsFromClient()
        {
            new EventLogger("STARTED: Syncing Employee details from Client", Severity.Event).Log();
            bool retryFlag = true;
            int retryCount = 0;
            
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
                        responsePost = await client.PostAsync(_clientEmpSyncURL, bytecontent);
                    }
                    catch (Exception e)
                    {
                        new EventLogger(
                            string.Format("Failed to sync employee details from client. Exception: \"{0}\"", e.Message),
                            Severity.Severe).Log();

                        //Send an email notification about the response
                        MailMessage emailMessage = new MailMessage
                        {
                            Subject = "Failed to sync Employee details",
                            Body = "Failed to sync Employee details from Client with error \"" + e.Message + "\"",
                            To = { new MailAddress("gerling.kieren@gmail.com") }
                        };
                        
                        EmailHandler.SendMail(emailMessage);

                        //Wait X amount of time before retrying
                        Thread.Sleep(200);
                        retryCount++;
                        continue;
                    }

                    if (responsePost.IsSuccessStatusCode)
                    {
                        new EventLogger("Connected to client API to recieve employee details", Severity.Event).Log();

                        //Get the list of employee objects from the Client
                        string result = await responsePost.Content.ReadAsStringAsync();
                        var employeeResult = JsonConvert.DeserializeObject<List<Employee>>(result);

                        await InsertUpdateEmployeeDetails(employeeResult);
                        //=================Testing notifications===================
                       // new DesktopNotification() { Company = employeeResult[0].Company, CreationDate = DateTime.Now, Message = (crud.Count + " records have been updated from the previous list") }.InsertDocument();
                        //==================================================
                        //  new Thread(UpdateAndCreateTransactions).Start();
                        //await GenerateTransaction.UpdateAndCreateTransactions();
                        retryFlag = false;
                    }
                    else
                    {
                        new EventLogger(string.Format("Failed to connect to client API with reason \"{0}\"", responsePost.ReasonPhrase), Severity.Severe).Log();

                        //Send an email notification about the response
                        MailMessage emailMessage = new MailMessage
                        {
                            Subject = "Failed to sync Employee details",
                            Body = "Failed to sync Employee details from Client with error \"" + responsePost.ReasonPhrase + "\"",
                            To = { new MailAddress("gerling.kieren@gmail.com", "Flourish User") }
                        };

                        EmailHandler.SendMail(emailMessage);

                        switch (responsePost.StatusCode)
                        {
                            case HttpStatusCode.NotFound:
                                retryFlag = false;
                                break;
                        }
                    }
                }
            }

            new EventLogger("COMPLETED: Syncing Employee details from Client", Severity.Event).Log();
        }

        public static async Task InsertUpdateEmployeeDetails(List<Employee> employeeList)
        {
            new EventLogger("STARTED: Inserting/Updating Employee details from recieved Employee list", Severity.Event).Log();
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

            new EventLogger("COMPLETED: Inserting/Updating Employee details from recieved Employee list", Severity.Event).Log();
        }
    }
}
