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
        public string loan_cnt_ytd { get; set; }
        public string os_amt_ytd { get; set; }
        public string loan_cnt_ytd_mtd { get; set; }
        public string os_amt_ytd_mtd { get; set; }
        public string loan_cnt_ytd_lytd { get; set; }
        public string os_amt_ytd_lytd { get; set; }
        public string os_pos_ytd { get; set; }
        public string os_growth_ytd { get; set; }
        public string os_growth_mtd { get; set; }
        public string os_target_amt { get; set; }
        public string os_target_ytd { get; set; }
        public string achit_os_growth_ytd { get; set; }
        public string pd_loan_cnt_yr { get; set; }
        public string pd_loan_amt_yr { get; set; }
        public string pd_loan_cnt_ytd { get; set; }
        public string pd_loan_amt_ytd { get; set; }
        public string pd_diff_ac { get; set; }
        public string pd_diff_amount { get; set; }
        public string disb_cnt_ytd { get; set; }
        public string disb_amt_ytd { get; set; }
        public string disb_cnt_ytd_mtd { get; set; }
        public string disb_amt_ytd_mtd { get; set; }
        public string inc_target_amt { get; set; }
        public string inc_target_ytd { get; set; }
        public string inc_amt_ytd { get; set; }
        public string inc_percnt_acht { get; set; }
        public string ss_cnt_ytd { get; set; }
        public string ss_amt_ytd { get; set; }
        public string df_cnt_ytd { get; set; }
        public string df_amt_ytd { get; set; }
        public string bl_cnt_ytd { get; set; }
        public string bl_amt_ytd { get; set; }
        public string cl_tot_ac { get; set; }
        public string cl_tot_amount { get; set; }        
        public string cl_percnt_cl { get; set; }
        public string cl_tot_amount_lytd { get; set; }
        public string cl_tot_ac_lytd { get; set; }
        public string cl_diff_amount { get; set; }
        public string cl_diff_ac { get; set; }
        public string wo_lst_ac { get; set; }
        public string wo_lst_amount { get; set; }
        public string close_cnt_ytd { get; set; }
        public string close_amt_ytd { get; set; }
        public string disb_target_ytd { get; set; }
        public string month_name { get; set; }
        public string ins_by { get; set; }
        public string ins_date { get; set; }
        public string upd_date { get; set; }
        public string upd_by { get; set; }
        public Message Message { get; set; }
    }
}
