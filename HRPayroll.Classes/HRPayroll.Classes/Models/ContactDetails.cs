using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HRPayroll.Classes.Models
{
    public class ContactDetails : CRUDAble
    {
        #region Fields

        [BsonElement("ContactName")]
        public string ContactName { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("Cell")]
        public string Cell { get; set; }
        #endregion

        #region Methods

        public override string createHash()
        {
            return string.Format("{0}-{1}-{2}", ContactName, Email, Cell);
        }
        #endregion
    }
}
