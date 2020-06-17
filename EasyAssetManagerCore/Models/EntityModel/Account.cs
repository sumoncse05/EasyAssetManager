using EasyAssetManagerCore.Model.CommonModel;
using System;

namespace EasyAssetManagerCore.Models.EntityModel
{
   public class Account
    {
        public Account()
        {
            Message = new Message();
        }
        public string cust_ac_no { get; set; }
        public string customer_no { get; set; }
        public string ac_dtl { get; set; }
      //  public string cust_no { get; set; }
        public string ac_desc { get; set; }
        public string ac_open_date { get; set; }
        public string agent_id { get; set; }
        public string status { get; set; }
        public string branch_code { get; set; }
        public string ins_by { get; set; }
        public string ins_date { get; set; }
        public Message Message { get; set; }
    }
}
