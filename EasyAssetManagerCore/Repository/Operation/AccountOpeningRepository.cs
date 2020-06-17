using Dapper;
using Dapper.Oracle;
using EasyAssetManagerCore.BusinessLogic.Operation;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Common;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EasyAssetManagerCore.Repository.Operation
{
    public class AccountOpeningRepository : BaseRepository, IAccountOpeningRepository
    {
        public AccountOpeningRepository(OracleConnection connection) : base(connection)
        {

        }
        public ResponseMessage CmdInitiateAccountOpening(string pvc_acregslno, string pvc_accusttype, string pvc_jointacind, string pvc_noofcust,
                                                         string pvc_acname, string pvc_chequebook, string pvc_debitcard, string pvc_remarks,
                                                         string pvc_appuser, string pvc_agentid, string pvc_stationip)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();

            dyParam.Add("pvc_acregslno", pvc_acregslno, OracleMappingType.Varchar2, ParameterDirection.InputOutput, 20);
            dyParam.Add("pvc_accusttype ", pvc_accusttype, OracleMappingType.Varchar2, ParameterDirection.Input, 1);
            dyParam.Add("pvc_jointacind ", pvc_jointacind, OracleMappingType.Varchar2, ParameterDirection.Input, 1);
            dyParam.Add("pvc_noofcust", pvc_noofcust, OracleMappingType.Varchar2, ParameterDirection.Input, 2);
            dyParam.Add("pvc_acname ", pvc_acname, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_chequebook ", pvc_chequebook, OracleMappingType.Varchar2, ParameterDirection.Input, 1);
            dyParam.Add("pvc_debitcard ", pvc_debitcard, OracleMappingType.Varchar2, ParameterDirection.Input, 1);
            dyParam.Add("pvc_remarks", pvc_remarks, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_agentid", pvc_agentid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_stationip", pvc_stationip, OracleMappingType.Varchar2, ParameterDirection.Input, 100);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_pms_manager.dpd_init_accountopening", dyParam);
            return res;
        }
        public IEnumerable<WorkFlowType> GetWorkflowType(string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);

            dyParam.Add("pcr_workflowtype", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<WorkFlowType>("pkg_lov_manager.dpd_get_workflowtype", dyParam, commandType: CommandType.StoredProcedure);
        }

        public ResponseMessage InitWorkflowDtl(string pvc_wfslno, string pvc_wftype, string pvc_custno, string pvc_custacno, string pvc_cardno, string pvc_acdesc, string pvc_custname,
                                         string pvc_fathername, string pvc_mothername, string pvc_dob,
                                         string pvc_sex, string pvc_nid, string pvc_mobile, string pvc_email,
                                         string pvc_amount, string pvc_remarks, string pvc_appuser, string pvc_agentid,
                                         string pvc_stationip)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();

            dyParam.Add("pvc_wfslno", pvc_wfslno, OracleMappingType.Varchar2, ParameterDirection.InputOutput, 20);
            dyParam.Add("pvc_wftype ", pvc_wftype, OracleMappingType.Varchar2, ParameterDirection.Input, 2);
            dyParam.Add("pvc_custno ", pvc_custno, OracleMappingType.Varchar2, ParameterDirection.Input, 9);
            dyParam.Add("pvc_custacno", pvc_custacno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_cardno", pvc_cardno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_acdesc ", pvc_acdesc, OracleMappingType.Varchar2, ParameterDirection.Input, 110);
            dyParam.Add("pvc_custname ", pvc_custname, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_fathername ", pvc_fathername, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_mothername ", pvc_mothername, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_dob ", pvc_dob, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_sex", pvc_sex, OracleMappingType.Varchar2, ParameterDirection.Input, 1);
            dyParam.Add("pvc_nid", pvc_nid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_mobile", pvc_mobile, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_email", pvc_email, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_amount", pvc_amount, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_remarks", pvc_remarks, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_agentid", pvc_agentid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_stationip", pvc_stationip, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_workflow_manager.dpd_init_workflow", dyParam);
            return res;
        }
        public ResponseMessage SetWorkflowStatus(string pvc_wfslno, string pvc_statuscode, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();

            dyParam.Add("pvc_wfslno", pvc_wfslno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_statuscode", pvc_statuscode, OracleMappingType.Varchar2, ParameterDirection.Input, 5);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_workflow_manager.dpd_set_workflowstatus", dyParam);
            return res;
        }
        public IEnumerable<AccountOpeningReq> GetAccountOpenRequest(string pvc_acregslono, string pvc_acname, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();

            dyParam.Add("pvc_acregslono", pvc_acregslono, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_acname", pvc_acname, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pcr_accountopenreq", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<AccountOpeningReq>("pkg_search_manager.dpd_get_accountopenreq", dyParam, commandType: CommandType.StoredProcedure);
        }
        public ResponseMessage DelAccountOpeningRequest(string pvc_acregslno, string pvc_delreason, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();

            dyParam.Add("pvc_acregslno", pvc_acregslno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_delreason", pvc_delreason, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_pms_manager.dpd_del_accountopening", dyParam);
            return res;
        }
        public IEnumerable<TransactionDetails> GetTransactionDetails(string pvc_transid, string pvc_custno, string pvc_custacno, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();

            dyParam.Add("pvc_transid", pvc_transid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_custno", pvc_custno, OracleMappingType.Varchar2, ParameterDirection.Input, 9);
            dyParam.Add("pvc_custacno", pvc_custacno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pcr_transactiondata", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<TransactionDetails>("pkg_search_manager.dpd_get_transactiondata", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<WorkflowDetail> GetWorkflowRequest(string pvc_wfslono, string pvc_acdesc, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();

            dyParam.Add("pvc_wfslono", pvc_wfslono, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_acdesc", pvc_acdesc, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pcr_workflowreq", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<WorkflowDetail>("pkg_search_manager.dpd_get_workflowreq", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<WorkflowDetail> GetUnauthWorkflowRequest(string pvc_wfslno, string pvc_wftype, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();

            dyParam.Add("pvc_wfslno", pvc_wfslno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_wftype", pvc_wftype, OracleMappingType.Varchar2, ParameterDirection.Input, 2);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pcr_workflowrequest", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<WorkflowDetail>("pkg_workflow_manager.dpd_get_workflowrequest", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<Module> GetModiuleList(string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);

            dyParam.Add("pcr_modulelist", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Module>("pkg_lov_manager.dpd_get_modulelist", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<UserType> GetUserTypeList(string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);

            dyParam.Add("pcr_usertypelist", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<UserType>("pkg_lov_manager.dpd_get_appusertypelist", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<Branch> GetBranchList(string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);

            dyParam.Add("pcr_branchlist", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Branch>("pkg_lov_manager.dpd_get_branchlist", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<Branch> GetUserBranchList(string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);

            dyParam.Add("pcr_usrbranchlist", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Branch>("pkg_lov_manager.dpd_get_usrbranchlist", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<Department> GetDepartmentList(string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);

            dyParam.Add("pcr_departmentlist", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Department>("pkg_lov_manager.dpd_get_departmentlist", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<Agent> GetAgentList(string pvc_agentid, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_agentid", pvc_agentid, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);

            dyParam.Add("pcr_agentlist", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Agent>("pkg_lov_manager.dpd_get_agentlist", dyParam, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<AgentOutlet> GetAgentOutletList(string pvc_agentid, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_agentid", pvc_agentid, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);

            dyParam.Add("pcr_outletlist", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<AgentOutlet>("pkg_lov_manager.dpd_get_agentoutletlist", dyParam, commandType: CommandType.StoredProcedure);
        }
        public ResponseMessage SetUserDtl(string pvc_userid, string pvc_userpass, string pvc_username,
                                    string pvc_useremail, string pvc_userphone, string pvc_usertype,
                                    string pvc_otponlogon, string pvc_clientip, string pvc_bindip,
                                    string pvc_modid, string pvc_empid, string pvc_brcode,
                                    string pvc_deptcode, string pvc_agentid, string pvc_outletid, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();

            dyParam.Add("pvc_userid", pvc_userid, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_userpass", pvc_userpass, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_username", pvc_username, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_useremail", pvc_useremail, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_userphone", pvc_userphone, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_usertype", pvc_usertype, OracleMappingType.Varchar2, ParameterDirection.Input, 2);
            dyParam.Add("pvc_otponlogon", pvc_otponlogon, OracleMappingType.Varchar2, ParameterDirection.Input, 1);
            dyParam.Add("pvc_clientip", pvc_clientip, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_bindip", pvc_bindip, OracleMappingType.Varchar2, ParameterDirection.Input, 1);
            dyParam.Add("pvc_modid", pvc_modid, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_empid", pvc_empid, OracleMappingType.Varchar2, ParameterDirection.Input, 8);
            dyParam.Add("pvc_brcode", pvc_brcode, OracleMappingType.Varchar2, ParameterDirection.Input, 3);
            dyParam.Add("pvc_deptcode", pvc_deptcode, OracleMappingType.Varchar2, ParameterDirection.Input, 3);
            dyParam.Add("pvc_agentid", pvc_agentid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_outletid", pvc_outletid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);

            var res = responseMessage.QueryExecute(Connection, "pkg_sec_user.dpd_set_userdtl", dyParam);
            return res;
        }
        public IEnumerable<User> CmdUnauthUsers(string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input);

            dyParam.Add("pcr_unauthusers", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<User>("pkg_sec_user.dpd_get_unauthusers", dyParam, commandType: CommandType.StoredProcedure);
        }
        public ResponseMessage CmdAuthUsers(string[] pvc_userid, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();

            dyParam.Add("pvc_userid", pvc_userid, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_sec_user.dpd_set_usersauthorized", dyParam);
            return res;
        }
        public Decimal CheckUserName(string pvc_userid)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_userid", pvc_userid, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("rowfound", 0, OracleMappingType.Decimal, ParameterDirection.ReturnValue);
            Connection.Query("pkg_sec_user.dfn_chk_user", dyParam, commandType: CommandType.StoredProcedure);
            var res = dyParam.Get<Decimal>("rowfound");
            return res;
        }

        public ResponseMessage AuthorizeServiceRequest(string wfslno, string wf_ref_no, string wf_type, string wf_resp_dtl, string user_id)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();

            dyParam.Add("pvc_wfslno", wfslno, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_wftype", wf_type, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_wfrefno", wf_ref_no, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_wfrespdtl", wf_resp_dtl, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_appuser", user_id, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_workflow_manager.dpd_set_workflowauthorized", dyParam);
            return res;
        }

        public IEnumerable<AccountOpeningReq> GetAccountOpeningRequest(string pvc_branch, string pvc_acregslno, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();

            dyParam.Add("pvc_branch", pvc_branch, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_acregslno", pvc_acregslno, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pcr_accountopeningrequest", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<AccountOpeningReq>("pkg_pms_manager.dpd_get_accountopeningrequest", dyParam, commandType: CommandType.StoredProcedure);
        }

        public ResponseMessage AuthorizeAccountOpeningRequest(string pvc_acregslno, string pvc_custacno, string pvc_acdesc, string pvc_acopendate, string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();

            dyParam.Add("pvc_acregslno", pvc_acregslno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_custacno", pvc_custacno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_acdesc", pvc_acdesc, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_acopendate", pvc_acopendate, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_pms_manager.dpd_set_accountauthorized", dyParam);
            return res;
        }
    }
    public interface IAccountOpeningRepository
    {
        ResponseMessage CmdInitiateAccountOpening(string pvc_acregslno, string pvc_accusttype, string pvc_jointacind, string pvc_noofcust,
                                                  string pvc_acname, string pvc_chequebook, string pvc_debitcard, string pvc_remarks,
                                                  string pvc_appuser, string pvc_agentid, string pvc_stationip);
        IEnumerable<WorkFlowType> GetWorkflowType(string pvc_appuser);
        ResponseMessage InitWorkflowDtl(string pvc_wfslno, string pvc_wftype, string pvc_custno, string pvc_custacno, string pvc_cardno, string pvc_acdesc, string pvc_custname,
                                         string pvc_fathername, string pvc_mothername, string pvc_dob,
                                         string pvc_sex, string pvc_nid, string pvc_mobile, string pvc_email,
                                         string pvc_amount, string pvc_remarks, string pvc_appuser, string pvc_agentid,
                                         string pvc_stationip);
        ResponseMessage SetWorkflowStatus(string pvc_wfslno, string pvc_statuscode, string pvc_appuser);
        IEnumerable<AccountOpeningReq> GetAccountOpenRequest(string pvc_acregslono, string pvc_acname, string pvc_appuser);
        ResponseMessage DelAccountOpeningRequest(string pvc_acregslno, string pvc_delreason, string pvc_appuser);
        IEnumerable<TransactionDetails> GetTransactionDetails(string pvc_transid, string pvc_custno, string pvc_custacno, string pvc_appuser);
        IEnumerable<WorkflowDetail> GetWorkflowRequest(string pvc_wfslono, string pvc_acdesc, string pvc_appuser);
        IEnumerable<WorkflowDetail> GetUnauthWorkflowRequest(string pvc_wfslono, string pvc_acdesc, string pvc_appuser);
        IEnumerable<Module> GetModiuleList(string pvc_appuser);
        IEnumerable<UserType> GetUserTypeList(string pvc_appuser);
        IEnumerable<Branch> GetBranchList(string pvc_appuser);
        IEnumerable<Department> GetDepartmentList(string pvc_appuser);
        IEnumerable<Agent> GetAgentList(string pvc_agentid, string pvc_appuser);
        IEnumerable<AgentOutlet> GetAgentOutletList(string pvc_agentid, string pvc_appuser);
        ResponseMessage SetUserDtl(string pvc_userid, string pvc_userpass, string pvc_username,
                                    string pvc_useremail, string pvc_userphone, string pvc_usertype,
                                    string pvc_otponlogon, string pvc_clientip, string pvc_bindip,
                                    string pvc_modid, string pvc_empid, string pvc_brcode,
                                    string pvc_deptcode, string pvc_agentid, string pvc_outletid, string pvc_appuser);
        IEnumerable<User> CmdUnauthUsers(string pvc_appuser);
        ResponseMessage CmdAuthUsers(string[] pvc_userid, string pvc_appuser);
        Decimal CheckUserName(string pvc_userid);
        ResponseMessage AuthorizeServiceRequest(string wfslno, string wf_ref_no, string wf_type, string wf_resp_dtl, string user_id);
        IEnumerable<Branch> GetUserBranchList(string pvc_appuser);
        IEnumerable<AccountOpeningReq> GetAccountOpeningRequest(string pvc_branch, string pvc_acregslno, string pvc_appuser);
       ResponseMessage AuthorizeAccountOpeningRequest(string pvc_acregslno, string pvc_custacno, string pvc_acdesc, string pvc_acopendate, string pvc_appuser);
    }
}
