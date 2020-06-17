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
    public class AgentRepository : BaseRepository, IAgentRepository
    {
        public AgentRepository(OracleConnection connection) : base(connection)
        {

        }

        public IEnumerable<Account> GetAgentAccountList(string pvc_agentid, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_agentid", pvc_agentid, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_accountlist", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Account>("pkg_lov_manager.dpd_get_agentaccountlist", dyParam, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<Account> GetAgentAccounts(string pvc_agentid, string pvc_customerno, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_agentid", pvc_agentid, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_customerno", pvc_customerno, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_agentaccount", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Account>("pkg_pms_manager.dpd_get_agentaccount", dyParam, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<Agent> GetAgentDetails(string pvc_agentid, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_agentid", pvc_agentid, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_agentdtl", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Agent>("pkg_pms_manager.dpd_get_agentdtl", dyParam, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<Agent> GetAgentDetails(string pvc_agentusrid, string pvc_agentname, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_agentusrid", pvc_agentusrid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_agentname", pvc_agentname, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_agentdtl", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Agent>("pkg_search_manager.dpd_get_agentdtl", dyParam, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<AgentOutlet> GetAgentOutletDetails(string pvc_agentid, string pvc_outletid, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_agentid", pvc_agentid, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_outletid", pvc_outletid, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_outletdtl", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<AgentOutlet>("pkg_pms_manager.dpd_get_outletdtl", dyParam, commandType: CommandType.StoredProcedure);

        }

        public IEnumerable<AgentOutlet> GetAgentOutletDetails(string pvc_agentusrid, string pvc_agentname, string pvc_outletname, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_agentusrid", pvc_agentusrid, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_agentname", pvc_agentname, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_outletname", pvc_outletname, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_outletdtl", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<AgentOutlet>("pkg_search_manager.dpd_get_agentoutletdtl", dyParam, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<Agent> GetUnAuthendicatedAgentDetails(string pvc_agentid, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_agentid", pvc_agentid, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_unauthagentdtl", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Agent>("pkg_pms_manager.dpd_get_unauthagentdtl", dyParam, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<AgentOutlet> GetUnauthenticAgentOutlet(string pvc_agentid, string pvc_outletid, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_agentid", pvc_agentid, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_outletid", pvc_outletid, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pcr_unauthagentoutlet", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<AgentOutlet>("pkg_pms_manager.dpd_get_unauthagentoutlet", dyParam, commandType: CommandType.StoredProcedure);
        }

        public ResponseMessage SetAgentAccountStatus(string pvc_agentid, string pvc_custacno, string pvc_acstatus, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_agentid", pvc_agentid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_custacno", pvc_custacno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_acstatus", pvc_acstatus, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_pms_manager.dpd_set_agentaccountstatus", dyParam);
            return res;
        }

        public ResponseMessage SetAgentAuthorized(string pvc_agentid, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_agentid", pvc_agentid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_pms_manager.dpd_set_agentuthorized", dyParam);
            return res;
        }

        public ResponseMessage SetAgentDetail(Agent agent, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_agentid", agent.AGENT_ID, OracleMappingType.Varchar2, ParameterDirection.InputOutput, 20);
            dyParam.Add("pvc_agentusrid", agent.AGENT_USR_ID, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_agentname", agent.AGENT_NAME, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_sex", agent.SEX, OracleMappingType.Varchar2, ParameterDirection.Input, 1);
            dyParam.Add("pvc_custno", agent.CUSTOMER_NO, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_phone", agent.PHONE, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_mobile", agent.MOBILE, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_email", agent.EMAIL, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_tradelic", agent.TRADE_LIC, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_address", agent.ADDRESS, OracleMappingType.Varchar2, ParameterDirection.Input, 200);

            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_pms_manager.dpd_set_agentdtl", dyParam);
            return res;
        }

        public ResponseMessage SetAgentOutletAuthorized(string pvc_agentid, string pvc_outletid, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_agentid", pvc_agentid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_outletid", pvc_outletid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_pms_manager.dpd_set_agentoutletauthorized", dyParam);
            return res;
        }

        public ResponseMessage SetAgentOutletDetails(AgentOutlet agentOutlet, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_outletid", agentOutlet.OUTLET_ID, OracleMappingType.Varchar2, ParameterDirection.InputOutput, 20);
            dyParam.Add("pvc_outletname", agentOutlet.OUTLET_NAME, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_agentid", agentOutlet.AGENT_ID, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_branch", agentOutlet.BRANCH_CODE, OracleMappingType.Varchar2, ParameterDirection.Input, 3);
            dyParam.Add("pvc_account", agentOutlet.CUST_AC_NO, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_division", agentOutlet.DIV_CODE, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_district", agentOutlet.DIST_CODE, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_thana", agentOutlet.THANA_CODE, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_loctype", agentOutlet.LOC_TYPE, OracleMappingType.Varchar2, ParameterDirection.Input, 2);
            dyParam.Add("pvc_address", agentOutlet.ADDRESS, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_contactname", agentOutlet.CONTACT_NAME, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_contactmobile", agentOutlet.CONTACT_MOBILE, OracleMappingType.Varchar2, ParameterDirection.Input, 200);

            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 5);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_pms_manager.dpd_set_outletdtl", dyParam);
            return res;

        }
    }

    public interface IAgentRepository
    {
        IEnumerable<Agent> GetAgentDetails(string pvc_agentid, string pvc_appuser);
        IEnumerable<Agent> GetUnAuthendicatedAgentDetails(string pvc_agentid, string pvc_appuser);
        ResponseMessage SetAgentDetail(Agent agent, string pvc_appuser);
        IEnumerable<Agent> GetAgentDetails(string pvc_agentid, string pvc_agentname, string pvc_appuser);
        ResponseMessage SetAgentAuthorized(string pvc_agentid, string pvc_appuser);
        IEnumerable<Account> GetAgentAccounts(string pvc_agentid, string pvc_customerno, string pvc_appuser);
        ResponseMessage SetAgentAccountStatus(string pvc_agentid, string pvc_custacno, string pvc_acstatus, string pvc_appuser);
        IEnumerable<Account> GetAgentAccountList(string pvc_agentid, string pvc_appuser);
        IEnumerable<AgentOutlet> GetAgentOutletDetails(string pvc_agentid, string pvc_outletid, string pvc_appuser);
        IEnumerable<AgentOutlet> GetAgentOutletDetails(string pvc_agentid, string pvc_agentname, string pvc_outletname, string pvc_appuser);
        ResponseMessage SetAgentOutletDetails(AgentOutlet agentOutlet, string pvc_appuser);
        IEnumerable<AgentOutlet> GetUnauthenticAgentOutlet(string pvc_agentid, string pvc_outletid, string pvc_appuser);
        ResponseMessage SetAgentOutletAuthorized(string pvc_agentid, string pvc_outletid, string pvc_appuser);
    }
}
