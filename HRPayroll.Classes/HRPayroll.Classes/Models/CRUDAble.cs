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

        [BsonElement]
        public string PreviousHashCode
        {
             get; set; 
        }

        public abstract string createHash();

    }
}
