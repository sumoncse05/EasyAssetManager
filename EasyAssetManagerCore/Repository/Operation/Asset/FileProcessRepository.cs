using Dapper;
using Dapper.Oracle;
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

        public int Process_LOAN_PORTFOLIO(List<AST_LOAN_PORTFOLIO_TMP> portFolios)
        {
            var sql = @"insert into ERMP.AST_LOAN_PORTFOLIO_TMP (File_Process_ID,ID_of_Area,Name_of_Area,Brn_Code,Branch_Name,ID_of_RM,Name_of_RM,Loan_Acct_No,INS_BY,INS_DATE)
                      values (:File_Process_ID,:ID_of_Area,:Name_of_Area,:Brn_Code,:Branch_Name,:ID_of_RM,:Name_of_RM,:Loan_Acct_No,:INS_BY,:INS_DATE)";
            var affectedRows = Connection.Execute(sql, portFolios);
            return affectedRows;
        }
    }

    #region Interface
    public interface IFileProcessRepository
    {
        IEnumerable<Account> GeAccountDetails(string customerAccountNo, string userId);
        IEnumerable<Division> GetDivisionList(string pvc_appuser);
        IEnumerable<District> GetDistrictList(string div_code, string pvc_appuser);
        IEnumerable<Thana> GetThanaList(string div_code, string dist_code, string pvc_appuser);
        int DeleteTable(string tableName, string user_id);
        int Process_LOAN_PORTFOLIO(List<AST_LOAN_PORTFOLIO_TMP> portFolios);
    }

    #endregion
}
