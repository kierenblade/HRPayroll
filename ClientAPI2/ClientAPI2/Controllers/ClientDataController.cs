﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlourishAPI.Models.Classes;
using FlourishAPI.Models;


namespace ClientAPI2.Controllers
{
    [Produces("application/json")]
    [Route("api/ClientData")]
    public class ClientDataController : Controller
    {
    

        [HttpGet("SyncAllEmployees")]

        public IEnumerable<Employee> SyncAllEmployees()
        {

            Employee e = new Employee();
            e.InsertDocument("ClientDB");
            List<Employee> emps = e.GetAllEmployees("ClientDB");
            emps.Remove(e);
            e.Delete("ClientDB");

            return emps;

        }

        [HttpPost("SyncEmployees")]
        public IEnumerable<Employee> SyncEmployees([FromBody] List<string> empIDs)
        {

            Employee e = new Employee();
            e.InsertDocument("ClientDB");
            List<Employee> emps = e.GetAllEmployees("ClientDB");
            emps.Remove(e);
            e.Delete("ClientDB");

            return emps.Where(x => x.EmployeeStatus == EmployeeStatus.Employed).ToList();

        }

        [HttpPost("SyncEmployees4Today")]
        public IEnumerable<Employee> GetEmployees4Today([FromBody] List<string> empIDs)
        {

            Employee e = new Employee();
            e.InsertDocument("ClientDB");
            List<Employee> emps = e.GetAllEmployees("ClientDB");
            emps.Remove(e);
            e.Delete("ClientDB");

            List<Employee> weekly = emps.Where(x => x.PayFrequency == PayFrequency.Weekly).ToList();
            List<Employee> monthly = emps.Where(x => x.PayFrequency == PayFrequency.Monthly).ToList();
            List<Employee> toSend = new List<Employee>();

            weekly = weekly.Where(x => x.PayDate == (int)DateTime.Now.DayOfWeek && x.EmployeeStatus == EmployeeStatus.Employed).ToList();
            monthly = monthly.Where(x => x.PayDate == DateTime.Now.Day && x.EmployeeStatus == EmployeeStatus.Employed).ToList();

            toSend = monthly;
            
            foreach (Employee item in weekly)
            {
                toSend.Add(item);
            }

            return toSend;

        }

    }
}