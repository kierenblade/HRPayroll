using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HRPayroll.Archive
{
    #region Enums
    public enum PayFrequency { Weekly = 1, Monthly }
    public enum EmployeeStatus { Ghost = 1, Duplicate, Fired }
    #endregion

    public class EmployeeArc
    {
        #region Fields
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("IdNumber")]
        public string IdNumber { get; set; }

        [BsonElement("FirstName")]
        public string FirstName { get; set; }

        [BsonElement("LastName")]
        public string LastName { get; set; }

        [BsonElement("AccountNumber")]
        public string AccountNumber { get; set; }

        [BsonElement("Bank")]
        public BankArc Bank { get; set; }

        [BsonElement("BusinessUnit")]
        public string BusinessUnitName { get; set; }

        [BsonElement("Position")]
        public string Position { get; set; }

        [BsonElement("Salary")]
        public decimal PropertyName { get; set; }

        [BsonElement("PayFrequency")]
        public PayFrequency PayFrequency { get; set; }

        [BsonElement("PayDate")]
        public DateTime PayDate { get; set; }

        [BsonElement("PaymentType")]
        public PaymentTypeArc PaymentType { get; set; }

        [BsonElement("EmployeeStatus")]
        public EmployeeStatus EmployeeStatus { get; set; }

        [BsonElement("SyncDate")]
        public DateTime SyncDate { get; set; }

        [BsonElement("ArchiveDate")]
        public DateTime ArchiveDate { get; set; }
        #endregion
    }
}
