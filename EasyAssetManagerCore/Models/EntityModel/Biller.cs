using EasyAssetManagerCore.Model.CommonModel;

namespace EasyAssetManagerCore.Models.EntityModel
{
    public class Biller
    {
        public Biller()
        {
            Message = new Message();
        }
        public string biller_id { get; set; }
        public string biller_desc { get; set; }
        public string cust_ac_no { get; set; }
        public string status { get; set; }
        public string branch_code { get; set; }
        public string ins_by { get; set; }
        public string ins_date { get; set; }
        public string auth_by { get; set; }
        public string auth_date { get; set; }
        public Message Message { get; set; }
    }

   public class BillPayCash
    {
        public string biller_id { get; set; }
        public string biller_desc { get; set; }
        public string bill_ref_no { get; set; }

        public string customerName { get; set; }
        public string customerMobile { get; set; }
        public string billDetails { get; set; }
        public string transactionDate { get; set; }
        public string transactionAmount { get; set; }
        public string transactionAmountVat { get; set; }
        public string transactionAccountNo { get; set; }
        public string transactionCharge { get; set; }
        public string transactionChargeVat { get; set; }
        public string transactionTotalAmount { get; set; }
        public string sex { get; set; }
        public string customerType { get; set; }
        public string customerAccount { get; set; }
        //public string billMonth { get; set; }
        public string stampValue { get; set; }
        public string cardName { get; set; }
        public string productName { get; set; }
        public Message Message { get; set; }
    }
}
