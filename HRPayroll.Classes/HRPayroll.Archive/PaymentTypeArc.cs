using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HRPayroll.Archive
{
    public class PaymentTypeArc
    {
        #region Fields
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("ArchiveDate")]
        public DateTime ArchiveDate { get; set; }
        #endregion
    }
}
