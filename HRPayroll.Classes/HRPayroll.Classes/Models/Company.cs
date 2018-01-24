using MongoDB.Bson.Serialization.Attributes;
namespace HRPayroll.Classes.Models
{
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
    }
}
