using HRPayroll.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlourishAPI.DTOs
{
    public class FilteredReportDTO
    {
        public DateTime DateOfTransaction { get; set; }

        public string BUName { get; set; }

        public string TransactionId { get; set; }

        public decimal Amount { get; set; }

        public string EmployeeId { get; set; }

        public string EmployeeFullname { get; set; }


        public FilteredReportDTO(Transaction x)
        {
            DateOfTransaction = x.DateCreated;
            BUName = x.Employee.BusinessUnit.Name;
            TransactionId = x.Id.ToString();
            Amount = x.Amount;
            EmployeeId = x.EmployeeReference;
            EmployeeFullname = x.Employee.FirstName + " " + x.Employee.LastName;
        }

    }
}
