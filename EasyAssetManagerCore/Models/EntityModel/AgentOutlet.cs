using EasyAssetManagerCore.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAssetManagerCore.Models.EntityModel
{
    public class AgentOutlet
    {
        public AgentOutlet()
        {
            Message = new Message();
        }
        public string OUTLET_ID { get; set; }
        public string OUTLET_NAME { get; set; }
        public string AGENT_ID { get; set; }
        public string AGENT_USR_ID { get; set; }
        public string AGENT_NAME { get; set; }
        public string BRANCH_CODE { get; set; }
        public string CUST_AC_NO { get; set; }
        public string DIV_CODE { get; set; }
        public string DIV_NAME { get; set; }
        public string DIST_CODE { get; set; }
        public string DIST_NAME { get; set; }
        public string THANA_CODE { get; set; }
        public string THANA_NAME { get; set; }
        public string LOC_TYPE { get; set; }
        public string ADDRESS { get; set; }
        public string CONTACT_NAME { get; set; }
        public string CONTACT_MOBILE { get; set; }
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
