using EasyAssetManagerCore.Model.CommonModel;
using System;

namespace EasyAssetManagerCore.Models.EntityModel
{
    [TableAttribute("AST_RM_PORTFOLIO_TMP", "ERMP")]
    public class AST_RM_PORTFOLIO_TMP
    {
        public int File_Process_ID { get; set; }
        public string AREA_CODE { get; set; }
        public string AREA_NAME { get; set; }
        public string BRANCH_CODE { get; set; }
        public string BRANCH_NAME { get; set; }
        public string RM_CODE { get; set; }
        public string RM_NAME { get; set; }
        public string LOAN_AC_NUMBER { get; set; }
        public DateTime EFF_DATE { get; set; }
        public string INS_BY { get; set; }
        public DateTime INS_DATE { get; set; }
    }

    [TableAttribute("AST_LOAN_CL_TMP", "ERMP")]
    public class AST_LOAN_CL_TMP
    {
        public int File_Process_ID { get; set; }
        public string AREA_CODE { get; set; }
        public string AREA_NAME { get; set; }
        public string BRANCH_CODE { get; set; }
        public string BRANCH_NAME { get; set; }
        public string RM_CODE { get; set; }
        public string RM_NAME { get; set; }
        public string BST_CODE { get; set; }
        public string BST_NAME { get; set; }
        public string LOAN_AC_NUMBER { get; set; }
        public string CL_STATUS { get; set; }
        public DateTime EFF_DATE { get; set; }
        public string INS_BY { get; set; }
        public DateTime INS_DATE { get; set; }
    }

    [TableAttribute("AST_LOAN_TARGET_TMP", "ERMP")]
    public class AST_LOAN_TARGET_TMP
    {
        public int File_Process_ID { get; set; }
        public string AREA_CODE { get; set; }
        public string AREA_NAME { get; set; }
        public string BRANCH_CODE { get; set; }
        public string BRANCH_NAME { get; set; }
        public string RM_CODE { get; set; }
        public string RM_NAME { get; set; }
        public string BST_CODE { get; set; }
        public string BST_NAME { get; set; }
        public string SEG_ID { get; set; }
        public string SEG_NAME { get; set; }
        public string OS_TARGET_AMT { get; set; }
        public string DISB_TARGET_AMT { get; set; }
        public string INC_TARGET_AMT { get; set; }
        public string INS_BY { get; set; }
        public DateTime INS_DATE { get; set; }
    }

    [TableAttribute("AST_LOAN_WO_STATUS_TEMP", "ERMP")]
    public class AST_LOAN_WO_STATUS_TEMP
    {
        public int File_Process_ID { get; set; }
        public string AREA_CODE { get; set; }
        public string AREA_NAME { get; set; }
        public string BRANCH_CODE { get; set; }
        public string BRANCH_NAME { get; set; }
        public string SEG_ID { get; set; }
        public string SEG_NAME { get; set; }
        public string PRODUCT_CODE { get; set; }
        public string PRODUCT_DESC { get; set; }
        public string LOAN_AC_NUMBER { get; set; }
        public string OS_AMOUNT { get; set; }
        public string WO_AMOUNT { get; set; }
        public DateTime WO_DATE { get; set; }
        public string INS_BY { get; set; }
        public DateTime INS_DATE { get; set; }
    }
}
