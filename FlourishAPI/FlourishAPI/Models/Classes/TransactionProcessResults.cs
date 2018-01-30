using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlourishAPI.Models.Classes
{

    public class TransactionProcessResult
    {
        public ObjectId TransactionId { get; set; }
        public string FailReason { get; set; }
        public StatusCode Code { get; set; }
    }

    public enum StatusCode
    {
        Network = 1,
        Other,
        Success
    }
}
