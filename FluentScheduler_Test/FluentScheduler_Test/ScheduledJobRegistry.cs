using FluentScheduler;

namespace FluentScheduler_Demo
{
    public class ScheduledJobRegistry : Registry
    {
        //Register new jobs
        public ScheduledJobRegistry()
        {
            //Create new schedule for job
            Schedule<PostConsoleMessage>()
                .NonReentrant()     //Only one instance of the job can run at a time
                .ToRunNow()         //Run the job immediately 
                .AndEvery(1)        //Run the job every minute
                .Minutes();

            //==================
            //Add more jobs here
            //==================
        }
    }
}
