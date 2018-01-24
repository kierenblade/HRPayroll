using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HRPayroll.Archive
{
    public class ContactDetailsArc
    {
        #region Fields
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("ContactName")]
        public string ContactName { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("Cell")]
        public string Cell { get; set; }

        [BsonElement("ArchiveDate")]
        public DateTime ArchiveDate { get; set; }
        #endregion
    }
}
