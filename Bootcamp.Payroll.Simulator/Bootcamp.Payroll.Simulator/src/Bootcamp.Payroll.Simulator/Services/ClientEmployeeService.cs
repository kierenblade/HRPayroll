using Bootcamp.Payroll.Simulator.Classes;
using Bootcamp.Payroll.Simulator.ClientClasses;
using Bootcamp.Payroll.Simulator.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bootcamp.Payroll.Simulator.Services
{
    public class ClientEmployeeService
    {
        private IOptions<SimAppSettings> appSettings;
        private ILogger logger;
        private DBService dbService;
        public ClientEmployeeService(ILoggerFactory loggerFactory, IOptions<SimAppSettings> appSettings)
        {
            this.logger = loggerFactory.CreateLogger<BankAccountService>();
            this.appSettings = appSettings;
            this.dbService = new DBService(loggerFactory, appSettings);
        }

        public ClientEmployee GetEmployeeByIDNum(string ID)
        {
            var employees = this.dbService.getEmployees($" WHERE IDNumber =  '{ID}'");
            if (employees.Count <= 0)
                throw new ExceptionLogger(this.logger, $"No employee could be found with the ID Number: {ID}");

            return employees[0];
        }
        //public List<ClientEmployee> GetEmployeesByIDNums(List<string> IDs)
        //{
        //    List<ClientEmployee> allEmps = new List<ClientEmployee>();
        //}

        //private List<string> getWhereClauses(List<string> IDs)
        //{
        //    List<string> allClauses = new List<string>();

        //}
    }
}
