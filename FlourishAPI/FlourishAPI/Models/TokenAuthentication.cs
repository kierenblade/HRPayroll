using FlourishAPI.DTOs;
using FlourishAPI.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlourishAPI.Models
{
    public class TokenAuthentication
    {

        public bool VerifyToken(SuccesfullLoginDTO cred)
        {
            LoginDetails ld = new LoginDetails() { Company = new Company(), Role = new Role() };
            ld.InsertDocument();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            
            dic.Add("Username", cred.Username);
            dic.Add("Hash", cred.Hash);
            dic.Add("Company.Name", cred.CompanyName);
            List<CRUDAble> ldList = ld.SearchDocument(dic);
            if (ldList.LongCount() > 0 && cred.Status == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
