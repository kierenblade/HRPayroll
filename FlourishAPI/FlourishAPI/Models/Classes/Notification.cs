using System;
using MongoDB.Bson.Serialization.Attributes;

namespace FlourishAPI.Models.Classes
{
    #region Enums
    public enum Area { ClientApi = 1, Reporting, Transaction, ThirdParty }
    public enum AreaStatus { Success = 1, Fail }
    #endregion
    public class DesktopNotification : CRUDAble{
        #region Fields

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

        #region Methods
        public override string createHash()
        {
            return string.Format("{0}-{1}-{2}-{3}-{4}-{5}-{6}", NotificationId,CreationDate,Company.Name,Message,LoginDetails.Username,Area,AreaStatus);
        }
        #endregion
    }
}
