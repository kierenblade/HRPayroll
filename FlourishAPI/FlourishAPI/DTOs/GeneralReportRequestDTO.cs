using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlourishAPI.DTOs
{
    public class GeneralReportRequestDTO
    {
        public string Company { get; set; }
        public string EmployeeID { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal StartAmount { get; set; }

        public decimal EndAmount { get; set; }

        public string[] BU { get; set; }

        
    }
}
