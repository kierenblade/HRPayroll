﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FlourishAPI.DTOs;
using HRPayroll.Classes;
using Newtonsoft.Json;

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
        public Boolean Post([FromBody]string userDetails)
        {
            LoginDetails details =  JsonConvert.DeserializeObject<LoginDetails>(userDetails);

            SignInManager signIn = new SignInManager(details.Username, details.Password);


            return signIn.ValidateUserDetails();
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
