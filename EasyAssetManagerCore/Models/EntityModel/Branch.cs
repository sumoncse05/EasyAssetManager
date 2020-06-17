using EasyAssetManagerCore.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAssetManagerCore.Models.EntityModel
{
    public class Branch
    {
        public Branch()
        {
            Message = new Message();
        }
        public string BRANCH_CODE { get; set; }
        public string BRANCH_NAME { get; set; }
        public string STATUS { get; set; }
        public Message Message { get; set; }
    }
}
