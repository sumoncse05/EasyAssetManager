using System;
using System.Collections.Generic;
using System.Text;
using EasyAssetManagerCore.Model.CommonModel;

namespace EasyAssetManagerCore.Models.EntityModel
{
    public class Category
    {
        public Category()
        {
            Message = new Message();
        }
        public string cat_id { get; set; }
        public string cat_desc { get; set; }
        public Message Message { get; set; }
    }
}
