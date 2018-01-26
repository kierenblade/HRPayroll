using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FlourishAPI.DTOs;
using HRPayroll.Classes;
using Newtonsoft.Json;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlourishAPI.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        // GET: api/login
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "You are", "Connected" };
        }

        // GET api/login/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "Authenticate";
        }

        // POST api/login
        [HttpPost]
      
        public Boolean Post([FromBody]LoginDetailsDTO userDetails)
        {
            bool success = false;

            //LoginDetailsDTO details = JsonConvert.DeserializeObject<LoginDetailsDTO>(userDetails);

            SignInManager signIn = new SignInManager(userDetails.Username, userDetails.Password);

            success = signIn.ValidateUserDetails();

            return success;
        }



        // GET api/login/AlternativeGet
        //[HttpGet("AlternativeGet")] // <-- Specify your own Method name
        //public string AltGet(int id)
        //{
        //    return "Alternate Get";
        //}









        //// DELETE api/login/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
