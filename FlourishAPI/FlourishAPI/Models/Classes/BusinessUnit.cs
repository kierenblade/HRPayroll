using MongoDB.Bson.Serialization.Attributes;

namespace FlourishAPI.Models.Classes
{
     public class BusinessUnit
    {
        #region Fields
        //[BsonId]
        //public ObjectId Id { get; set; }

        [BsonElement("Name")] public string Name { get; set; }
        #endregion
    }
}
