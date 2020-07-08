using System;
using System.Collections.Generic;
using System.Text;
using EasyAssetManagerCore.Model.CommonModel;

namespace EasyAssetManagerCore.Models.EntityModel
{
    public class Grade
    {
        public Grade()
        {
            Message = new Message();
        }
        public string grade_code { get; set; }
        public string grade_name { get; set; }
        public Message Message { get; set; }
    }
}
