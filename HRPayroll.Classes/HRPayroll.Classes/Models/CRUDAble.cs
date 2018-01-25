using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HRPayroll.Classes.Models
{
    public abstract class CRUDAble
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement]
        public string HashCode
        {
            get { return createHash(); }
        }

        public abstract string createHash();

    }
}
