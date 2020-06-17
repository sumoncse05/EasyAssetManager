using Dapper;
using Dapper.Oracle;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Common;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;

namespace EasyAssetManagerCore.Repository.Operation
{
    public class AccountRepository:BaseRepository, IAccountRepository
    {
        public AccountRepository(OracleConnection connection) : base(connection)
        {

        }
        public IEnumerable<Account> GeAccountDetails(string customerAccountNo,string userId)
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
    }

    #region Interface
    public interface IAccountRepository
    {
        IEnumerable<Account> GeAccountDetails(string customerAccountNo, string userId);
        IEnumerable<Division> GetDivisionList(string pvc_appuser);
        IEnumerable<District> GetDistrictList(string div_code, string pvc_appuser);
        IEnumerable<Thana> GetThanaList(string div_code, string dist_code, string pvc_appuser);
    }

    #endregion
}
