using EasyAssetManagerCore.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAssetManagerCore.Models.EntityModel
{
    public class Department
    {
        public Department()
        {
            Message = new Message();
        }
        public string DEPT_CODE { get; set; }
        public string DEPT_NAME { get; set; }

        public Message Message { get; set; }
    }
}
