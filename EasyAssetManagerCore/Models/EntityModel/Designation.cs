using System;
using System.Collections.Generic;
using EasyAssetManagerCore.Model.CommonModel;
using System.Text;

namespace EasyAssetManagerCore.Models.EntityModel
{
    public class Designation
    {
        public Designation()
        {
            Message = new Message();
        }
        public string desig_code { get; set; }
        public string desig_name { get; set; }
        public Message Message { get; set; }
    }
}
