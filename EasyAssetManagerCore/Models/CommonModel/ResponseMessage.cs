using Dapper;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Linq;

namespace EasyAssetManagerCore.Models.CommonModel
{
    public class ResponseMessage
    {
        public string pvc_regslno { get; set; }
        public string pvc_status { get; set; }
        public string pvc_statusmsg { get; set; }
        public string pvc_otpreqrefno { get; set; }
        public string pvc_requestdata { get; set; }
        public string pvc_acdesc { get; set; }
        public string pvc_operationmode { get; set; }
        public string pnm_commamt { get; set; }
        public string pnm_vatamt { get; set; }
        public string pnm_totalamt { get; set; }
        public string pvc_transid { get; set; }
        public string pvc_charge { get; set; }
        public string pvc_chargevat { get; set; }
        public string pvc_totalamount { get; set; }
        public string pvc_balance { get; set; }
        public string pvc_acregslno { get; set; }
        public string pvc_wfslno { get; set; }
        public int rowfound { get; set; }
        public string pvc_limitid { get; set; }
        public string pvc_limitslno { get; set; }
        public string pvc_agentid { get; set; }
        public string pvc_outletid { get; set; }
        public string pvc_msg { get; set; }
        public string pnm_run_id { get; set; }
        public ResponseMessage()
        {
            pvc_status = "40900";
        }

        public ResponseMessage QueryExecute(OracleConnection connection,string procedure, OracleDynamicParameters dyParam)
        {
            var result = connection.Execute(procedure, dyParam, commandType: CommandType.StoredProcedure);
            var properties = typeof(ResponseMessage).GetProperties();
            var responseMessage = new ResponseMessage();
            foreach (var v in properties)
            {
                if (dyParam.ParameterNames.Contains(v.Name) && dyParam.GetParameter(v.Name).ParameterDirection != ParameterDirection.Input)
                {
                    SetObjectProperty(responseMessage, v.Name, dyParam.Get<string>("@"+v.Name));
                }           
            }
            return responseMessage;
        }

      private void SetObjectProperty(object theObject, string propertyName, object value)
        {
            Type type = theObject.GetType();
            var property = type.GetProperty(propertyName);
            if (property != null)
            {
                var setter = property.SetMethod;
                setter.Invoke(theObject, new object[] { value });
            }
          
        }

    }

    public enum ResponseMessageCode
    {
        Success=40999

    }
}
