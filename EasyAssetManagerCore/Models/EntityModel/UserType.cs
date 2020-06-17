using EasyAssetManagerCore.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAssetManagerCore.Models.EntityModel
{
    public class UserType
    {
        public UserType()
        {
            Message = new Message();
        }
        public string USER_TYPE { get; set; }
        public string USER_DESC { get; set; }
        public string LICENCE_REQ { get; set; }
        public string LIMIT_REQ { get; set; }
        public string ALLOW_TRANS { get; set; }
        public string ALLOW_APP { get; set; }
        public string INS_BY { get; set; }
        public string INS_DATE { get; set; }
        public string UPD_BY { get; set; }
        public string UPD_DATE { get; set; }
        public string AUTH_BY { get; set; }
        public string AUTH_DATE { get; set; }

        public Message Message { get; set; }
    }
}
