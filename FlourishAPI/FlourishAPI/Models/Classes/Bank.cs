using MongoDB.Bson.Serialization.Attributes;

namespace FlourishAPI.Models.Classes
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
