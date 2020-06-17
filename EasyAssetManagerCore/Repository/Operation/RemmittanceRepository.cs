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
    public class RemmittanceRepository: BaseRepository, IRemmittanceRepository
    {
        public RemmittanceRepository(OracleConnection connection):base(connection)
        {

        }
        public IEnumerable<Remittance> GetRemitCompanyDetails(string pvc_remitcompid, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_remitcompid", pvc_remitcompid, OracleMappingType.Varchar2, ParameterDirection.Input, 3);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pcr_remitcompanydtl", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Remittance>("pkg_pms_manager.dpd_get_remitcompanydtl", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<Remittance> GetRemitCompanyDetails(string pvc_remitcompid,string pvc_companyname, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_remitcompid", pvc_remitcompid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_companyname", pvc_companyname, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pcr_remitcompanydtl", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Remittance>("pkg_search_manager.dpd_get_remitcompanydtl", dyParam, commandType: CommandType.StoredProcedure);
        }
        public ResponseMessage SetRemitCompanyDetails(string pvc_remitcompid, string pvc_remitcompname, string pvc_remitcompacno, string pvc_remitcompstatus, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_remitcompid", pvc_remitcompid, OracleMappingType.Varchar2, ParameterDirection.InputOutput, 3);
            dyParam.Add("pvc_remitcompname", pvc_remitcompname, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_remitcompacno", pvc_remitcompacno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_remitcompstatus", pvc_remitcompstatus, OracleMappingType.Varchar2, ParameterDirection.Input, 1);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_pms_manager.dpd_set_remitcompanydtl", dyParam);
            return res;
        }
        public ResponseMessage DelRemitCompanyDetails(string pvc_remitcompid, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_remitcompid", pvc_remitcompid, OracleMappingType.Varchar2, ParameterDirection.InputOutput, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_pms_manager.dpd_del_remitcompanydtl", dyParam);
            return res;
        }
        public IEnumerable<Remittance> GetUnauthoRemittanceCompany(string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pcr_unauthremitcompany", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Remittance>("pkg_pms_manager.dpd_get_unauthremitcompany", dyParam, commandType: CommandType.StoredProcedure);
        }
        public ResponseMessage SetRemittanceCompanyAuthorized(string pvc_remitcompid, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_remitcompid", pvc_remitcompid, OracleMappingType.Varchar2, ParameterDirection.InputOutput, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_pms_manager.dpd_set_remitcompanyauthorized", dyParam);
            return res;
        }
    }
    public interface IRemmittanceRepository 
    {
        IEnumerable<Remittance> GetRemitCompanyDetails(string pvc_remitcompid, string pvc_appuser);
        IEnumerable<Remittance> GetRemitCompanyDetails(string pvc_remitcompid, string pvc_remitcompname, string pvc_appuser);
        ResponseMessage SetRemitCompanyDetails(string pvc_remitcompid, string pvc_remitcompname, string pvc_remitcompacno, string pvc_remitcompstatus, string pvc_appuser);
        ResponseMessage DelRemitCompanyDetails(string pvc_remitcompid, string pvc_appuser);
        IEnumerable<Remittance> GetUnauthoRemittanceCompany(string pvc_appuser);
        ResponseMessage SetRemittanceCompanyAuthorized(string pvc_remitcompid, string pvc_appuser);
    }
}
