using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FlourishAPI.Models.Archive
{
    public class CompanyArc
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("CompanyId")]
        public int CompanyId { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("AccountNumber")]
        public string AccountNumber { get; set; }

        [BsonElement("Bank")]
        public BankArc Bank { get; set; }

        [BsonElement("ArchiveDate")]
        public DateTime ArchiveDate { get; set; }
    }
}
