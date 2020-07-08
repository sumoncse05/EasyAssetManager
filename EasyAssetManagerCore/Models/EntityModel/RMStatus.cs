using System;
using System.Collections.Generic;
using System.Text;
using EasyAssetManagerCore.Model.CommonModel;

namespace EasyAssetManagerCore.Models.EntityModel
{
    public class RMStatus
    {
        public RMStatus()
        {
            Message = new Message();
        }
        public string status_code { get; set; }
        public string status_desc { get; set; }
        public Message Message { get; set; }
    }
}
