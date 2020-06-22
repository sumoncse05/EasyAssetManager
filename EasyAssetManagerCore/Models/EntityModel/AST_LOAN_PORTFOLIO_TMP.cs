using EasyAssetManagerCore.Model.CommonModel;
using System;

namespace EasyAssetManagerCore.Models.EntityModel
{
    [TableAttribute("AST_LOAN_PORTFOLIO_TMP", "ERMP")]
    public class AST_LOAN_PORTFOLIO_TMP
    {
        public int File_Process_ID { get; set; }
        public string ID_of_Area { get; set; }
        public string Name_of_Area { get; set; }
        public string Brn_Code { get; set; }
        public string Branch_Name { get; set; }
        public string ID_of_RM { get; set; }
        public string Name_of_RM { get; set; }
        public string Loan_Acct_No { get; set; }
        public string INS_BY { get; set; }
        public DateTime INS_DATE { get; set; }
    }

    [TableAttribute("AST_LOAN_CL_TMP", "ERMP")]
    public class AST_LOAN_CL_TMP
    {
        public int File_Process_ID { get; set; }
        public string ID_of_Area { get; set; }
        public string Name_of_Area { get; set; }
        public string Brn_Code { get; set; }
        public string Branch_Name { get; set; }
        public string ID_of_RM { get; set; }
        public string Name_of_RM { get; set; }
        public string ID_of_BST { get; set; }
        public string Name_of_BST { get; set; }
        public string Loan_Acct_No { get; set; }
        public string Classification_TYPE { get; set; }
        public string INS_BY { get; set; }
        public DateTime INS_DATE { get; set; }
    }

    [TableAttribute("AST_LOAN_TARGET_TMP", "ERMP")]
    public class AST_LOAN_TARGET_TMP
    {
        public int File_Process_ID { get; set; }
        public string ID_of_Area { get; set; }
        public string Name_of_Area { get; set; }
        public string Brn_Code { get; set; }
        public string Branch_Name { get; set; }
        public string ID_of_RM { get; set; }
        public string Name_of_RM { get; set; }
        public string ID_of_BST { get; set; }
        public string Name_of_BST { get; set; }
        public string Out_Standing_Amount { get; set; }
        public string Disbursed_Amount { get; set; }
        public string INS_BY { get; set; }
        public DateTime INS_DATE { get; set; }
    }

    [TableAttribute("AST_LOAN_WO_STATUS_TEMP", "ERMP")]
    public class AST_LOAN_WO_STATUS_TEMP
    {
        public int File_Process_ID { get; set; }
        public string AREA_CODE { get; set; }
        public string LOAN_NUMBER { get; set; }
        public string LOAN_OUTSTANDING { get; set; }
        public string WO_AMOUNT { get; set; }
        public string AREA_NAME { get; set; }
        public string INS_BY { get; set; }
        public DateTime INS_DATE { get; set; }
    }
}
