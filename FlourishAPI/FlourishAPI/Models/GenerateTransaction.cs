using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using FlourishAPI.Models.Classes;
using FlourishAPI.Models.Scheduler;
using FluentScheduler;
using MongoDB.Bson;
using PaymentSwitchHandler;

namespace FlourishAPI.Models
{
    public class GenerateTransaction : IJob
    {
        public async void Execute()
        {
            await GenerateTransactionsForToday();
        }
        public static async Task GenerateTransactionsForToday()
        {
            new EventLogger(string.Format("STARTED: Generation of transactions for \"{0}\"", DateTime.Now),
                Severity.Event).Log();

            //Set the list of monthly and weekly payments for today
            List<Employee> weeklyEmployees = await GetEmployeesPaidToday(PayFrequency.Weekly);
            List<Employee> monthlyEmployees = await GetEmployeesPaidToday(PayFrequency.Monthly);

            //Send list of employees being paid today to client to verify that the employees get paid today
            await EmployeeSync.SyncEmployeeDetailsFromClient(weeklyEmployees, "http://172.18.12.209/api/ClientData/SyncEmployees4Today");
            await EmployeeSync.SyncEmployeeDetailsFromClient(monthlyEmployees, "http://172.18.12.209/api/ClientData/SyncEmployees4Today");

            //Get the updated records
            weeklyEmployees = await GetEmployeesPaidToday(PayFrequency.Weekly);
            monthlyEmployees = await GetEmployeesPaidToday(PayFrequency.Monthly);

            //Generate weekly/monthly transactions
            await GenerateTransactionFromEmployeeList(weeklyEmployees, true);
            await GenerateTransactionFromEmployeeList(monthlyEmployees, true);

            new EventLogger(string.Format("COMPLETED: Generation of transactions for \"{0}\"", DateTime.Now),
                Severity.Event).Log();
        }

        public static async Task UpdateAndCreateTransactions()
        {
            Employee e = new Employee() { Company = new Company(), BusinessUnit = new BusinessUnit() };

            e.InsertDocument();
            List<Employee> existingEmployees = e.GetAllEmployees();
            existingEmployees.Remove(e);
            List<Transaction> toUpdateTransactions = new List<Transaction>();

            await GenerateTransactionFromEmployeeList(existingEmployees, false);

            e.Delete();

        }

        private static async Task GenerateTransactionFromEmployeeList(List<Employee> existingEmployees, bool imediatePayment)
        {
            new EventLogger("STARTED: Generation of Transactions from recieved Employee list", Severity.Event).Log();

            Transaction t = new Transaction() { Employee = new Employee(), Company = new Company() };
            t.InsertDocument();

            List<Transaction> transactions = new List<Transaction>();

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
                    Transaction newTransaction = new Transaction()
                    {
                        Employee = item,
                        Company = item.Company,
                        Amount = item.Salary,
                        DateCreated = DateTime.Now,
                        Status = Status.Pending
                    };

                    newTransaction.InsertDocument();
                    transactions.Add(newTransaction);
                }
            }

            t.Delete();
            if (imediatePayment && existingEmployees.Count > 0)
            {
                await ProcessTransactions(transactions);
            }

            new EventLogger("COMPLETED: Generation of Transactions from recieved Employee list", Severity.Event).Log();
        }

        public static async Task<List<Employee>> GetEmployeesPaidToday(PayFrequency payFrequency)
        {
            Employee e = new Employee() { Company = new Company(), BusinessUnit = new BusinessUnit() };
            e.InsertDocument();

            //Get all employees from DB
            List<Employee> employees = e.GetAllEmployees();
            employees.Remove(e);
            e.Delete();

            List<Employee> employeesPaidToday = new List<Employee>();
            if (payFrequency == PayFrequency.Weekly)
            {
                //Filter employees that get paid weekly on this day
                employeesPaidToday = employees.Where(x => x.PayFrequency == PayFrequency.Weekly).ToList();
                employeesPaidToday = employeesPaidToday.Where(x => x.PayDate == (int)DateTime.Now.DayOfWeek).ToList();
            }
            else if (payFrequency == PayFrequency.Monthly)
            {
                //Filter employees that get paid monthly on this day
                employeesPaidToday = employees.Where(x => x.PayFrequency == PayFrequency.Monthly).ToList();
                employeesPaidToday = employeesPaidToday.Where(x => x.PayDate == DateTime.Now.Day).ToList();
            }

            //Filter lists on who is getting paid today
            return employeesPaidToday;
        }

        public static async Task ProcessTransactions(List<Transaction> transactionList)
        {
            //Push transactions to payment switch
            SwitchHandler switchHandler = new SwitchHandler("http://172.18.12.224");
            List<TransactionProcessResult> processResults = await switchHandler.ProcessTransaction(transactionList);

            foreach (TransactionProcessResult processResult in processResults)
            {
                if (processResult.Code == StatusCode.Success)
                {

                    foreach (Transaction crud in transactionList)
                    {
                        if (crud.Id == processResult.TransactionId)
                        {
                            crud.Status = Status.Success;
                            crud.InsertDocument("FlourishDB_ARC");
                            crud.Delete();
                        }
                    }
                }
            }
           new DesktopNotification() { Message = string.Format("Successful Payments: {0} <br/><br/> Failed Payments: <br/> Network Related: {1} <br/> Other: {2}", processResults.Count(x => x.Code == StatusCode.Success), processResults.Count(x => x.Code == StatusCode.Network), processResults.Count(x => x.Code == StatusCode.Other)), Company = transactionList.First().Company, LoginDetails = new LoginDetails() { Role = new Role(), Company = new Company() } }.InsertDocument();

            MailMessage mailMessage = new MailMessage
            {
                Subject = string.Format("Payment Overview for {0}", DateTime.Now.Date),
                Body = string.Format("Successful Payments: {0} <br/><br/> Failed Payments: <br/> Network Related: {1} <br/> Other: {2}", processResults.Count(x => x.Code == StatusCode.Success), processResults.Count(x => x.Code == StatusCode.Network), processResults.Count(x => x.Code == StatusCode.Other)),
                To = { new MailAddress("gerling.kieren@gmail.com", "Kieren Gerling") }
            };

            EmailHandler.SendMail(mailMessage);
        }
    }
}
