using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlourishAPI.DTOs;
using FlourishAPI.Models;
using FlourishAPI.Models.Classes;

namespace FlourishAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Report")]
    public class ReportController : Controller
    {

        // GET api/Report/SalsPerBU
        [HttpGet("TotSalsPerMonth/{id}")]
        public IEnumerable<DashboardDataDTO> SalariesPerMonth(string id,[FromBody] SuccesfullLoginDTO logCred) {

           if(!new TokenAuthentication().VerifyToken(logCred)){
                return null;
            }

            Transaction t = new Transaction() { Employee = new Employee(), Company = new Company() };
            List<Transaction> outgoingTransactions = new List<Transaction>();
            t.InsertDocument();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Company.Name", id);
            List<CRUDAble> w = t.SearchDocument(parameters);
            w.Remove(t);
            t.Delete();


            foreach (Transaction item in w)
            {
                outgoingTransactions.Add(item);
            }

            Dictionary<string, MonthAndYear> totlSalariesPerMonth = new Dictionary<string, MonthAndYear>();

            foreach (var item in outgoingTransactions)
            {
                MonthAndYear monthAndYear = new MonthAndYear() { MonthInt = item.DateCreated.Month, Month = item.DateCreated.ToString("MMM"), Year = item.DateCreated.Year , Amount = item.Amount};

                string monthAndYearString = monthAndYear.Month + "-" + monthAndYear.Year;
                if (totlSalariesPerMonth.Keys.Contains(monthAndYearString))
                {
                    totlSalariesPerMonth[monthAndYearString].Amount += item.Amount;
                }
                else
                {
                    totlSalariesPerMonth.Add(monthAndYearString, monthAndYear);
                }

            }

            

            List<DashboardDataDTO> outGoingData = new List<DashboardDataDTO>();

            for (int i = 1; i < 13; i++)
            {
                foreach (var item in totlSalariesPerMonth.Values)
                {
                    if (item.MonthInt == i)
                    {
                        outGoingData.Add(new DashboardDataDTO() { FieldName = item.Month + " - " + item.Year, Value = item.Amount });
                        break;
                    }

                }
 
            }

            

            return outGoingData;

        }

        
        [HttpGet("SalariesPerBU/{id}")]
        public IEnumerable<DashboardDataDTO> SalariesPerBU(string id)
        {
            

            Transaction t = new Transaction() { Employee = new Employee(), Company = new Company() };
            List<Transaction> outgoingTransactions = new List<Transaction>();
            t.InsertDocument();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Company.Name", id);
            List<CRUDAble> w = t.SearchDocument(parameters);
            w.Remove(t);
            t.Delete();

            foreach (Transaction item in w)
            {
                outgoingTransactions.Add(item);
            }

            Dictionary<string, decimal> totlSalariesPerBU = new Dictionary<string, decimal>();

            foreach (var item in outgoingTransactions)
            {
                if (totlSalariesPerBU.Keys.Contains(item.Employee.BusinessUnit.Name))
                {
                    totlSalariesPerBU[item.Employee.BusinessUnit.Name] += item.Amount;
                }
                else
                {
                    totlSalariesPerBU.Add(item.Employee.BusinessUnit.Name, item.Amount);
                }
            }

            List<DashboardDataDTO> outGoingData = new List<DashboardDataDTO>();

            foreach (var item in totlSalariesPerBU.Keys)
            {
                outGoingData.Add(new DashboardDataDTO() { FieldName = item, Value = totlSalariesPerBU[item] });
            }

          

            return outGoingData;

        }

        [HttpGet("TotalTransactions/{id}")]
        public int TotalTransactions(string id) {

            Transaction t = new Transaction() { Employee = new Employee(), Company = new Company() };
            List<Transaction> outgoingTransactions = new List<Transaction>();
            t.InsertDocument();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Company.Name", id);
            List<CRUDAble> w = t.SearchDocument(parameters);
            w.Remove(t);
            t.Delete();

            return w.Count;


        }

        [HttpGet("SumOfTransactions/{id}")]
        public decimal TotalSumOfAllTransactions(string id)
        {

            decimal SumOfTransactions = 0;

            Transaction t = new Transaction() { Employee = new Employee(), Company = new Company() };
            List<Transaction> outgoingTransactions = new List<Transaction>();
            t.InsertDocument();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Company.Name", id);
            List<CRUDAble> w = t.SearchDocument(parameters);
            w.Remove(t);
            t.Delete();

            foreach (Transaction item in w)
            {
                SumOfTransactions += item.Amount;
            }

            return SumOfTransactions;


        }

        [HttpGet("GetNotifications/{id}")]
        public IEnumerable<DesktopNotification> GetNotifications(string id)//Wade Please Review this Code, Will this work?
        {

            DesktopNotification t = new DesktopNotification() { };
            t.InsertDocument();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Company.Name", id);
            List<CRUDAble> notes = t.SearchDocument(parameters);
            notes.Remove(t);

            List<DesktopNotification> notifications = new List<DesktopNotification>();

          
            foreach (DesktopNotification item in notes)
            {
                notifications.Add(item);
            }

            return notifications;
        }


        // POST api/Report/GenReportRequest
        [HttpPost("GenReportRequest")]
        public IEnumerable<FilteredReportDTO> GeneralReportRequest([FromBody] GeneralReportRequestDTO reportRequestDetails)
        {
            


            Transaction t = new Transaction() { Employee = new Employee(),Company = new Company()};
            List<Transaction> outgoingTransactions = new List<Transaction>();
            t.InsertDocument();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Company.Name", reportRequestDetails.Company);
            List<CRUDAble> w = t.SearchDocument(parameters);
            w.Remove(t);
            t.Delete();

            foreach (Transaction item in w)
            {
                outgoingTransactions.Add(item);
            }
            // Beginning of Filtering
            IEnumerable<Transaction> filter = outgoingTransactions;

            if (reportRequestDetails.StartDate != null)
            {

                filter = filter.AsQueryable().Where(x => x.DateCreated >= reportRequestDetails.StartDate).ToList(); 
            }

            if (reportRequestDetails.EndDate != null)
            {

                filter = filter.AsQueryable().Where(x => x.DateCreated <= reportRequestDetails.EndDate).ToList(); ;
            }

            if (reportRequestDetails.StartAmount != 0)
            {

                filter = filter.AsQueryable().Where(x => x.Amount >= reportRequestDetails.StartAmount).ToList(); ;
            }

            if (reportRequestDetails.EndAmount != 0)
            {
                filter = filter.AsQueryable().Where(x => x.Amount <= reportRequestDetails.EndAmount).ToList(); ;
            }

            List<Transaction> withBUs = new List<Transaction>();

            if (reportRequestDetails.BU.Length != 0)
            {
                IEnumerable<Transaction> temp = new List<Transaction>();
                foreach (var item in reportRequestDetails.BU)
                {
                    temp = filter.AsQueryable().Where(x => x.Employee.BusinessUnit.Name == item).ToList();

                    foreach (Transaction instant in temp)
                    {
                        withBUs.Add(instant);
                    }
                }

                filter = withBUs;
            }

            if (reportRequestDetails.EmployeeID != null)
            {
                filter = filter.AsQueryable().Where(x => x.EmployeeReference == reportRequestDetails.EmployeeID).ToList(); ;
            }

            List<FilteredReportDTO> outGoingData = new List<FilteredReportDTO>();

            if (reportRequestDetails.EmployeeID != null)
            {
                filter = outgoingTransactions.AsQueryable().Where(x => x.EmployeeReference == reportRequestDetails.EmployeeID).ToList();
            }

            foreach (var item in filter)
            {
                outGoingData.Add(new FilteredReportDTO(item));
            }

            //FilteredReportDTO totalSumAmount = new FilteredReportDTO(new Transaction() { EmployeeReference = "TotalAmount", Employee = new Employee() });

            //foreach (var item in filter)
            //{
            //    totalSumAmount.Amount += item.Amount;
            //}
            //outGoingData.Add(totalSumAmount); //The last record of this List is the sum Of the Amounts of all the filtered Records


           
            return outGoingData;
        }





    }
}