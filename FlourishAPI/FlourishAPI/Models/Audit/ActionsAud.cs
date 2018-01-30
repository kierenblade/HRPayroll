using FlourishAPI.DTOs;
using FlourishAPI.Models.Classes;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlourishAPI.Models.Audit
{
    public class ActionsAud : CRUDAble
    {

        #region Fields

        [BsonElement("Message")]
        public string Message { get; set; }

        [BsonElement("Origin")]
        public string Origin { get; set; }

        [BsonElement("DoneBy")]
        public string DoneBy { get; set; }

        [BsonElement("LogTime")]
        public DateTime LogTime { get; set; }

        [BsonElement("DoneByCompany")]
        public string DoneByCompanyName { get; set; }
        #endregion
        public override string createHash()
        {
            return string.Format("{0}-{1}-{2}-{3}-{4}", DoneBy, DoneByCompanyName, Message, Origin, LogTime);
        }

        //public ActionsAud(LogActionDTO log)
        //{
        //    Message = log.Message;
        //    Origin = log.Origin;
        //    DoneBy = log.DoneBy;
        //    DoneByCompanyName = log.DoneByCompanyName;
        //    LogTime = DateTime.Now;
        //}
    }
}
