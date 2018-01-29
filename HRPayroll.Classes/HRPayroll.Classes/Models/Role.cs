using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HRPayroll.Classes.Models
{
    public class Role : CRUDAble
    {
        #region Fields


        [BsonElement("Name")]
        public string Name { get; set; }

        public override string createHash()
        {
            return string.Format("{0}",Name);
        }
        #endregion
    }
}
