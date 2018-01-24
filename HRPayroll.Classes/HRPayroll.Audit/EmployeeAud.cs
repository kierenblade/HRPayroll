using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using HRPayroll.Classes.Models;

namespace HRPayroll.Audit
{
    public class EmployeeAud : CRUDAble
    {
        #region Fields
        //[BsonId]
        //public ObjectId Id { get; set; }

        [BsonElement("EmployeeId")]
        public int EmployeeId { get; set; }

        [BsonElement("CitizenId")]
        public string CitizenId { get; set; }

        [BsonElement("FirstName")]
        public string FirstName { get; set; }

        [BsonElement("LastName")]
        public string LastName { get; set; }

        [BsonElement("ChangeBy")]
        public string ChangeBy { get; set; }

        [BsonElement("ChangeTime")]
        public DateTime ChangeTime { get; set; }
        
        [BsonElement("ChangeLog")]
        public List<ChangeLog> ChangeLog { get; set; }
        #endregion
    }
}
