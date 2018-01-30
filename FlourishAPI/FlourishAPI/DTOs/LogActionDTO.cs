using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlourishAPI.DTOs
{
    public class LogActionDTO
    {
        #region Fields

        public string Message { get; set; }

        public string Origin { get; set; }

        public string DoneBy { get; set; }

        public DateTime LogTime { get; set; }

        public string DoneByCompanyName { get; set; }
        #endregion

       

    }
}
