using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using HRPayroll.Classes.Models;

namespace HRPayroll.Audit
{
    public class LoginDetailsAud : CRUDAble
    {
        #region Fields

        [BsonElement("Username")]
        public string Username { get; set; }

        [BsonElement("Hash")]
        public string Hash { get; set; }

        [BsonElement("Role")]
        public Role Role { get; set; }

        [BsonElement("ChangeBy")]
        public string ChangeBy { get; set; }

        [BsonElement("ChangeTime")]
        public DateTime ChangeTime { get; set; }

        [BsonElement("ChangeLog")]
        public List<ChangeLog> ChangeLog { get; set; }
        #endregion
    }
}
