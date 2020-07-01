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
        public string area_code { get; set; }
        public string branch_code { get; set; }
        public string rm_code { get; set; }
        public int business_year { get; set; }
        public double os_target_amt { get; set; }
        public double disb_target_amt { get; set; }
        public double mtd_os_target_amt { get; set; }
        public double mtd_disb_target_amt { get; set; }
        public double mtd_loan_cnt { get; set; }
        public double mtd_loan_amt { get; set; }
        public double mtd_os_amt { get; set; }
        public double mtd_disb_cnt { get; set; }
        public double mtd_disb_amt { get; set; }
        public double mtd_ss_cnt { get; set; }
        public double mtd_ss_amt { get; set; }
        public double mtd_df_cnt { get; set; }
        public double mtd_df_amt { get; set; }
        public double mtd_bl_cnt { get; set; }
        public double mtd_bl_amt { get; set; }
        public double mtd_wo_cnt { get; set; }
        public double mtd_wo_amt { get; set; }
        public double mtd_close_cnt { get; set; }
        public double mtd_close_amt { get; set; }
        public double qtd_os_target_amt { get; set; }
        public double qtd_disb_target_amt { get; set; }
        public double qtd_loan_cnt { get; set; }
        public double qtd_loan_amt { get; set; }
        public double qtd_os_amt { get; set; }
        public double qtd_disb_cnt { get; set; }
        public double qtd_disb_amt { get; set; }
        public double qtd_ss_cnt { get; set; }
        public double qtd_ss_amt { get; set; }
        public double qtd_df_cnt { get; set; }
        public double qtd_df_amt { get; set; }
        public double qtd_bl_cnt { get; set; }
        public double qtd_bl_amt { get; set; }
        public double qtd_wo_cnt { get; set; }
        public double qtd_wo_amt { get; set; }
        public double qtd_close_cnt { get; set; }
        public double qtd_close_amt { get; set; }
        public double ytd_os_target_amt { get; set; }
        public double ytd_disb_target_amt { get; set; }
        public double ytd_loan_cnt { get; set; }
        public double ytd_loan_amt { get; set; }
        public double ytd_os_amt { get; set; }
        public double ytd_disb_cnt { get; set; }
        public double ytd_disb_amt { get; set; }
        public double ytd_ss_cnt { get; set; }
        public double ytd_ss_amt { get; set; }
        public double ytd_df_cnt { get; set; }
        public double ytd_df_amt { get; set; }
        public double ytd_bl_cnt { get; set; }
        public double ytd_bl_amt { get; set; }
        public double ytd_wo_cnt { get; set; }
        public double ytd_wo_amt { get; set; }
        public double ytd_close_cnt { get; set; }
        public double ytd_close_amt { get; set; }
        public string ins_by { get; set; }
        public string ins_date { get; set; }
        public string upd_date { get; set; }
        public string upd_by { get; set; }
        public Message Message { get; set; }
        
    }
}
