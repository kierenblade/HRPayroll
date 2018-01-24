using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HRPayroll.Classes.Models
{
    class BusinessUnit
    {
        #region Fields
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("Name")] public string Name { get; set; }
        #endregion
    }
}
