using System;
using EasyAssetManagerCore.Model.CommonModel;
using System.Text;

namespace EasyAssetManagerCore.Models.EntityModel
{
    public class WorkflowDetail
    {
        public WorkflowDetail()
        {
            Message = new Message();
        }
        public string wf_slno { get; set; }
        public string wf_type { get; set; }
        public string wf_desc { get; set; }
        public string cust_no { get; set; }
        public string cust_ac_no { get; set; }
        public string card_no { get; set; }
        public string ac_desc { get; set; }
        public string cust_name { get; set; }
        public string acy_avl_bal { get; set; }
        public string customer_name { get; set; }
        public string mobile_number { get; set; }
        public string father_name { get; set; }
        public string mother_name { get; set; }
        public string birth_date { get; set; }
        public string sex { get; set; }
        public string nid { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string photo_type { get; set; }
        public string amount { get; set; }
        public string remarks { get; set; }
        public string wf_ref_no { get; set; }
        public string wf_resp_dtl { get; set; }
        public DateTime? wf_eff_date { get; set; }
        public string finger_slno { get; set; }
        public string finger_status { get; set; }
        public string status_code { get; set; }
        public string status_msg { get; set; }
        public string ins_by { get; set; }
        public string ins_date { get; set; }
        public string upd_by { get; set; }
        public string upd_date { get; set; }
        public string auth_date { get; set; }
        public string auth_by { get; set; }
        public string customer_no { get; set; }
        public string vat_amount { get; set; }
        public string checkbook_leaves { get; set; }
        public string workflow_state { get; set; }
        public string checkbook_requisition_type { get; set; }
        public Message Message { get; set; }
    }
}
