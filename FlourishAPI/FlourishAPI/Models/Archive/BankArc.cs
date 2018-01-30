using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FlourishAPI.Models.Archive
{
    public class BankArc
    {
        #region Fields
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("BankId")]
        public int BankId { get; set; }

        [BsonElement("BankName")]
        public string Name { get; set; }

        [BsonElement("ArchiveDate")]
        public DateTime ArchiveDate { get; set; }
        #endregion
    }
}
