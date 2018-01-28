using Bootcamp.Payroll.Simulator.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bootcamp.Payroll.Simulator.ClientClasses
{
    public class ClientEmployee
    {
        public string ID { get; set; }
        public string IdNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountNumber { get; set; }
        public ClientBank Bank { get; set; } 
        public string BusinessUnit { get; set; }
        public string Position { get; set; }
        public double Salary { get; set; }
        public string CardNumber { get; set; }
        public PayFrequency PayFrequency { get; set; }
        public int PayDate { get; set; }
        public ClientPaymentType PaymentType { get; set; }
        public EmployeeStatus EmployeeStatus { get; set; }
        public DateTime SyncDate { get; set; }


    }
}
