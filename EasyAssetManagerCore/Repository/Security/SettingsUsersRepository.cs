using Dapper;
using Dapper.Oracle;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
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
        

        public SettingsUsers GetUserByUserName(string userName)
        {
            string query = QB<SettingsUsers>.Select(o => o.branch_name == o.branch_name);
            if (Transaction != null)
            {
                return Connection.Query<SettingsUsers>(query, new
                {
                    UserName = userName
                }, transaction: Transaction).FirstOrDefault();
            }

            return Connection.Query<SettingsUsers>(query, new
            {
                UserName = userName
            }).FirstOrDefault();
        }

        public bool CheckUserName(string userName, int userId)
        {
            string query = QB<SettingsUsers>.Select(o => o.branch_name == o.branch_name && o.user_id != o.user_id);
            if (Transaction != null)
            {
                return Connection.Query<SettingsUsers>(query, new
                {
                    UserName = userName,
                    UserId = userId
                }, transaction: Transaction).Count() > 0;
            }

            return Connection.Query<SettingsUsers>(query, new
            {
                UserName = userName,
                UserId = userId
            }).Count() > 0;
        }

        public IEnumerable<Screen> GetScreens(string userId)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_userid", userId, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pcr_useramenu", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Screen>("pkg_sec_user.dpd_get_usermenualternet", dyParam, commandType: CommandType.StoredProcedure);

        }

        public int Insert(SettingsUsers user)
        {
            throw new System.NotImplementedException();
        }

        public SettingsUsers Get(int userId)
        {
            throw new System.NotImplementedException();
        }

        public int Update(SettingsUsers olduser)
        {
            throw new System.NotImplementedException();
        }

        public SettingsUsers Get(string userName)
        {
            throw new System.NotImplementedException();
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
        public IEnumerable<Module> GetUserModules(string user_Id)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_userid", user_Id, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_usermodule", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Module>("pkg_sec_user.dpd_get_usermodule", dyParam, commandType: CommandType.StoredProcedure);
        }

        public ResponseMessage SetUserPassword(string pvc_userid, string pvc_useroldpass,
                                          string pvc_usernewpass, string pvc_resetcode, string pvc_expdays,
                                          string pvc_stationip, string pvc_sessionid,
                                          string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_userid", pvc_userid, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_useroldpass", pvc_useroldpass, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_usernewpass", pvc_usernewpass, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_resetcode", pvc_resetcode, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_expdays", pvc_expdays, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_stationip", pvc_stationip, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_sessionid", pvc_sessionid, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_sec_user.dpd_set_userpass", dyParam);
            return res;
  
        }

        public UserRole GetRole(string pvc_roleid, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_roleid", pvc_roleid, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_roledtl", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<UserRole>("pkg_sec_role.dpd_get_roledtl", dyParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
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

        public IEnumerable<UserType> GetUserTypeList(string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_usertypelist", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<UserType>("pkg_lov_manager.dpd_get_limitusertypelist", dyParam, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<UserRole> GetRoleList(string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_rolelist", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<UserRole>("pkg_sec_role.dpd_get_rolelist", dyParam, commandType: CommandType.StoredProcedure);
        }

        public ResponseMessage SaveRole(string pvc_roleid, string pvc_roledesc, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_roleid", pvc_roleid, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_roledesc", pvc_roledesc, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_sec_role.dpd_set_roledtl", dyParam);
            return res;
        }

        public ResponseMessage SaveScreenAccessPermission(string pvc_roleid, string[] pvc_scrid, string[] pvc_updexp, string[] pvc_delprn, string[] pvc_viwviw, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_roleid", pvc_roleid, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_scrid", pvc_scrid, OracleMappingType.Varchar2, ParameterDirection.Input, pvc_scrid.Length, null, null, null, null, null, OracleMappingCollectionType.PLSQLAssociativeArray);
            dyParam.Add("pvc_updexp", pvc_updexp, OracleMappingType.Varchar2, ParameterDirection.Input, pvc_updexp.Length,null,null,null,null,null,OracleMappingCollectionType.PLSQLAssociativeArray);
            dyParam.Add("pvc_delprn", pvc_delprn, OracleMappingType.Varchar2, ParameterDirection.Input, pvc_delprn.Length, null, null, null, null, null, OracleMappingCollectionType.PLSQLAssociativeArray);
            dyParam.Add("pvc_viwviw", pvc_viwviw, OracleMappingType.Varchar2, ParameterDirection.Input, pvc_viwviw.Length, null, null, null, null, null, OracleMappingCollectionType.PLSQLAssociativeArray);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_sec_role.dpd_set_scrprivilege", dyParam);
            return res;
        }

        public ResponseMessage DeleteRole(string pvc_roleid, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_roleid", pvc_roleid, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_sec_role.dpd_del_roledtl", dyParam);
            return res;
        }

        public IEnumerable<ScreenAccessPermission> GetInterfaceList(string pvc_modid, string pvc_scrid, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_scrid", pvc_scrid, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_modid", pvc_modid, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_interfacedtl", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<ScreenAccessPermission>("pkg_sec_role.dpd_get_interfacedtl", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<User> GetUnAuthorizeUserList(string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_unauthusers", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<User>("pkg_sec_user.dpd_get_unauthusers", dyParam, commandType: CommandType.StoredProcedure);
        }
        public ResponseMessage AuthUsers(string[] pvc_userid, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_userid", pvc_userid, OracleMappingType.Varchar2, ParameterDirection.Input, pvc_userid.Length, null, null, null, null, null, OracleMappingCollectionType.PLSQLAssociativeArray);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_sec_user.dpd_set_usersauthorized", dyParam);
            return res;
        }
        public IEnumerable<UserRole> UnauthUserRole(string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_unauthuserrole", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<UserRole>("pkg_sec_user.dpd_get_unauthuserrole", dyParam, commandType: CommandType.StoredProcedure);
        }
        public ResponseMessage AuthUsersRole(string pvc_roleslno, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_roleslno", pvc_roleslno, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_sec_user.dpd_set_userroleauthorized", dyParam);
            return res;
        }
        public IEnumerable<User> PasswordEmailList(string pvc_rstslno,string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters(); 
            dyParam.Add("pvc_rstslno", pvc_rstslno, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_resetemailuser", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<User>("pkg_sec_user.dpd_get_resetemailuser", dyParam, commandType: CommandType.StoredProcedure);
        }
        public ResponseMessage SetPasswordEmailStatus(string pvc_rstslno, string pvc_userid, string pvc_emailstatus, string pvc_emailerrmsg, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_rstslno", pvc_rstslno, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_userid", pvc_userid, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_emailstatus", pvc_emailstatus, OracleMappingType.Varchar2, ParameterDirection.Input, 1);
            dyParam.Add("pvc_emailerrmsg", pvc_emailerrmsg, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_sec_user.dpd_set_resetemailstatus", dyParam);
            return res;
        }
        public IEnumerable<User> GetInactiveUser(string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_inactiveuser", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<User>("pkg_sec_user.dpd_get_inactiveuser", dyParam, commandType: CommandType.StoredProcedure);
        }
        public ResponseMessage UpdateUserStatus(string pvc_actvslno, string pvc_userid, string pvc_activecode, string pvc_activereason, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_actvslno", pvc_actvslno, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_userid", pvc_userid, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_activecode", pvc_activecode, OracleMappingType.Varchar2, ParameterDirection.Input, 1);
            dyParam.Add("pvc_activereason", pvc_activereason, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_sec_user.dpd_set_useractive", dyParam);
            return res;
        }
        public IEnumerable<SettingsUsers> GetActiveUsers(string pvc_usertype, string pvc_userid, string pvc_username, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_usertype", pvc_usertype, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_userid", pvc_userid, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_username", pvc_username, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_activeusers", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<SettingsUsers>("pkg_sec_user.dpd_get_activeusers", dyParam, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<SettingsUsers> GetUnauthResetPasswordUsers(string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_resetpassuser", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<SettingsUsers>("pkg_sec_user.dpd_get_resetpassuser", dyParam, commandType: CommandType.StoredProcedure);
        }

        public ResponseMessage RequestResetPassword(string pvc_userid, string pvc_resetcode, string pvc_resetrefno, string pvc_resetreason, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_userid", pvc_userid, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_resetcode", pvc_resetcode, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_resetrefno", pvc_resetrefno, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_resetreason", pvc_resetreason, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_sec_user.dpd_set_passresetrequest", dyParam);
            return res;
        }
        public ResponseMessage AuthorizeResetPassword(string pvc_rstslno, string pvc_userid, string pvc_resetcode, string pvc_userpass, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_rstslno", pvc_rstslno, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_userid", pvc_userid, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_resetcode", pvc_resetcode, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_userpass", pvc_userpass, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_sec_user.dpd_set_resetuserpass", dyParam);
            return res;
        }
        public IEnumerable<User> LoogedinUsers(string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pcr_sessiondtl", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<User>("pkg_sec_user.dpd_get_sessiondtl", dyParam, commandType: CommandType.StoredProcedure);
        }
        public ResponseMessage ClearLoginSession(string pvc_userid, string pvc_stationip, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_userid", pvc_userid, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_stationip", pvc_stationip, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_sec_user.dpd_set_sessionclr", dyParam);
            return res;
        }
        public IEnumerable<User> GetUserList(string pvc_usertype, string pvc_userid, string pvc_username, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_usertype", pvc_usertype, OracleMappingType.Varchar2, ParameterDirection.Input, 2);
            dyParam.Add("pvc_userid", pvc_userid, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_username", pvc_username, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_userlist", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<User>("pkg_sec_user.dpd_get_userlist", dyParam, commandType: CommandType.StoredProcedure);
        }
        public ResponseMessage SetUserInactive(string pvc_userid, string pvc_inactivereason, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_userid", pvc_userid, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_inactivereason", pvc_inactivereason, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_sec_user.dpd_set_userinactive", dyParam);
            return res;
        }

        public ResponseMessage DeleteUser(string pvc_userid, string pvc_appuser)
        {

            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();

            dyParam.Add("pvc_userid", pvc_userid, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_sec_user.dpd_del_userdtl", dyParam);
            return res;
        }
        public User GetUser(string pvc_userid, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_userid", pvc_userid, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pcr_userdtl", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<User>("pkg_sec_user.dpd_get_userdtl", dyParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
        }

        public IEnumerable<User> CmdUserList(string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_userlist", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<User>("pkg_lov_manager.dpd_get_userlist", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<UserRole> GetFreeRole(string pvc_userid, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_userid", pvc_userid, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_freerole", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<UserRole>("pkg_sec_user.dpd_get_freerole", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<UserRole> GetAssignedRole(string pvc_userid, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_userid", pvc_userid, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_userrole", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<UserRole>("pkg_sec_user.dpd_get_userrole", dyParam, commandType: CommandType.StoredProcedure);
        }
        public ResponseMessage SetUserRole(string pvc_userid, string pvc_roleid, string pvc_action, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();

            dyParam.Add("pvc_userid", pvc_userid, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_roleid", pvc_roleid, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_action", pvc_action, OracleMappingType.Varchar2, ParameterDirection.Input, 1);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_sec_user.dpd_set_userrole", dyParam);
            return res;
        }
    }


    #region Interface
    public interface ISettingsUsersRepository
    {
        SettingsUsers GetUserByUserName(string userName);
        bool CheckUserName(string userName, int userId);
        int Insert(SettingsUsers user);
        SettingsUsers Get(int userId);
        int Update(SettingsUsers olduser);
        SettingsUsers Get(string userName);
        IEnumerable<Screen> GetScreens(string userId);
        SettingsUsers GetLoginInfo(string userName, string password, string stationId, string sessionId);
        IEnumerable<Module> GetUserModules(string user_Id);
        ResponseMessage SetUserPassword(string pvc_userid, string pvc_useroldpass,
                                         string pvc_usernewpass, string pvc_resetcode, string pvc_expdays,
                                         string pvc_stationip, string pvc_sessionid,
                                         string pvc_appuser);
        UserRole GetRole(string pvc_roleid, string pvc_appuser);
        IEnumerable<UserRole> GetRoleList(string pvc_appuser);
        IEnumerable<ScreenAccessPermission> GetRoleWisePermission(string pvc_roleid, string pvc_modid, string pvc_appuser);
        ResponseMessage SaveRole(string pvc_roleid, string pvc_roledesc, string pvc_appuser);
        ResponseMessage SaveScreenAccessPermission(string pvc_roleid, string[] pvc_scrid,
                                            string[] pvc_updexp, string[] pvc_delprn,
                                            string[] pvc_viwviw, string pvc_appuser);
        ResponseMessage DeleteRole(string pvc_roleid, string pvc_appuser);
        IEnumerable<ScreenAccessPermission> GetInterfaceList(string pvc_modid, string pvc_scrid, string pvc_appuser);
        IEnumerable<User> GetUnAuthorizeUserList(string pvc_appuser);
        ResponseMessage AuthUsers(string[] pvc_userid, string pvc_appuser);
        IEnumerable<UserRole> UnauthUserRole(string pvc_appuser);
        ResponseMessage AuthUsersRole(string pvc_roleslno, string pvc_appuser);
        IEnumerable<User> PasswordEmailList(string pvc_rstslno, string pvc_appuser);
        ResponseMessage SetPasswordEmailStatus(string pvc_rstslno, string pvc_userid, string pvc_emailstatus, string pvc_emailerrmsg, string pvc_appuser);
        IEnumerable<User> GetInactiveUser(string pvc_appuser);
        ResponseMessage UpdateUserStatus(string pvc_actvslno, string pvc_userid, string pvc_activecode, string pvc_activereason, string pvc_appuser);
        IEnumerable<SettingsUsers> GetActiveUsers(string pvc_usertype, string pvc_userid, string pvc_username, string pvc_appuser);
        ResponseMessage RequestResetPassword(string pvc_userid, string pvc_resetcode,
                                              string pvc_resetrefno, string pvc_resetreason,
                                              string pvc_appuser);
        IEnumerable<SettingsUsers> GetUnauthResetPasswordUsers(string pvc_appuser);
        ResponseMessage AuthorizeResetPassword(string pvc_rstslno, string pvc_userid, string pvc_resetcode, string pvc_userpass, string pvc_appuser);
        IEnumerable<UserType> GetUserTypeList(string pvc_appuser);
        IEnumerable<User> LoogedinUsers(string pvc_appuser);
        ResponseMessage ClearLoginSession(string pvc_userid, string pvc_stationip, string pvc_appuser);
        IEnumerable<User> GetUserList(string pvc_usertype, string pvc_userid, string pvc_username, string pvc_appuser);
        ResponseMessage SetUserInactive(string pvc_userid, string pvc_inactivereason, string pvc_appuser);
        ResponseMessage DeleteUser(string pvc_userid, string pvc_appuser);
        User GetUser(string pvc_userid, string pvc_appuser);
        IEnumerable<User> CmdUserList(string pvc_appuser);
        IEnumerable<UserRole> GetFreeRole(string pvc_userid, string pvc_appuser);
        IEnumerable<UserRole> GetAssignedRole(string pvc_userid, string pvc_appuser);
        ResponseMessage SetUserRole(string pvc_userid, string pvc_roleid, string pvc_action, string pvc_appuser);
    }

    #endregion

}
