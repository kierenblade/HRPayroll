﻿using System;
using System.Collections.Generic;
using FlourishAPI.Models.Classes;
using MongoDB.Bson.Serialization.Attributes;

namespace FlourishAPI.Models.Audit
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

        #region Methods
        public override string createHash()
        {
            return string.Format("{0}-{1}-{2}-{3}-{4}", Name,AccountNumber,ChangeBy,ChangeTime,ChangeLog);
        }
        #endregion
    }
}
