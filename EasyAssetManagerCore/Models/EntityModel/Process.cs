using EasyAssetManagerCore.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAssetManagerCore.Models.EntityModel
{
    public class Process
    {
        public Process()
        {
            Message = new Message();
        }
        public string RUN_ID { get; set; }
        public string STATUS_TEXT { get; set; }
        public string PROGRAM_NAME { get; set; }
        public string COMPLETED { get; set; }
        public Message Message { get; set; }
    }
}
