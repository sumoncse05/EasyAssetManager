using Dapper;
using Dapper.Oracle;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Common;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;

namespace EasyAssetManagerCore.Repository.Operation.Asset
{
    public class FileProcessRepository : BaseRepository, IFileProcessRepository
    {
        public FileProcessRepository(OracleConnection connection) : base(connection)
        {

        }

        public int DeleteTable(string tableName, string user_id)
        {
            var sql = "DELETE FROM ERMP."+tableName+" WHERE INS_BY='"+user_id+"'";
            var result = Connection.Execute(sql);
            return result;
        }

        public IEnumerable<Account> GeAccountDetails(string customerAccountNo, string userId)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_custacno", customerAccountNo, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", userId, OracleMappingType.Varchar2, ParameterDirection.Input);

            dyParam.Add("pcr_accountdtl", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Account>("pkg_lov_manager.dpd_get_accountdtl", dyParam, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<District> GetDistrictList(string div_code, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_divcode", div_code, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);

            dyParam.Add("pcr_districtlist", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<District>("pkg_lov_manager.dpd_get_districtlist", dyParam, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<Division> GetDivisionList(string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);

            dyParam.Add("pcr_divisionlist", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Division>("pkg_lov_manager.dpd_get_divisionlist", dyParam, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<Thana> GetThanaList(string div_code, string dist_code, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_divcode", div_code, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_distcode", dist_code, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);

            dyParam.Add("pcr_thanalist", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Thana>("pkg_lov_manager.dpd_get_thanalist", dyParam, commandType: CommandType.StoredProcedure);

        }

        public int Process_LOAN_CL(List<AST_LOAN_CL_TMP> portFolios)
        {
            var sql = QB<AST_LOAN_CL_TMP>.Insert();
            var affectedRows = Connection.Execute(sql, portFolios);
            return affectedRows;
        }

        public int Process_LOAN_PORTFOLIO(List<AST_RM_PORTFOLIO_TMP> portFolios)
        {
            //var sql = @"insert into ERMP.AST_LOAN_PORTFOLIO_TMP (File_Process_ID,ID_of_Area,Name_of_Area,Brn_Code,Branch_Name,ID_of_RM,Name_of_RM,Loan_Acct_No,INS_BY,INS_DATE)
            //          values (:File_Process_ID,:ID_of_Area,:Name_of_Area,:Brn_Code,:Branch_Name,:ID_of_RM,:Name_of_RM,:Loan_Acct_No,:INS_BY,:INS_DATE)";
            var sql = QB<AST_RM_PORTFOLIO_TMP>.Insert();
            var affectedRows = Connection.Execute(sql, portFolios);
            return affectedRows;
        }

        public int Process_LOAN_TARGET(List<AST_LOAN_TARGET_TMP> portFolios)
        {
            var sql = QB<AST_LOAN_TARGET_TMP>.Insert();
            var affectedRows = Connection.Execute(sql, portFolios);
            return affectedRows;
        }

        public int Process_LOAN_WO(List<AST_LOAN_WO_STATUS_TEMP> portFolios)
        {
            var sql = QB<AST_LOAN_WO_STATUS_TEMP>.Insert();
            var affectedRows = Connection.Execute(sql, portFolios);
            return affectedRows;
        }

        public ResponseMessage SetProcess_LOAN_WO(int fileProcessId,int business_year, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_process_id", fileProcessId, OracleMappingType.Int32, ParameterDirection.Input, 20);
            dyParam.Add("pvc_business_year", business_year, OracleMappingType.Int32, ParameterDirection.Input, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_asset_manager.dpd_set_process_loan_wo", dyParam);
            return res;
        }

        public ResponseMessage SetProcess_LOAN_TARGET(int fileProcessId, int business_year, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_process_id", fileProcessId, OracleMappingType.Int32, ParameterDirection.Input, 20);
            dyParam.Add("pvc_business_year", business_year, OracleMappingType.Int32, ParameterDirection.Input, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_asset_manager.dpd_set_process_loan_target", dyParam);
            return res;
        }

        public ResponseMessage SetProcess_LOAN_CL(int fileProcessId, int business_year, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_process_id", fileProcessId, OracleMappingType.Int32, ParameterDirection.Input, 20);
            dyParam.Add("pvc_business_year", business_year, OracleMappingType.Int32, ParameterDirection.Input, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_asset_manager.dpd_set_process_loan_cl", dyParam);
            return res;
        }

        public ResponseMessage SetProcess_LOAN_PORTFOLIO(int fileProcessId, int business_year, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_process_id", fileProcessId, OracleMappingType.Int32, ParameterDirection.Input, 20);
            dyParam.Add("pvc_business_year", business_year, OracleMappingType.Int32, ParameterDirection.Input, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_asset_manager.dpd_set_process_loan_portfolio", dyParam);
            return res;
        }


    }

    #region Interface
    public interface IFileProcessRepository
    {
        IEnumerable<Account> GeAccountDetails(string customerAccountNo, string userId);
        IEnumerable<Division> GetDivisionList(string pvc_appuser);
        IEnumerable<District> GetDistrictList(string div_code, string pvc_appuser);
        int Process_LOAN_WO(List<AST_LOAN_WO_STATUS_TEMP> portFolios);
        IEnumerable<Thana> GetThanaList(string div_code, string dist_code, string pvc_appuser);
        int DeleteTable(string tableName, string user_id);
        int Process_LOAN_PORTFOLIO(List<AST_RM_PORTFOLIO_TMP> portFolios);
        int Process_LOAN_TARGET(List<AST_LOAN_TARGET_TMP> portFolios);
        int Process_LOAN_CL(List<AST_LOAN_CL_TMP> portFolios);
        ResponseMessage SetProcess_LOAN_WO(int fileProcessId, int business_year, string pvc_appuser);
        ResponseMessage SetProcess_LOAN_TARGET(int fileProcessId, int business_year, string pvc_appuser);
        ResponseMessage SetProcess_LOAN_CL(int fileProcessId, int business_year, string pvc_appuser);
        ResponseMessage SetProcess_LOAN_PORTFOLIO(int fileProcessId, int business_year, string pvc_appuser);
    }

    #endregion
}
