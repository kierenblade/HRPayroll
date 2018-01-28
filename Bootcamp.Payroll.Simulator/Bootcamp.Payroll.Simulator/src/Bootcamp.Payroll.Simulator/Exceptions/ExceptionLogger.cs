using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bootcamp.Payroll.Simulator.Exceptions
{

    /// <summary>
    /// Used to write an error to logs and then throw the error.
    /// </summary>
    public class ExceptionLogger : Exception
    {
        public ExceptionLogger(ILogger logger, string message) : base(message)
        {
            logger.LogError(message);
        }
        public ExceptionLogger(ILogger logger, string message, Exception innerException) : base(message, innerException)
        {
            logger.LogError(message);
        }
    }
}
