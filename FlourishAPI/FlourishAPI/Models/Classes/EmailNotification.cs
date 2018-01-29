using System;
using MongoDB.Bson.Serialization.Attributes;

namespace FlourishAPI.Models.Classes
{
    public class EmailNotification : CRUDAble
    {
        #region Fields

        [BsonElement("ContactDetails")]
        public ContactDetails ContactDetails { get; set; }

        [BsonElement("TimeStamp")]
        public DateTime TimeStamp { get; set; }

        [BsonElement("Message")]
        public string Message { get; set; }

        [BsonElement("Logs")] public string Logs { get; set; }
        #endregion

        #region Methods
        public override string createHash()
        {
            return string.Format("{0}-{1}-{2}-{3}", ContactDetails,TimeStamp,Message,Logs);
        }
        #endregion
    }
}
