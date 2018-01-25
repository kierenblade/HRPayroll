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
            Employee e = new Employee() { FirstName = "Jaon", LastName = "VH", AccountNumber = "10101012", Bank = new Bank() { Name = "Absa" } };
            bool outcome = e.InsertDocument();
            
            //e.LastName = "VH2";
            //e.Bank.Name = "FNB";
            //e.AccountNumber = "10101013";
            //List<CRUDAble> updateList = new List<CRUDAble>();
            //updateList.Add(e);
            //updateList.UpdateManyDocument();
            //Dictionary<string, string> searchFieldsAndCols = new Dictionary<string, string>();
            //searchFieldsAndCols.Add("FirstName", "Jason");
            //new Employee() { }.SearchDocument(searchFieldsAndCols);
            if (outcome)
            {
                Console.WriteLine("Completed succesfully");
            }
            else
            {
                Console.WriteLine("Completed unsuccesfully");
            }
            Console.ReadLine();
        }
    }
}
