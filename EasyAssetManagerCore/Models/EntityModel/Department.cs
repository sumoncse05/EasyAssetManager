using System;
using System.Collections.Generic;
using System.Text;
using EasyAssetManagerCore.Model.CommonModel;

namespace EasyAssetManagerCore.Models.EntityModel
{
    public class Department
    {
        public Department()
        {
            Message = new Message();
        }
        public string dept_code { get; set; }
        public string dept_name { get; set; }
        public Message Message { get; set; }
    }
}
