using System;
using System.Collections.Generic;
using System.Text;
using FluentScheduler;

namespace HRPayroll.Scheduler
{
    class EmployeeSync : IJob
    {
        public EmployeeSync()
        {
            
        }
        public void Execute()
        {
            Console.WriteLine("Sync started");
        }

        public void SyncEmployeesFromClient()
        {

        }
    }
}
