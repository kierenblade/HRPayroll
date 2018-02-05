using FlourishAPI.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlourishAPI.DTOs
{
    public class NotificationsDTO
    {
        public string Message { get; set; }

        public DateTime DateCreated { get; set; }

        public NotificationsDTO(DesktopNotification x)
        {
            Message = x.Message;
            DateCreated = x.CreationDate;
        }
    }
}
