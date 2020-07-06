using Dapper;
using Dapper.Oracle;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Common;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EasyAssetManagerCore.Repository.Security
{
    public class SettingsUsersRepository : BaseRepository, ISettingsUsersRepository
    {
        public SettingsUsersRepository(OracleConnection connection) : base(connection)
        {
        }
        public IEnumerable<Screen> GetScreens(string userId)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_userid", userId, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pcr_useramenu", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Screen>("pkg_sec_user.dpd_get_usermenualternet", dyParam, commandType: CommandType.StoredProcedure);

        }
        public SettingsUsers GetLoginInfo(string userName, string password, string stationId, string sessionId)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_userid", userName, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_userpass", password, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_stationip", stationId, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_sessionid", sessionId, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_logindtl", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<SettingsUsers>("pkg_sec_user.dpd_get_logindtl", dyParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
        }
        public IEnumerable<ScreenAccessPermission> GetRoleWisePermission(string pvc_roleid, string pvc_modid, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_roleid", pvc_roleid, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_modid", pvc_modid, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_scrprivilege", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<ScreenAccessPermission>("pkg_sec_role.dpd_get_scrprivilege", dyParam, commandType: CommandType.StoredProcedure);
        }

    }


    #region Interface
    public interface ISettingsUsersRepository
    {
        IEnumerable<Screen> GetScreens(string userId);
        SettingsUsers GetLoginInfo(string userName, string password, string stationId, string sessionId);
        IEnumerable<ScreenAccessPermission> GetRoleWisePermission(string pvc_roleid, string pvc_modid, string pvc_appuser);
        
    }

    #endregion

}
