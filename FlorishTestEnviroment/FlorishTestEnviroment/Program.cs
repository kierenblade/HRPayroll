using HRPayroll.Classes.Models;
using HRPayroll.Audit;
using System;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Collections.Generic;

namespace FlorishTestEnviroment
{
    class Program
    {
        static void Main(string[] args)
        {
            //Employee e = new Employee() { FirstName = "Jason", LastName = "VH", AccountNumber = "10101012",Bank = new Bank() { Name="Absa"} };
            //e.InsertDocument();
            //e.LastName = "VH2";
            //e.Bank.Name = "FNB";
            //e.AccountNumber = "10101013";
            //List<CRUDAble> updateList = new List<CRUDAble>();
            //updateList.Add(e);
            //updateList.UpdateManyDocument();
            Dictionary<string, string> searchFieldsAndCols = new Dictionary<string, string>();
            searchFieldsAndCols.Add("FirstName", "Jason");
            new Employee() { }.SearchDocument(searchFieldsAndCols);
            Console.WriteLine("Completed succesfully");
            Console.ReadLine();
        }
    }
}
