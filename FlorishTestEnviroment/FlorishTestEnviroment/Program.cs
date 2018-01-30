using HRPayroll.Classes.Models;
using HRPayroll.Audit;
using System;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace FlorishTestEnviroment
{
    class Program
    {
        static void Main(string[] args)
        {



          
            //using (StreamReader r = new StreamReader("empOut.json"))
            //{
            //    string json = r.ReadToEnd();
            //    List<Employee> employees = JsonConvert.DeserializeObject<List<Employee>>(json);
            //    foreach (Employee item in employees)
            //    {
            //        item.InsertDocument();
            //        Console.WriteLine(item.FirstName);
            //    }
            //}


            //Employee e = new Employee() { Company = new Company() , BusinessUnit = new BusinessUnit()};
            //Transaction t = new Transaction() { Employee = new Employee(), Company = new Company() };
            //e.InsertDocument();
            //t.InsertDocument();
            //List<CRUDAble> existingEmployees = e.SearchDocument(new Dictionary<string, object>());
            //List<Transaction> toUpdateTransactions = new List<Transaction>();
            //e.Delete();
            //existingEmployees.Remove(e);
            //foreach (Employee item in existingEmployees)
            //{
            //    Dictionary<string, object> filterList = new Dictionary<string, object>();
            //    filterList.Add("Employee.HashCode", item.HashCode);
            //    List<CRUDAble> result = t.SearchDocument(filterList);
            //    result.Remove(t);
            //    if (result.LongCount() > 0)
            //    {
            //        result.UpdateManyDocument();
            //    }
            //    else
            //    {
            //        new Transaction() { Employee = item, Company = item.Company, Amount = item.Salary, DateCreated = DateTime.Now, Status = Status.Pending }.InsertDocument();
            //    }
            //}


            //t.Delete();



            //using (StreamReader r = new StreamReader("compOut.json"))
            //{
            //    string json = r.ReadToEnd();
            //    List<Company> companies = JsonConvert.DeserializeObject<List<Company>>(json);

            //    foreach (Company item in companies)
            //    {
            //        new LoginDetails() { Company = item, Username = "Wade", Hash = "Wade", Role = new Role() { Name = "Human Relations Manager" } }.InsertDocument();
            //        item.InsertDocument();
            //        Console.WriteLine(item.Name);
            //    }
            //}

            //using (StreamReader r = new StreamReader("banksOut.json"))
            //{
            //    string json = r.ReadToEnd();
            //    List<Bank> banks = JsonConvert.DeserializeObject<List<Bank>>(json);

            //    foreach (Bank item in banks)
            //    {
            //        item.InsertDocument();
            //        Console.WriteLine(item.Name);
            //    }
            //}
            //Employee e = new Employee() { Company = new Company(), Bank = new Bank()};
            //e.InsertDocument();

            //List<CRUDAble> eList = e.SearchDocument(new Dictionary<string, object>());


            //Company c = new Company() { CompanyId = 1, Name = "Sybrin", AccountNumber = "1010101010101001", Bank = new Bank() { Name = "Absa" } };
            //bool outcome = c.InsertDocument();
            //c.Name = "Florish";
            //c.Bank.Name = "FNB";
            //e.LastName = "VH2";
            //e.Bank.Name = "FNB";
            //e.AccountNumber = "10101013";
            //List<CRUDAble> updateList = e.SearchDocument(new Dictionary<string, object>());


            //foreach (Employee item in updateList)
            //{
            //    item.FirstName = "Wade";
            //}

            //updateList.UpdateManyDocument();

            //e.Delete();
            //new LoginDetails() { Username = "admin", Hash = "admin" , Role = new Role() { Name = "administrator" } , Company = new Company() {Name = "Sybrin" } }.InsertDocument();
            //new LoginDetails() { Username = "Tshief", Hash = "Poi" , Role = new Role() { Name="administrator"} , Company = new Company() { Name = "Sybrin" } }.InsertDocument();

            //new Transaction() { Company = new Company() { Name = "Sybrin" }, Amount = 1000, Employee = new Employee() { IdNumber = "9509045284081", FirstName = "Wade", AccountNumber = "10101125" , BusinessUnitName = "CM"},DateCreated = new  DateTime(2018,4,12) }.InsertDocument();
            //new Transaction() { Company = new Company() { Name = "Sybrin" }, Amount = 5000, Employee = new Employee() { IdNumber = "9509042284081", FirstName = "Jean", AccountNumber = "101011125", BusinessUnitName = "Payments" }, DateCreated = new DateTime(2018, 4, 12) }.InsertDocument();
            //new Transaction() { Company = new Company() { Name = "Sybrin" }, Amount = 7000, Employee = new Employee() { IdNumber = "9509042284081", FirstName = "Jason", AccountNumber = "101011125", BusinessUnitName = "Payments" }, DateCreated = new DateTime(2018, 2, 12) }.InsertDocument();
            //new Transaction() { Company = new Company() { Name = "Flourish" }, Amount = 12000, Employee = new Employee() { IdNumber = "9509042284081", FirstName = "tshief", AccountNumber = "101125" }, DateCreated = new DateTime(2018, 1, 12) }.InsertDocument();

            //Dictionary<string, object> searchFieldsAndCols = new Dictionary<string, object>();
            ////searchFieldsAndCols.Add("FirstName", "Jason");
            ////searchFieldsAndCols.Add("Bank.BankName", "Absa");
            //List<CRUDAble> eList = c.SearchDocument(searchFieldsAndCols);
            //foreach (Employee item in eList)
            //{
            //    Console.WriteLine(item.Id);
            //}
            //if (outcome)
            //{
            //    Console.WriteLine("Completed succesfully");
            //}
            //else
            //{
            //    Console.WriteLine("Completed unsuccesfully");
            //}
            Console.ReadLine();
        }
    }
}
