﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FlourishAPI.DTOs;
using Newtonsoft.Json;
using System.Text;
using FlourishAPI.Models;
using FlourishAPI.Models.Classes;

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


           
            //SignInManager signIn = new SignInManager(userDetails.Username, userDetails.Password);

          
            LoginDetails verifyingAccount = new LoginDetails() { Username = userDetails.Username, Hash = userDetails.Password };

            verifyingAccount = verifyingAccount.verifyLoginDetails();

            if (verifyingAccount != null)
            {

                return true;
            }
            else
            {
                return false;
            }

            //SignInManager signIn = new SignInManager(details.Username, details.Password);

            //success = signIn.ValidateUserDetails();

            //return success;
        }



    }
}
