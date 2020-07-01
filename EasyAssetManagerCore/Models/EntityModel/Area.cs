using System;
using System.Collections.Generic;
using System.Text;
using EasyAssetManagerCore.Model.CommonModel;

namespace EasyAssetManagerCore.Models.EntityModel
{
    public class Area
    {
        public Area()
        {
            Message = new Message();
        }
        public string AREA_CODE { get; set; }
        public string AREA_NAME { get; set; }
        public Message Message { get; set; }
    }
}
