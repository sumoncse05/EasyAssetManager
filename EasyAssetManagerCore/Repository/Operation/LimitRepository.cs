using Dapper;
using Dapper.Oracle;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Common;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;

namespace EasyAssetManagerCore.Repository.Operation
{
    public class LimitRepository : BaseRepository, ILimitRepository
    {
        public LimitRepository(OracleConnection connection) : base(connection)
        {
            
        }

        public ResponseMessage AuthorizeLimitPackage(List<string> pvc_limitid, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_limitid", pvc_limitid.ToArray(), OracleMappingType.Varchar2, ParameterDirection.Input, pvc_limitid.Count, null, null, null, null, null, OracleMappingCollectionType.PLSQLAssociativeArray);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_limit_manager.dpd_set_limitpackageauthorized", dyParam);
            return res;
        }

        public ResponseMessage AuthorizeTransLimit(List<string> pvc_limitslno, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_limitslno", pvc_limitslno.ToArray(), OracleMappingType.Varchar2, ParameterDirection.Input,pvc_limitslno.Count, null, null, null, null, null, OracleMappingCollectionType.PLSQLAssociativeArray);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_limit_manager.dpd_set_translimitauthorized", dyParam);
            return res;
            
        }

        public ResponseMessage DeleteLimitPackage(string pvc_limitid, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_limitid", pvc_limitid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_limit_manager.dpd_del_limitpackage", dyParam);
            return res;
           
        }

        public ResponseMessage DeleteTransactionLimit(string pvc_limitslno, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_limitslno", pvc_limitslno, OracleMappingType.Varchar2, ParameterDirection.InputOutput, 50);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_limit_manager.dpd_del_translimitpackage", dyParam);
            return res;
            
        }

        public IEnumerable<LimitPackage> GetLimitDetails(string pvc_limitid, string pvc_transtype, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_limitid", pvc_limitid, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_transtype", pvc_transtype, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_limitdetails", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<LimitPackage>("pkg_limit_manager.dpd_get_limitdetails", dyParam, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<LimitPackage> GetLimitPackages(string pvc_limitid, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_limitid", pvc_limitid, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_limitpackages", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<LimitPackage>("pkg_limit_manager.dpd_get_limitpackages", dyParam, commandType: CommandType.StoredProcedure); 
        }

        public IEnumerable<LimitPackage> GetLimitPackageList(string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_limitpackagelist", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<LimitPackage>("pkg_lov_manager.dpd_get_limitpackagelist", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<LimitPackage> GetUnauthLimitPackages(string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_unauthlimitpackage", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<LimitPackage>("pkg_limit_manager.dpd_get_unauthlimitpackage", dyParam, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<LimitPackage> GetUnauthTransLimit(string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_unauthtranslimit", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<LimitPackage>("pkg_limit_manager.dpd_get_unauthtranslimit", dyParam, commandType: CommandType.StoredProcedure);
        }

        public ResponseMessage SetLimitPackage(LimitPackage limitPackage, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            var transtype = new List<string> { "00","01","02","05" };
            var maxtransno = new List<string> { limitPackage.maxtransno00, limitPackage.maxtransno01, limitPackage.maxtransno02, limitPackage.maxtransno05 };
            var maxtransamt = new List<string> { limitPackage.maxtransamt00, limitPackage.maxtransamt01, limitPackage.maxtransamt02, limitPackage.maxtransamt05 };
            var tottransamt = new List<string> { limitPackage.tottransamt00, limitPackage.tottransamt01, limitPackage.tottransamt02, limitPackage.tottransamt05 };
            dyParam.Add("pvc_limitid", limitPackage.limit_id, OracleMappingType.Varchar2, ParameterDirection.InputOutput, 20);
            dyParam.Add("pvc_limitdesc", limitPackage.limit_desc, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_remarks", limitPackage.remarks, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_transtype", transtype.ToArray(), OracleMappingType.Varchar2, ParameterDirection.Input, transtype.Count, null, null, null, null, null, OracleMappingCollectionType.PLSQLAssociativeArray);
            dyParam.Add("pvc_maxtransno", maxtransno.ToArray(), OracleMappingType.Varchar2, ParameterDirection.Input, maxtransno.Count, null, null, null, null, null, OracleMappingCollectionType.PLSQLAssociativeArray);
            dyParam.Add("pvc_maxtransamt", maxtransamt.ToArray(), OracleMappingType.Varchar2, ParameterDirection.Input, maxtransamt.Count, null, null, null, null, null, OracleMappingCollectionType.PLSQLAssociativeArray);
            dyParam.Add("pvc_tottransamt", tottransamt.ToArray(), OracleMappingType.Varchar2, ParameterDirection.Input, tottransamt.Count, null, null, null, null, null, OracleMappingCollectionType.PLSQLAssociativeArray);

            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_limit_manager.dpd_set_limitpackage", dyParam);
            return res;
        }

        public ResponseMessage SetTransLimitPackage(LimitPackage limitPackage, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_limitslno", limitPackage.limit_slno, OracleMappingType.Varchar2, ParameterDirection.InputOutput, 50);
            dyParam.Add("pvc_limitscope", limitPackage.limit_scope, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_limitfrequency", limitPackage.limit_frequency, OracleMappingType.Varchar2, ParameterDirection.Input, 2);
            dyParam.Add("pvc_usertype", limitPackage.usertype, OracleMappingType.Varchar2, ParameterDirection.Input, 2);
            dyParam.Add("pvc_userrefno", limitPackage.user_ref_no, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_limitid", limitPackage.limit_id, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_actypecode", limitPackage.ac_type_desc, OracleMappingType.Varchar2, ParameterDirection.Input, 2);
            dyParam.Add("pvc_effdate", limitPackage.eff_date, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_expdate", limitPackage.remarks, OracleMappingType.Varchar2, ParameterDirection.Input, 10);

            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_limit_manager.dpd_set_translimitpackage", dyParam);
            return res;
        }
    }

    public interface ILimitRepository
    {
        IEnumerable<LimitPackage> GetLimitPackages(string pvc_limitid, string pvc_appuser);
        IEnumerable<LimitPackage> GetLimitDetails(string pvc_limitid, string pvc_transtype, string pvc_appuser);
        ResponseMessage SetLimitPackage(LimitPackage limitPackage, string pvc_appuser);
        ResponseMessage DeleteLimitPackage(string pvc_limitid, string pvc_appuser);
        IEnumerable<LimitPackage> GetUnauthLimitPackages(string pvc_appuser);
        ResponseMessage AuthorizeLimitPackage(List<string> pvc_limitid, string pvc_appuser);
        ResponseMessage SetTransLimitPackage(LimitPackage limitPackage, string pvc_appuser);
        ResponseMessage DeleteTransactionLimit(string pvc_limitslno, string pvc_appuser);
        IEnumerable<LimitPackage>  GetUnauthTransLimit(string pvc_appuser);
        ResponseMessage AuthorizeTransLimit(List<string> pvc_limitslno, string pvc_appuser);
        IEnumerable<LimitPackage> GetLimitPackageList(string pvc_appuser);
    }
}
