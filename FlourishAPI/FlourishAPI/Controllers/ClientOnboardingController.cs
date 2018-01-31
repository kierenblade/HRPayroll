using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlourishAPI.Models.Classes;
using FlourishAPI.Models;
using Newtonsoft.Json;

namespace FlourishAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/ClientOnboarding")]
    public class ClientOnboardingController : Controller
    {
        [HttpPost]
        public bool CompanyOnboarding([FromBody] string obj)
        {
            LoginDetails newCompany = JsonConvert.DeserializeObject<LoginDetails>(obj);
            newCompany.Role = new Role()
            {
                Name = "admin"
            };
            Company c = new Company()
            {
                Bank = new Bank()
            };
            c.InsertDocument();
            List<CRUDAble> clist = c.SearchDocument(new Dictionary<string, object>());
            clist.Remove(c);
            c.Delete();
            int companyID = 0;
            List<Company> comp = new List<Company>();
            foreach (Company item in clist)
            {
                comp.Add(item);
            }
            for (int i = 0; i < 1000; i++)
            {
                if (comp.AsQueryable().Where(p => p.CompanyId == i).LongCount() < 1)
                {
                    companyID = i;
                    break;
                }
            }
            newCompany.Company.CompanyId = companyID;
            newCompany.InsertDocument();
            newCompany.Company.InsertDocument();
            return true;
        }
    }
}