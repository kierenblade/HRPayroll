using System;
using MongoDB.Bson.Serialization.Attributes;

namespace HRPayroll.Classes.Models
{
    #region Enums
    public enum PayFrequency { Weekly = 1, Monthly }
    public enum EmployeeStatus { Employed = 1, Ghost, Duplicate, Fired }

    public enum PaymentType { ABSA = 1, VISA }
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
        public BusinessUnit BusinessUnit { get; set; }

        [BsonElement("Position")]
        public string Position { get; set; }

        [BsonElement("Salary")]
        public decimal Salary { get; set; }

        [BsonElement]
        public string CardNumber { get; set; }

        [BsonElement("Company")]
        public Company Company { get; set; }

        [BsonElement("PayFrequency")]
        public PayFrequency PayFrequency { get; set; }

        [BsonElement("PayDate")]
        public int PayDate { get; set; }

        [BsonElement("PaymentType")]
        public PaymentType PaymentType { get; set; }

        [BsonElement("EmployeeStatus")]
        public EmployeeStatus EmployeeStatus { get; set; }

        [BsonElement("SyncDate")]
        public DateTime SyncDate { get; set; }


        #endregion

        #region Methods


        public override string createHash()
        {
            return string.Format("{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}", IdNumber, FirstName, LastName, AccountNumber, BusinessUnit.Name, Salary, Position, EmployeeStatus);
        }
        #endregion
    }
}
