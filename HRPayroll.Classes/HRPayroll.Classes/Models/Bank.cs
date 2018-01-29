using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HRPayroll.Classes.Models
{
    public class Bank : CRUDAble
    {
        #region Fields

        [BsonElement("BankId")]
        public int BankId { get; set; }

        [BsonElement("BankName")] public string Name { get; set; }
        #endregion


        public override string createHash()
        {
            return string.Format("{0}-{1}", BankId, Name);
        }
    }
}
