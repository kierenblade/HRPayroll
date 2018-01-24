using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using HRPayroll.Classes.Models;

namespace HRPayroll.Audit
{
    public class CompanyAud : CRUDAble
    {
        #region Fields

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("AccountNumber")]
        public string AccountNumber { get; set; }

        [BsonElement("ChangeBy")]
        public string ChangeBy { get; set; }

        [BsonElement("ChangeTime")]
        public DateTime ChangeTime { get; set; }

        [BsonElement("ChangeLog")]
        public List<ChangeLog> ChangeLog { get; set; }
        #endregion
    }
}
