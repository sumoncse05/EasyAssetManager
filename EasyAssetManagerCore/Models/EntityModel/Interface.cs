using System;
using System.Collections.Generic;
using System.Text;
using EasyAssetManagerCore.Model.CommonModel;

namespace EasyAssetManagerCore.Models.EntityModel
{
    public class Interface
    {
        public Interface()
        {
            Message = new Message();
        }
        public string MOD_ID { get; set; }
        public string MOD_DESC { get; set; }
        public string MOD_PATH { get; set; }
        public string SCR_ID { get; set; }
        public string SCR_PARENT_ID { get; set; }
        public Nullable<decimal> SCR_SEQNO { get; set; }
        public string SCR_NAME { get; set; }
        public string SCR_LINK { get; set; }
        public string SCR_TYPE { get; set; }
        public string SCR_STATUS { get; set; }
        public string INS_BY { get; set; }
        public string INS_DATE { get; set; }
        public string UPD_BY { get; set; }
        public string UPD_DATE { get; set; }
        public string AUTH_BY { get; set; }
        public string AUTH_DATE { get; set; }

        public Message Message { get; set; }
    }
}
