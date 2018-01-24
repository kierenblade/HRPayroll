using System;
using MongoDB.Bson.Serialization.Attributes;

namespace HRPayroll.Classes.Models
{
    #region Enums
    public enum PayFrequency { Weekly = 1, Monthly }
    public enum EmployeeStatus { Employed = 1, Ghost, Duplicate, Fired }
    #endregion

    public class Employee : CRUDAble{
        #region Fields
        //[BsonId]
        //public ObjectId Id { get; set; }

        [BsonElement("IdNumber")]
        public string IdNumber { get; set; }

        [BsonElement("FirstName")]
        public string FirstName { get; set; }

        [BsonElement("LastName")]
        public string LastName { get; set; }

        [BsonElement("AccountNumber")]
        public string AccountNumber { get; set; }

        [BsonElement("Bank")]
        public Bank Bank { get; set; }

        [BsonElement("BusinessUnit")]
        public string BusinessUnitName { get; set; }

        [BsonElement("Position")]
        public string Position { get; set; }

        [BsonElement("Salary")]
        public decimal Salary { get; set; }

        [BsonElement("PayFrequency")]
        public PayFrequency PayFrequency { get; set; }

        [BsonElement("PayDate")]
        public DateTime PayDate { get; set; }

        [BsonElement("PaymentType")]
        public PaymentType PaymentType { get; set; }

        [BsonElement("EmployeeStatus")]
        public EmployeeStatus EmployeeStatus { get; set; }

        [BsonElement("SyncDate")]
        public DateTime SyncDate { get; set; }
        #endregion
    }
}
