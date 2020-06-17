namespace EasyAssetManagerCore.Models.EntityModel
{
    public class TransactionXml
    {
        public string from_branch_code { get; set; }
        public string from_account { get; set; }
        public string trans_code { get; set; }
        public string VALUEDT { get; set; }
        public string total_amount { get; set; }
        public string addl_txt { get; set; }
        public string AMOUNT { get; set; }
        public string biller_rev_ac { get; set; }
        public string to_branch_code { get; set; }
        public string amount_vat { get; set; }
        public string to_account { get; set; }
        public string charge { get; set; }
        public string charge_gl { get; set; }
        public string agent_branch_code { get; set; }
        public string charge_vat { get; set; }
        public string charge_vat_gl { get; set; }
        public string biller_id { get; set; }
        public string biller_vat_ac { get; set; }
        public string trans_id { get; set; }
        public string bene_mobile { get; set; }

    }
    public class TransactionXmlConstant
    {
        public static string SOURCE { get; set; }
        public static string UBSCOMP { get; set; }
        public static string USERID { get; set; }
        public static string SERVICE { get; set; }
        public static string OPERATION { get; set; }
        public static string BATCHNO { get; set; }
        public static string CCY { get; set; }
        public static string AUTHSTAT { get; set; }
        public static string SLNO { get; set; }
        public static string BATCH_NO { get; set; }
        public static string DESCRIPTION { get; set; }
        public static string BALANCING { get; set; }
    }
}
