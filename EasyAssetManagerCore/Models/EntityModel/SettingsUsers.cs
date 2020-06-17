using EasyAssetManagerCore.Model.CommonModel;
using System;

namespace EasyAssetManagerCore.Models.EntityModel
{
    [TableAttribute("users", "settings")]
    public partial class SettingsUsers
    {
        public SettingsUsers()
        {
            Message = new Message();
        }
        [PKey]
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string branch_name { get; set; }
        public string agent_id { get; set; }
        public string agent_name { get; set; }
        public string Password { get; set; }
        public string outlet_id { get; set; }
        public string roleid { get; set; }
        public int SelfServiceCategoryId { get; set; }
        public string Locked { get; set; }
        public string Active { get; set; }
        public string Loggedin { get; set; }
        public bool? ChangePassword { get; set; }
        public string StationIp { get; set; }
        public string SessionId { get; set; }
        public string outlet_name { get; set; }
        public string address { get; set; }
        public string agent_cust_ac_no { get; set; }
        public string pass_expired { get; set; }
        public string mod_id { get; set; }
        public string user_type { get; set; }
        public decimal AgentBalance { get; set; }
        public string mobile { get; set; }
        public string PhotoPath { get; set; }
        public string NewPassword { get; set; }
        public string ConfPassword { get; set; }
        public string otp_req { get; set; }


        public string email { get; set; }
        public string phone { get; set; }
        public string ins_by { get; set; }
        public string ins_date { get; set; }
        public string upd_by { get; set; }
        public string upd_date { get; set; }
        public string auth_by { get; set; }
        public string auth_date { get; set; }
        public string user_desc { get; set; }
        public string rst_slno { get; set; }
        public string reset_code { get; set; }
        public string rst_reason { get; set; }
        public string req_by { get; set; }
        public string req_date { get; set; }

        [NotMaped]
        public string ReturnURL { get; set; }
        public Message Message { get; set; }
        public UserRole Role { get; set; }

    }
}
