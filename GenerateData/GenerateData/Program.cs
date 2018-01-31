using HRPayroll.Classes.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GenerateData
{
    class Program
    {

        static List<Employee> employees;
        static List<Company> companies;
        static List<Bank> banks;

        static void Main(string[] args)
        {
            ReadJson();
            foreach (var item in employees)
            {
                int r = new Random().Next(0, banks.Count);
                Bank b = banks[r];
                item.Bank.BankId = b.BankId;
                item.Bank.Name = b.Name;

                item.Company = companies[new Random().Next(0, companies.Count)];
                if (item.PayFrequency == PayFrequency.Weekly)
                {
                    item.PayDate = new Random().Next(1, 8);
                }
                else
                {
                    item.PayDate = new Random().Next(1, 28);
                }

                item.BusinessUnit = item.Company.BusinessUnits[new Random().Next(0, item.Company.BusinessUnits.Count)];
            }

            foreach (var item in companies)
            {
                int r = new Random().Next(0, banks.Count);
                Bank b = banks[r];
                item.Bank.BankId = b.BankId;
                item.Bank.Name = b.Name;
            }
            
            using (StreamWriter file = File.CreateText("empOut.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, employees);
            }

            using (StreamWriter file = File.CreateText("compOut.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, companies);
            }

            using (StreamWriter file = File.CreateText("banksOut.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, banks);
            }

            Console.ReadKey();
        }


        static void ReadJson()
        {
            using (StreamReader r = new StreamReader("employees.json"))
            {
                string json = r.ReadToEnd();
                employees =  JsonConvert.DeserializeObject<List<Employee>>(json);
            }

            using (StreamReader r = new StreamReader("companies.json"))
            {
                string json = r.ReadToEnd();
                companies = JsonConvert.DeserializeObject<List<Company>>(json);
            }

            using (StreamReader r = new StreamReader("banks.json"))
            {
                string json = r.ReadToEnd();
                banks = JsonConvert.DeserializeObject<List<Bank>>(json);
            }
        }
    }
}
