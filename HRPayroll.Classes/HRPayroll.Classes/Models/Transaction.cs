using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace HRPayroll.Classes.Models
{
    #region Enum
    public enum Status { Success = 1, Fail , Pending}
    #endregion
    public class Transaction : CRUDAble{
        #region Fields

        [BsonElement("TransactionId")]
        public int TransactionId { get; set; }

        [BsonElement("Amount")]
        public decimal Amount { get; set; }

        [BsonElement("Employee")]
        public Employee Employee { get; set; }

        [BsonElement("EmpReference")]
        public string EmployeeReference { get; set; }

        [BsonElement("Company")]
        public Company Company { get; set; }

        [BsonElement("CompReference")]
        public string CompanyReference { get; set; }

        [BsonElement("Status")]
        public Status Status { get; set; }

        [BsonElement("DateCreated")]
        public DateTime DateCreated { get; set; }
        #endregion

        #region Methods
        public override string createHash()
        {
            return string.Format("{0}-{1}-{2}-{3}-{4}-{5}", TransactionId,Employee.IdNumber,EmployeeReference,Company.Name,CompanyReference,Amount);
        }
        #endregion
    }
}
