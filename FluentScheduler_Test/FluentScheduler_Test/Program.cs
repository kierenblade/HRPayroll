using System;
using FluentScheduler;

namespace FluentScheduler_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Start scheduler
            JobManager.Initialize(new ScheduledJobRegistry());
            
            Console.ReadLine();
        }
    }
}
