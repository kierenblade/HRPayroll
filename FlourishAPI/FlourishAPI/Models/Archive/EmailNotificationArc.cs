using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FlourishAPI.Models.Archive
{
    public class EmailNotification
    {
        #region Fields
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("ContactDetails")]
        public ContactDetailsArc ContactDetails { get; set; }

        [BsonElement("TimeStamp")]
        public DateTime TimeStamp { get; set; }

        [BsonElement("Message")]
        public string Message { get; set; }

        [BsonElement("Logs")]
        public string Logs { get; set; }

        [BsonElement("ArchiveDate")]
        public DateTime ArchiveDate { get; set; }
        #endregion
    }
}
