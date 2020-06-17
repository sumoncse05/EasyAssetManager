using EasyAssetManagerCore.Model.CommonModel;

namespace EasyAssetManagerCore.Models.EntityModel
{
    public class Remittance
    {
        public Remittance()
        {
            Message = new Message();
        }
        public string remit_sec_code { get; set; }
        public string bene_name { get; set; }
        public string bene_address { get; set; }
        public string bene_mobile { get; set; }
        public string remit_sender_name { get; set; }
        public string remit_sender_country { get; set; }
        public string remarks { get; set; }
        public string amount { get; set; }
        public string remit_comp_name { get; set; }
        public string agent_name { get; set; }
        public string outlet_name { get; set; }
        public string address { get; set; }
        public string trans_id { get; set; }
        public string remit_comp_id { get; set; }
        public string remit_receiver_name { get; set; }
        public string remit_sex { get; set; }
        public string remit_receiver_type { get; set; }
        public string remit_receiver_address { get; set; }
        public string remit_receiver_accountno { get; set; }
        public string remit_receiver_mobile { get; set; }
        public string ins_by { get; set; }
        public string ins_date { get; set; }
        public string auth_by { get; set; }
        public string auth_date { get; set; }
        public string agent_id { get; set; }
        public string outlet_id { get; set; }
        public string customer_image_path { get; set; }
        public string cust_ac_no { get; set; }
        public string status { get; set; }
        public string branch_code { get; set; }
        public string status_code { get; set; }
        public Message Message { get; set; }
    }
}
