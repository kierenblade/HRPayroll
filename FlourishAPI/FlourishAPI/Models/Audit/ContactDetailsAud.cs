using System;
using System.Collections.Generic;
using FlourishAPI.Models.Classes;
using MongoDB.Bson.Serialization.Attributes;

namespace FlourishAPI.Models.Audit
{
    public class ContactDetailsAud : CRUDAble
    {
        #region Fields

        [BsonElement("Company")]
        public Company Company { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("ChangeBy")]
        public string ChangeBy { get; set; }

        [BsonElement("ChangeTime")]
        public DateTime ChangeTime { get; set; }

        [BsonElement("ChangeLog")] public List<ChangeLog> ChangeLog { get; set; }

        #endregion

        #region Methods
        public override string createHash()
        {
            return string.Format("{0}-{1}-{2}-{3}-{4}-{5}", Name,Company.Name,Email,ChangeBy,ChangeTime,ChangeLog);
        }
        #endregion
    }
}
