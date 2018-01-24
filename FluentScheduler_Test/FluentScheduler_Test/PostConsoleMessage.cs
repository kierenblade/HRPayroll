using System;
using FluentScheduler;

namespace FluentScheduler_Demo
{
    public class PostConsoleMessage : IJob
    {
        public PostConsoleMessage(){}

        public void Execute()
        {
            //Execute scheduled task
            Console.WriteLine("The time is {0:HH:mm:ss}", DateTime.Now);
        }
    }
}
