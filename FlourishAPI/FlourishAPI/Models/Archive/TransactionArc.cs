using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FlourishAPI.Models.Archive
{
    #region Enum
    public enum Status { Success = 1, Fail }
    #endregion
    public class TransactionArc
    {
        #region Fields
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("TransactionId")]
        public int TransactionId { get; set; }

        [BsonElement("Amount")]
        public decimal Amount { get; set; }

        [BsonElement("Employee")]
        public EmployeeArc Employee { get; set; }

        [BsonElement("EmpReference")]
        public string EmployeeReference { get; set; }

        [BsonElement("Company")]
        public CompanyArc Company { get; set; }

        [BsonElement("CompReference")]
        public string CompanyReference { get; set; }

        [BsonElement("Status")]
        public Status Status { get; set; }

        [BsonElement("ArchiveDate")]
        public DateTime ArchiveDate { get; set; }
        #endregion
    }
}
