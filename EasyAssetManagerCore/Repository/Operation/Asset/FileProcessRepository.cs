using Dapper;
using Dapper.Oracle;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Common;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

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


        public IEnumerable<AST_LOAN_WO_STATUS_TEMP> Getloanwo(string loan_number,string are_code,string branch_code,string wo_date, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_area_code", are_code, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_branch_code", branch_code, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_loan_number", loan_number, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_wo_date", wo_date, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);

            dyParam.Add("pcr_loanwo", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<AST_LOAN_WO_STATUS_TEMP>("pkg_asset_manager.dpd_get_loan_wo", dyParam, commandType: CommandType.StoredProcedure);
        }
        public ResponseMessage Set_LOAN_WO(AST_LOAN_WO_STATUS_TEMP loan_wo, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_area_code", loan_wo.AREA_CODE, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_branch_code", loan_wo.BRANCH_CODE, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_osamount", loan_wo.OS_AMOUNT, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_woamount", loan_wo.WO_AMOUNT, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_wodate",loan_wo.WO_DATE.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_seg_id", loan_wo.SEG_ID, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_loan_number", loan_wo.LOAN_AC_NUMBER, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_product_code", loan_wo.PRODUCT_CODE, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_asset_manager.dpd_set_loan_wo", dyParam);
            return res;
        }

        public IEnumerable<AST_LOAN_CL_TMP> Getloancl(string loan_number, string cl_status,string eff_date, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_loan_number", loan_number, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_cl_status", cl_status, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_eff_date", eff_date, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);

            dyParam.Add("pcr_loancl", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<AST_LOAN_CL_TMP>("pkg_asset_manager.dpd_get_loan_cl", dyParam, commandType: CommandType.StoredProcedure);
        }
        public ResponseMessage Set_LOAN_CL(AST_LOAN_CL_TMP loan_wo, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_loan_number", loan_wo.LOAN_AC_NUMBER, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_cl_status", loan_wo.CL_STATUS, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_effdate", loan_wo.EFF_DATE.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) , OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_asset_manager.dpd_set_loan_cl", dyParam);
            return res;
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
        IEnumerable<AST_LOAN_WO_STATUS_TEMP> Getloanwo(string loan_number, string are_code, string branch_code, string wo_date, string pvc_appuser);
        ResponseMessage Set_LOAN_WO(AST_LOAN_WO_STATUS_TEMP loan_wo, string pvc_appuser);
        IEnumerable<AST_LOAN_CL_TMP> Getloancl(string loan_number, string cl_status, string eff_date, string pvc_appuser);
        ResponseMessage Set_LOAN_CL(AST_LOAN_CL_TMP loan_wo, string pvc_appuser);
        int Process_LOAN_WO(List<AST_LOAN_WO_STATUS_TEMP> portFolios);
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
