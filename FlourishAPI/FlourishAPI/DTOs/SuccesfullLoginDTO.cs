using FlourishAPI.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlourishAPI.DTOs
{
    public class SuccesfullLoginDTO
    {
        public bool Status { get; set; }

        //public string CompanyID { get; set; }

        public string CompanyName { get; set; }

        public string Username { get; set; }

        public string Hash { get; set; }




        public SuccesfullLoginDTO(LoginDetails x)
        {
            Status = true;
           // CompanyID = x.Company.Id.ToString();
            CompanyName = x.Company.Name;
            Username = x.Username;
            Hash = x.Hash;

        }

        public SuccesfullLoginDTO()
        {
            Status = false;
           

        }
    }
}
