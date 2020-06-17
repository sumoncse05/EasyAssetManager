using EasyAssetManagerCore.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAssetManagerCore.Models.EntityModel
{
    public class AccountOpening
    {
        public AccountOpening()
        {
            Message = new Message();
        }
        public string ac_reg_slno { get; set; }
        public string ac_customer_type { get; set; }
        public string joint_ac_indicator { get; set; }
        public Nullable<decimal> no_of_customer { get; set; }
        public string ac_name { get; set; }
        public string cheque_book { get; set; }
        public string debit_card { get; set; }
        public string remarks { get; set; }
        public string cust_ac_no { get; set; }
        public string ac_desc { get; set; }
        public Nullable<System.DateTime> ac_open_date { get; set; }
        public string status_code { get; set; }
        public string status_msg { get; set; }
        public string ins_by { get; set; }
        public string ins_date { get; set; }
        public string UPD_BY { get; set; }
        public string upd_date { get; set; }
        public string auth_by { get; set; }
        public string auth_date { get; set; }

        public Message Message { get; set; }
    }
}
