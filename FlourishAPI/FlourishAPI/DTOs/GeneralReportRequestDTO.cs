using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlourishAPI.DTOs
{
    public class GeneralReportRequestDTO
    {
 
        public DateTime StartDate
        {
            get { return StartDate; }
            set { StartDate = value; }
        }

        public DateTime EndDate { get; set; }

        public string EmployeeID { get; set; }

        public decimal MinSalary { get; set; }

        public decimal MaxSalary { get; set; }

        public string JobTitle { get; set; }

    }
}
