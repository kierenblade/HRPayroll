using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FlourishAPI.Models.Archive
{
    class BusinessUnitArc
    {
        #region Fields
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("Name")] public string Name { get; set; }

        [BsonElement("ArchiveDate")]
        public DateTime ArchiveDate { get; set; }
        #endregion
    }
}
