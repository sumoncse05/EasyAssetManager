using System;
using System.Collections.Generic;
using System.Text;
using EasyAssetManagerCore.Model.CommonModel;

namespace EasyAssetManagerCore.Models.EntityModel
{
    public class AstDailyStatus
    {
        public AstDailyStatus()
        {
            Message = new Message();
        }
        public string seg_id { get; set; }
        public string seg_desc { get; set; }
        public string area_code { get; set; }
        public string area_name { get; set; }
        public string branch_code { get; set; }
        public string branch_name { get; set; }
        public string rm_code { get; set; }
        public string emp_name { get; set; }
        public int business_year { get; set; }
        public string business_date { get; set; }
        public double os_pos_lyr_ac { get; set; }
        public double os_pos_lyr_amount { get; set; }
        public double os_pos_lmon_ac { get; set; }
        public double os_pos_lmon_amount { get; set; }
        public double os_pos_ytd { get; set; }
        public double os_growth_ytd { get; set; }
        public double os_growth_mtd { get; set; }
        public double os_growth_trgt_crnt_yr { get; set; }
        public double oa_growth_target_ytd { get; set; }
        public double achit_os_growth_ytd { get; set; }
        public double pd_last_yr_ac { get; set; }
        public double pd_last_yr_amount { get; set; }
        public double pd_ytd_ac { get; set; }
        public double pd_ytd_amount { get; set; }
        public double pd_diff_ac { get; set; }
        public double pd_diff_amount { get; set; }
        public double rl_ytd_ac { get; set; }
        public double rl_ytd_amount { get; set; }
        public double rl_mtd_ac { get; set; }
        public double rl_mtd_amount { get; set; }
        public double inc_trgt_curnt_yr { get; set; }
        public double inc_ytd_trgt { get; set; }
        public double inc_ach_ytd { get; set; }
        public double inc_percnt_acht { get; set; }
        public double cl_ss_ac { get; set; }
        public double cl_ss_amount { get; set; }
        public double cl_df_ac { get; set; }
        public double cl_df_amount { get; set; }
        public double cl_bl_ac { get; set; }
        public double cl_bl_amount { get; set; }
        public double cl_tot_ac { get; set; }
        public double cl_tot_amount { get; set; }
        public double cl_percnt_cl { get; set; }
        public double wo_lst_ac { get; set; }
        public double wo_lst_amount { get; set; }
        public double clos_ac_ac { get; set; }
        public double clos_ac_amount { get; set; }
        public string ins_by { get; set; }
        public string ins_date { get; set; }
        public string upd_date { get; set; }
        public string upd_by { get; set; }
        public Message Message { get; set; }

    }
}
