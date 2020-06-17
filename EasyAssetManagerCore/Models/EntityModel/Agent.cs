using EasyAssetManagerCore.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAssetManagerCore.Models.EntityModel
{
    public class Agent
    {
        public Agent()
        {
            Message = new Message();
        }
        public string AGENT_ID { get; set; }
        public string AGENT_USR_ID { get; set; }
        public string AGENT_NAME { get; set; }
        public string REF_AGENT_ID { get; set; }
        public string AGENT_TYPE { get; set; }
        public string SEX { get; set; }
        public string CUSTOMER_NO { get; set; }
        public string PHONE { get; set; }
        public string MOBILE { get; set; }
        public string EMAIL { get; set; }
        public string TRADE_LIC { get; set; }
        public string ADDRESS { get; set; }
        public string STATUS { get; set; }
        public string INS_BY { get; set; }
        public string INS_DATE { get; set; }
        public string UPD_BY { get; set; }
        public string UPD_DATE { get; set; }
        public string AUTH_BY { get; set; }
        public string AUTH_DATE { get; set; }

        public Message Message { get; set; }
    }
}
