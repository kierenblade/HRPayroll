using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace FlourishAPI.Models.Classes
{
    public enum PaymentType { ABSA = 1, VISA }
    public class Company: CRUDAble
        {

        [BsonElement("CompanyId")]
        public int CompanyId { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("AccountNumber")]
        public string AccountNumber { get; set; }

        [BsonElement("Bank")]
        public Bank Bank { get; set; }

        [BsonElement("CardNumber")]
        public string CardNumber { get; set; }

        [BsonElement("CardCVV")]
        public string CardCVV { get; set; }

        [BsonElement("BusinessUnits")]
        public List<BusinessUnit> BusinessUnits { get; set; }
        [BsonElement("PaymentType")]
        public PaymentType PaymentType { get; set; }

        #region Methods
        public override string createHash()
        {
            return string.Format("{0}-{1}-{2}", CompanyId,Name,AccountNumber);
        }
        #endregion

    }
}
