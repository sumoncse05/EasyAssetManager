using Dapper;
using Dapper.Oracle;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Repository.Common;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;

namespace EasyAssetManagerCore.Repository.Security
{
    public class OtpRepository : BaseRepository, IOtpRepository
    {
        public OtpRepository(OracleConnection connection) : base(connection)
        {
            
        }

        public ResponseMessage SetFingerScanRequest(string pvc_otpreqtype, string pvc_usertype, string pvc_userrefno, string pvc_transrefno, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_otpreqtype", pvc_otpreqtype, OracleMappingType.Varchar2, ParameterDirection.Input,1);
            dyParam.Add("pvc_usertype", pvc_usertype, OracleMappingType.Varchar2, ParameterDirection.Input,2);
            dyParam.Add("pvc_userrefno", pvc_userrefno, OracleMappingType.Varchar2, ParameterDirection.Input,20);
            dyParam.Add("pvc_transrefno", pvc_transrefno, OracleMappingType.Varchar2, ParameterDirection.Input,20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input,50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output,10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output,200);
            dyParam.Add("pvc_otpreqrefno", 0, OracleMappingType.Varchar2, ParameterDirection.Output,20);
            var res = responseMessage.QueryExecute(Connection, "pkg_otp_manager.dpd_set_fingerscanrequest", dyParam);
            return res;
        }
        public ResponseMessage GetFingerScanStatus(string pvc_otpreqrefno, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_otpreqrefno", pvc_otpreqrefno, OracleMappingType.Varchar2, ParameterDirection.Input,20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input,50);


            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.ReturnValue,10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output,100);

            var res = responseMessage.QueryExecute(Connection, "pkg_otp_manager.dfn_get_fingerscanstatus", dyParam);
            return res;
        }
        public List<string> SetSmsOtpRequest(string userType, string userRefNo, string userMobileNo, string TransrefNo, string userId)
        {
            
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_usertype", userType, OracleMappingType.Varchar2, ParameterDirection.Input,2);
            dyParam.Add("pvc_userrefno", userRefNo, OracleMappingType.Varchar2, ParameterDirection.Input,20);
            dyParam.Add("pvc_usermobileno", userMobileNo, OracleMappingType.Varchar2, ParameterDirection.Input,20);
            dyParam.Add("pvc_transrefno", TransrefNo, OracleMappingType.Varchar2, ParameterDirection.Input,20);
            dyParam.Add("pvc_appuser", userId, OracleMappingType.Varchar2, ParameterDirection.Input,50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output,10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output,200);
            dyParam.Add("pvc_otpreqrefno", 0, OracleMappingType.Varchar2, ParameterDirection.Output,20);
            dyParam.Add("pvc_requestdata", 0, OracleMappingType.Varchar2, ParameterDirection.Output,6);
            var result =Connection.Execute("pkg_otp_manager.dpd_set_smsotprequest", dyParam, commandType: CommandType.StoredProcedure);

            var pvc_status = dyParam.Get<string>("@pvc_status");
            var pvc_statusmsg = dyParam.Get<string>("@pvc_statusmsg");
            var pvc_otpreqrefno = dyParam.Get<string>("@pvc_otpreqrefno");
            var pvc_requestdata = dyParam.Get<string>("@pvc_requestdata");
            List<string> res = new List<string> { pvc_status, pvc_statusmsg, pvc_otpreqrefno, pvc_requestdata };
            return res;
        }
        public ResponseMessage SetSmsOtpResponse(string pvc_otpreqrefno, string pvc_requestdata, string pvc_responsedata, string pvc_responsestatus, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_otpreqrefno", pvc_otpreqrefno, OracleMappingType.Varchar2, ParameterDirection.Input,20);
            dyParam.Add("pvc_requestdata", pvc_requestdata, OracleMappingType.Varchar2, ParameterDirection.Input,6);
            dyParam.Add("pvc_responsedata", pvc_responsedata, OracleMappingType.Varchar2, ParameterDirection.Input,6);
            dyParam.Add("pvc_responsestatus", pvc_responsestatus, OracleMappingType.Varchar2, ParameterDirection.Input,1);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input,50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output,10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output,200);

            var res = responseMessage.QueryExecute(Connection, "pkg_otp_manager.dpd_set_smsotpresponse", dyParam);
            return res;
        }
    }

    public interface IOtpRepository
    {
        List<string> SetSmsOtpRequest(string userType, string userRefNo, string userMobileNo, string TransrefNo, string userId);
        ResponseMessage SetFingerScanRequest(string pvc_otpreqtype, string pvc_usertype, string pvc_userrefno, string pvc_transrefno, string pvc_appuser);
        ResponseMessage GetFingerScanStatus(string pvc_otpreqrefno, string pvc_appuser);
        ResponseMessage SetSmsOtpResponse(string pvc_otpreqrefno, string pvc_requestdata, string pvc_responsedata, string pvc_responsestatus, string pvc_appuser);
    }
}
