using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HRPayroll.Classes.Models
{
    #region Enums
    public enum Area { ClientApi = 1, Reporting, Transaction, ThirdParty }
    public enum AreaStatus { Success = 1, Fail }
    #endregion
    public class DesktopNotification{
        #region Fields
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("NotificationId")]
        public int NotificationId { get; set; }

        [BsonElement("CreationDate")]
        public DateTime CreationDate { get; set; }

        [BsonElement("Company")]
        public Company Company { get; set; }

        [BsonElement("Message")]
        public string Message { get; set; }

        [BsonElement("LoginDetails")]
        public LoginDetails LoginDetails { get; set; }

        [BsonElement("Area")]
        public Area Area { get; set; }

        [BsonElement("AreaStatus")]
        public AreaStatus AreaStatus { get; set; }
        #endregion
    }
}
