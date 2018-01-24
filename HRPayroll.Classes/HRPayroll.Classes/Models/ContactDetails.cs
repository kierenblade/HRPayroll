using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HRPayroll.Classes.Models
{
    public class ContactDetails
    {
        #region Fields
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("ContactName")]
        public string ContactName { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("Cell")]
        public string Cell { get; set; }
        #endregion
    }
}
