using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FlourishAPI.Models.Classes
{
    public enum Severity
    {
        Severe = 1,
        Warning,
        Event
    }
    public class EventLogger
    {
        [BsonElement("Message")] private string Message { get; }

        [BsonElement("Timestamp")] private DateTime Timestamp { get; }

        [BsonElement("SeverityLevel")] private Severity Severity { get; }

        public EventLogger(string message, Severity severity)
        {
            Message = message;
            Timestamp = DateTime.Now;
            Severity = severity;
        }

        public void Log()
        {
            new DatabaseConnection().DatabaseConnect("FlourishDB_Events").GetCollection<EventLogger>("Logs").InsertOne(this);
        }
        
    }
}
