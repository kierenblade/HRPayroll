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
            Employee e = new Employee() { FirstName = "Wade", LastName = "Martin", AccountNumber = "10101012", Bank = new Bank() { Name = "Absa" } };
            e.InsertDocument();
            //Company c = new Company() { CompanyId = 1, Name = "Sybrin", AccountNumber = "1010101010101001", Bank = new Bank() { Name = "Absa" } };
            //bool outcome = c.InsertDocument();
            //c.Name = "Florish";
            //c.Bank.Name = "FNB";
            //e.LastName = "VH2";
            //e.Bank.Name = "FNB";
            //e.AccountNumber = "10101013";
            List<CRUDAble> updateList = e.SearchDocument(new Dictionary<string, object>());
            updateList.UpdateManyDocument();
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
