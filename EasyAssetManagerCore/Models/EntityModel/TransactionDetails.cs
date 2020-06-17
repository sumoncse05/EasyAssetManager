using EasyAssetManagerCore.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAssetManagerCore.Models.EntityModel
{
    public class TransactionDetails
    {
        public TransactionDetails()
        {
            Message = new Message();
        }

        public string trans_id { get; set; }
        public string trans_type { get; set; }
        public string trans_desc { get; set; }
        public string branch_code { get; set; }
        public string cust_ac_no { get; set; }
        public string cust_ac_desc { get; set; }
        public string alt_branch_code { get; set; }
        public string alt_ac_no { get; set; }
        public string alt_ac_desc { get; set; }
        public string amount { get; set; }
        public string charge { get; set; }
        public string charge_vat { get; set; }
        public string tot_amount { get; set; }
        public string cust_amount { get; set; }
        public string bearer_type { get; set; }
        public string bearer_desc { get; set; }
        public string bearer_ref_no { get; set; }
        public string bearer_name { get; set; }
        public string remit_sender_name { get; set; }
        public string remit_sender_country { get; set; }
        public string valuedt { get; set; }
        public string addl_txt { get; set; }
        public string bene_mobile { get; set; }
        public string ins_by { get; set; }
        public string ins_date { get; set; }
        public string UbsTransactionRefNo { get; set; }
        public Message  Message { get; set; }
    }
}
