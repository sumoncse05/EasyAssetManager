using System;

namespace EasyAssetManagerCore.Models.EntityModel
{
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
}
