using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlourishAPI.DTOs
{
    public class LoginTokenDTO
    {
        public string Username { get; set; }
        public string Hash { get; set; }

        public string CompanyName { get; set; }

        public DateTime LastLogin { get; set; }

        public bool loggedIn { get; set; }
    }
}
