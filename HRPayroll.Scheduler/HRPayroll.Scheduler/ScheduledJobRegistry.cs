﻿using FluentScheduler;

namespace HRPayroll.Scheduler
{
    public class ScheduledJobRegistry : Registry
    {
        public void InitializeJobs()
        {
            JobManager.Initialize(new ScheduledJobRegistry());
        }

        //Register new jobs
        public ScheduledJobRegistry()
        {
            //Create new schedule for job
            Schedule<EmployeeSync>()
                .NonReentrant()     //Only one instance of the job can run at a time
                .ToRunNow()         //Run the job immediately 
                .AndEvery(5)        //Run the job every minute
                .Seconds();

            //==================
            //Add more jobs here
            //==================
        }
    }
}
