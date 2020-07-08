using EasyAssetManagerCore.Model.CommonModel;

namespace EasyAssetManagerCore.Models.EntityModel
{
    public class RM
    {
        public RM()
        {
            Message = new Message();
        }
        public string emp_name { get; set; }
        public string rm_code { get; set; }
        public string emp_id { get; set; }
        public string emp_cat { get; set; }
        public string desig_code { get; set; }
        public string desig_name { get; set; }
        public string branch_code { get; set; }
        public string new_branch_code { get; set; }
        public string branch_name { get; set; }
        public string area_code { get; set; }
        public string area_name { get; set; }
        public string cat_id { get; set; }
        public string cat_desc { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string dept_name { get; set; }
        public string department { get; set; }
        public string dept_code { get; set; }
        public string grade_code { get; set; }
        public string grade_name { get; set; }
        public string new_rm_code { get; set; }
        public string effect_date { get; set; }
        public string status_code { get; set; }
        public string ins_date { get; set; }
        public string ins_by { get; set; }
        public Message Message { get; set; }
    }
}
