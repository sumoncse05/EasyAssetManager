using Dapper;
using Dapper.Oracle;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;

namespace EasyAssetManagerCore.Repository.Common
{
    public class CommonRepository : BaseRepository, ICommonRepository
    {
        public CommonRepository(OracleConnection connection) : base(connection)
        {
        }

        public IEnumerable<RemittanceCompany> GetRemittanceCompany(string pvc_remitcompid, string pvc_appuser)
        {

            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_remitcompid", pvc_remitcompid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pcr_remitcompany", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<RemittanceCompany>("pkg_lov_manager.dpd_get_remitcompany", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<Biller> GetBillerDetail(string pvc_billerid, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_billerid", pvc_billerid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pcr_billerlist", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Biller>("pkg_lov_manager.dpd_get_billerlist", dyParam, commandType: CommandType.StoredProcedure);
        }
        public ResponseMessage GetAccountStatus(BatchProcess batchProcess, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pnm_run_id", 0, OracleMappingType.Decimal, ParameterDirection.Output, 20);
            dyParam.Add("pvc_batch_id", batchProcess.batch_id, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_process_id", batchProcess.process_id, OracleMappingType.Char, ParameterDirection.Input, 4);
            dyParam.Add("pvc_comp_code", batchProcess.comp_code, OracleMappingType.Char, ParameterDirection.Input, 2);
            dyParam.Add("pvc_emp_user_no", batchProcess.emp_user_no, OracleMappingType.Varchar2, ParameterDirection.Input, 15);
            dyParam.Add("pvc_process_month", batchProcess.process_month, OracleMappingType.Varchar2, ParameterDirection.Input, 7);
            dyParam.Add("pvc_start_date", batchProcess.start_date, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_end_date", batchProcess.end_date, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_process_type", batchProcess.process_type, OracleMappingType.Char, ParameterDirection.Input, 1);
            dyParam.Add("pvc_username", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_msg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 2000);
            var responseMessage = new ResponseMessage();
            var res = Connection.Execute("dpg_utl_process_control.dpd_single_control_insert", dyParam, commandType: CommandType.StoredProcedure);
            responseMessage.pnm_run_id = dyParam.Get<decimal>("@pnm_run_id").ToString();
            responseMessage.pvc_msg = dyParam.Get<string>("@pvc_msg").ToString();
            return responseMessage;
        }
        public IEnumerable<BatchProcess> GetProcessRunStatus(string pRunId, string pUser)
        {

            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pnm_run_id", pRunId, OracleMappingType.Decimal, ParameterDirection.Input, 20);
            dyParam.Add("pvc_username", pUser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pcr_processstatus", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<BatchProcess>("dpg_utl_process_control.dpd_process_run_status", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<BatchProcess> GetProcessMsgCommand(string pRunId)
        {

            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pnm_run_id", pRunId, OracleMappingType.Decimal, ParameterDirection.Input, 20);
            dyParam.Add("pcr_processmsg", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<BatchProcess>("dpg_utl_process_control.dpd_process_msg", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<BatchProcess> GetProcessErrMsgCommand(string pRunId)
        {

            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pnm_run_id", pRunId, OracleMappingType.Decimal, ParameterDirection.Input, 20);
            dyParam.Add("pcr_processerrmsg", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<BatchProcess>("dpg_utl_process_control.dpd_process_errmsg", dyParam, commandType: CommandType.StoredProcedure);

        }
    }

    #region Interface
    public interface ICommonRepository
    {
        IEnumerable<RemittanceCompany> GetRemittanceCompany(string pvc_remitcompid, string pvc_appuser);
        IEnumerable<Biller> GetBillerDetail(string pvc_billerid, string pvc_appuser);
        ResponseMessage GetAccountStatus(BatchProcess batchProcess, string pvc_appuser);
        IEnumerable<BatchProcess> GetProcessRunStatus(string pRunId, string pUser);
        IEnumerable<BatchProcess> GetProcessMsgCommand(string pRunId);
        IEnumerable<BatchProcess> GetProcessErrMsgCommand(string pRunId);
    }

    #endregion

}
