using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HRPayroll.Classes.Models
{
    public class LoginDetails : CRUDAble
    {
        #region Fields


        [BsonElement("Username")]
        public string Username { get; set; }

        [BsonElement("Hash")]
        public string Hash { get; set; }

        [BsonElement("Role")]
        public Role Role { get; set; }

        [BsonElement("Company")]
        public Company Company { get; set; }

        [BsonElement("LastLogin")]
        public DateTime LastLogin { get; set; }
        #endregion

        #region Methods

        public override string createHash()
        {
            return string.Format("{0}-{1}-{2}-{3}", Username,Hash,Role.Name,Company.Name);
        }
        #endregion
    }
}
