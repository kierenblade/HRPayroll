using HRPayroll.Classes.Models;
using HRPayroll.Audit;
using System;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace FlorishTestEnviroment
{
    class Program
    {
        static void Main(string[] args)
        {
            //Employee e = new Employee() { FirstName = "Wde", LastName = "Martin", AccountNumber = "10101012", Bank = new Bank() { Name = "Absa" } };
            //e.InsertDocument();
            //Company c = new Company() { CompanyId = 1, Name = "Sybrin", AccountNumber = "1010101010101001", Bank = new Bank() { Name = "Absa" } };
            //bool outcome = c.InsertDocument();
            //c.Name = "Florish";
            //c.Bank.Name = "FNB";
            //e.LastName = "VH2";
            //e.Bank.Name = "FNB";
            //e.AccountNumber = "10101013";
            //List<Employee> updateList = new Employee().GetAllEmployees();
            //new LoginDetails() { Username = "admin", Hash = "admin" , Role = new Role() { Name = "administrator" } , Company = new Company() {Name = "Sybrin" } }.InsertDocument();
            //new LoginDetails() { Username = "Tshief", Hash = "Poi" , Role = new Role() { Name="administrator"} , Company = new Company() { Name = "Sybrin" } }.InsertDocument();

            new Transaction() { Company = new Company() { Name = "Sybrin" }, Amount = 12000, Employee = new Employee() { IdNumber = "9509045284081", FirstName = "Wade", AccountNumber = "10101125" } }.InsertDocument();
            new Transaction() { Company = new Company() { Name = "Sybrin" }, Amount = 12000, Employee = new Employee() { IdNumber = "9509042284081", FirstName = "Jean", AccountNumber = "101011125" } }.InsertDocument();
            new Transaction() { Company = new Company() { Name = "Flourish" }, Amount = 12000, Employee = new Employee() { IdNumber = "9509042284081", FirstName = "tshief", AccountNumber = "101125" } }.InsertDocument();
            //updateList.UpdateManyDocument();
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
