namespace EasyAssetManagerCore.Models.EntityModel
{
    public class BatchProcess
    {
        public string RUN_ID { get; set; }
        public string STATUS_TEXT { get; set; }
        public string PROGRAM_NAME { get; set; }
        public string COMPLETED { get; set; }
        public string batch_id { get; set; }
        public string process_id { get; set; }
        public string comp_code { get; set; }
        public string emp_user_no { get; set; }
        public string process_month { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string process_type { get; set; }

        public string err_fld_name { get; set; }
        public string err_fld_value { get; set; }
        public string err_code { get; set; }
        public string err_desc { get; set; }
    }
}
