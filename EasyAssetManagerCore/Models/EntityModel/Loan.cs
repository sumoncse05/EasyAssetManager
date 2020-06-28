namespace EasyAssetManagerCore.Models.EntityModel
{
    public class Loan
    {
        public string loan_slno { get; set; }
        public string seg_id { get; set; }
        public string seg_desc { get; set; }
        public string area_code { get; set; }
        public string branch_code { get; set; }
        public string branch_name { get; set; }
        public string old_rm_code { get; set; }
        public string old_rm_name { get; set; }
        public string new_rm_code { get; set; }
        public string new_rm_name { get; set; }
        public string loan_ac_number { get; set; }
        public string loan_ac_name { get; set; }
        public string product_code { get; set; }
        public string loan_category { get; set; }
        public string book_date { get; set; }
        public string loan_amount { get; set; }
        public string outstanding_amount { get; set; }
        public string rescheduled { get; set; }
        public string status { get; set; }
        public string ins_date { get; set; }
        public string ins_by { get; set; }
        public string upd_date { get; set; }
        public string upd_by { get; set; }
        public string auth_date { get; set; }
        public string auth_by { get; set; }
    }
}
