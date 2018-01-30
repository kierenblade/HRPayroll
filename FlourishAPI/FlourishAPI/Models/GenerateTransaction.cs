using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using FlourishAPI.Models.Classes;
using FluentScheduler;

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

            Employee e = new Employee() { Company = new Company(), BusinessUnit = new BusinessUnit() };
            e.InsertDocument();

            //Split employee list into Weekly and Monthly payments
            List<Employee> employees = e.GetAllEmployees();
            employees.Remove(e);

            List<Employee> weeklyEmployees = employees.Where(x => x.PayFrequency == PayFrequency.Weekly).ToList();
            List<Employee> monthlyEmployees = employees.Where(x => x.PayFrequency == PayFrequency.Monthly).ToList();

            //Filter lists on who is getting paid today
            weeklyEmployees = weeklyEmployees.Where(x => x.PayDate == (int)DateTime.Now.DayOfWeek).ToList();
            monthlyEmployees = monthlyEmployees.Where(x => x.PayDate == DateTime.Now.Day).ToList();

            //Generate weekly transactions
            GenerateTransactionFromEmployeeList(weeklyEmployees);
            GenerateTransactionFromEmployeeList(monthlyEmployees);

            e.Delete();
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

            GenerateTransactionFromEmployeeList(existingEmployees);

            e.Delete();

        }

        private static void GenerateTransactionFromEmployeeList(List<Employee> existingEmployees)
        {
            new EventLogger("STARTED: Generation of Transactions from recieved Employee list", Severity.Event).Log();

            Transaction t = new Transaction() { Employee = new Employee(), Company = new Company() };
            t.InsertDocument();

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
                    new Transaction()
                    {
                        Employee = item,
                        Company = item.Company,
                        Amount = item.Salary,
                        DateCreated = DateTime.Now,
                        Status = Status.Pending
                    }.InsertDocument();
                }
            }

            t.Delete();
            new EventLogger("COMPLETED: Generation of Transactions from recieved Employee list", Severity.Event).Log();
        }
    }
}
