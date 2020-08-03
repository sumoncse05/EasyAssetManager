using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAssetManagerCore.Models.EntityModel
{
   public class ProductWiseReport
    {
        public string product_name { get; set; }
        public string product_code { get; set; }
        public string dislastyear_account { get; set; }
        public string dislastyear_amount { get; set; }
        public string discurrentyear_account { get; set; }
        public string discurrentyear_amount { get; set; }
        public string osaccount { get; set; }
        public string osamount { get; set; }
        public string ostotal { get; set; }
        public string claccount { get; set; }
        public string clamount { get; set; }
        public string clpercentage_withos { get; set; }
        public string cltotal { get; set; }
        public string revenue_amount { get; set; }
        public string revenue_total { get; set; }
        public string woaccount { get; set; }
        public string woamount { get; set; }
        public string wototal { get; set; }
    }

    public class YearWiseReport
    {
        public string year { get; set; }
        public string disaccount { get; set; }
        public string disamount { get; set; }
        public string osaccount { get; set; }
        public string osamount { get; set; }
        public string claccount { get; set; }
        public string clamount { get; set; }
        public string clpercentage_withos { get; set; }
        public string clpercentage_withdis { get; set; }
        public string woaccount { get; set; }
        public string woamount { get; set; }
        public string wototal { get; set; }
    }
}
