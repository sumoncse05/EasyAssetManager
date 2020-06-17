using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAssetManagerCore.Models.EntityModel
{
    public class AccountOpeningReq
    {
        public string ac_reg_slno { get; set; }
        public string ac_customer_type_desc { get; set; }
        public string joint_desc { get; set; }
        public string no_of_customer { get; set; }
        public string ac_name { get; set; }
        public string cheque_book { get; set; }
        public string debit_card { get; set; }
        public string cust_ac_no { get; set; }
        public string ac_desc { get; set; }
        public string ac_open_date { get; set; }
        public string remarks { get; set; }
        public string ins_by { get; set; }        
        public string ins_date { get; set; }
        public string auth_date { get; set; }
        public string auth_by { get; set; }
    }
}
