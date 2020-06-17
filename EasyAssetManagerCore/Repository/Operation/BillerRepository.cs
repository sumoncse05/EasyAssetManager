using Dapper;
using Dapper.Oracle;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Common;
using Oracle.ManagedDataAccess.Client;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EasyAssetManagerCore.Repository.Operation
{
    public class BillerRepository: BaseRepository, IBillerRepository
    {
        public BillerRepository(OracleConnection connection):base(connection)
        {

        }
        public IEnumerable<Biller> GetBillerDetails(string pvc_billerid, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_billerid", pvc_billerid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pcr_billerdetails", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Biller>("pkg_pms_manager.dpd_get_billerdetails", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<Biller> GetBillerDetails(string pvc_billerid,string pvc_billerdesc, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_billerid", pvc_billerid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_billerdesc", pvc_billerdesc, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pcr_billerdetails", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Biller>("pkg_search_manager.dpd_get_billerdetails", dyParam, commandType: CommandType.StoredProcedure);
        }
        public ResponseMessage SetBillerDetails(string pvc_billerid, string pvc_billerdesc, string pvc_billeracno, string pvc_billerstatus, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_billerid", pvc_billerid, OracleMappingType.Varchar2, ParameterDirection.InputOutput, 20);
            dyParam.Add("pvc_billerdesc", pvc_billerdesc, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_billeracno", pvc_billeracno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_billerstatus", pvc_billerstatus, OracleMappingType.Varchar2, ParameterDirection.Input, 1);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_pms_manager.dpd_set_billerdetails", dyParam);
            return res;
        }

        public ResponseMessage DelBillerDetails(string pvc_billerid, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_billerid", pvc_billerid, OracleMappingType.Varchar2, ParameterDirection.InputOutput, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_pms_manager.dpd_del_billerdetails", dyParam);
            return res;
        }
        public IEnumerable<Biller> GetUnauthoBillerDetails( string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pcr_unauthbiller", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Biller>("pkg_pms_manager.dpd_get_unauthbiller", dyParam, commandType: CommandType.StoredProcedure);
        }
        public ResponseMessage SetBillerAuthorized(string pvc_billerid, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_billerid", pvc_billerid, OracleMappingType.Varchar2, ParameterDirection.InputOutput, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_pms_manager.dpd_set_billerauthorized", dyParam);
            return res;
        }
    }

    public interface IBillerRepository 
    {
        IEnumerable<Biller> GetBillerDetails(string pvc_custacno, string pvc_appuser);
        IEnumerable<Biller> GetBillerDetails(string pvc_billerid, string pvc_billerdesc, string pvc_appuser);
        ResponseMessage SetBillerDetails(string pvc_billerid, string pvc_billerdesc, string pvc_billeracno, string pvc_billerstatus, string pvc_appuser);
        ResponseMessage DelBillerDetails(string pvc_billerid, string pvc_appuser);
        IEnumerable<Biller> GetUnauthoBillerDetails(string pvc_appuser);
        ResponseMessage SetBillerAuthorized(string pvc_billerid, string pvc_appuser);
    }
}
