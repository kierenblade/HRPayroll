using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HRPayroll.Archive
{
    public class LoginDetails
    {
        #region Fields
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("Username")]
        public string Username { get; set; }

        [BsonElement("Hash")]
        public string Hash { get; set; }

        [BsonElement("Role")]
        public RoleArc Role { get; set; }

        [BsonElement("Company")]
        public CompanyArc Company { get; set; }

        [BsonElement("LastLogin")]
        public DateTime LastLogin { get; set; }

        [BsonElement("ArchiveDate")]
        public DateTime ArchiveDate { get; set; }
        #endregion
    }
}
