using Dapper.Oracle;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Common;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace EasyAssetManagerCore.Repository.Operation
{
    public class CheckBookRequestRepository : BaseRepository, ICheckBookRequestRepository
    {
        public CheckBookRequestRepository(OracleConnection connection) : base(connection)
        {

        }
    
        public ResponseMessage InitiateCheckBookRequest(WorkflowDetail workflowDetail, AppSession appSession)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();

            dyParam.Add("pvc_wfslno", workflowDetail.wf_slno, OracleMappingType.Varchar2, ParameterDirection.InputOutput, 20);
            dyParam.Add("pvc_wftype ", workflowDetail.wf_type, OracleMappingType.Varchar2, ParameterDirection.Input, 2);
            dyParam.Add("pvc_custno ", workflowDetail.cust_no, OracleMappingType.Varchar2, ParameterDirection.Input, 9);
            dyParam.Add("pvc_custacno", workflowDetail.cust_ac_no, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_cardno", workflowDetail.card_no, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_acdesc ", workflowDetail.ac_desc, OracleMappingType.Varchar2, ParameterDirection.Input, 110);
            dyParam.Add("pvc_custname ", workflowDetail.cust_name, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_fathername ", workflowDetail.father_name, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_mothername ", workflowDetail.mother_name, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_dob ", workflowDetail.birth_date, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_sex", workflowDetail.sex, OracleMappingType.Varchar2, ParameterDirection.Input, 1);
            dyParam.Add("pvc_nid", workflowDetail.nid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_mobile", workflowDetail.mobile, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_email", workflowDetail.email, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_amount", workflowDetail.amount, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_vatamount", workflowDetail.vat_amount, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_leavesnumber", workflowDetail.checkbook_leaves, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_checkbook_requisition_type", workflowDetail.checkbook_requisition_type, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_remarks", workflowDetail.remarks, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_appuser", appSession.User.user_id, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_agentid", appSession.User.agent_id, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_stationip", appSession.User.StationIp, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_phase3.dpd_init_workflow", dyParam);
            return res;
        }

        public ResponseMessage GetCheckBookLeavesCharges(string pvc_leavesnumber, string pvc_appuser)
        {

            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_leavesnumber", pvc_leavesnumber, OracleMappingType.Varchar2, ParameterDirection.Input, 2);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 20);

            dyParam.Add("pnm_commamt", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pnm_vatamt", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 50);
            dyParam.Add("pnm_totalamt", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 50);
            var responseMessage = new ResponseMessage();
            var res = responseMessage.QueryExecute(Connection, "pkg_phase3.dpp_get_workflow_charge", dyParam);
            return res;
        }

        public ResponseMessage SaveCheckBookRequestDoc(string pvc_wfslno,string pvc_filename, string pvc_fileExtension, AppSession appSession)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();

            dyParam.Add("pvc_wfslno", pvc_wfslno, OracleMappingType.Varchar2, ParameterDirection.InputOutput, 20);
            dyParam.Add("pvc_filename ", pvc_filename, OracleMappingType.Varchar2, ParameterDirection.Input, 2);
            dyParam.Add("pvc_fileExtension ", pvc_fileExtension, OracleMappingType.Varchar2, ParameterDirection.Input, 9);

            dyParam.Add("pvc_appuser", appSession.User.user_id, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_agentid", appSession.User.agent_id, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_stationip", appSession.User.StationIp, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_phase3.dpd_set_docfile", dyParam);
            return res;
        }
        public ResponseMessage DeleteCheckBookRequestDoc(string pvc_wfslno, string pvc_filename, AppSession appSession)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();

            dyParam.Add("pvc_wfslno", pvc_wfslno, OracleMappingType.Varchar2, ParameterDirection.InputOutput, 20);
            dyParam.Add("pvc_filename ", pvc_filename, OracleMappingType.Varchar2, ParameterDirection.Input, 2);

            dyParam.Add("pvc_appuser", appSession.User.user_id, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_agentid", appSession.User.agent_id, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_stationip", appSession.User.StationIp, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var res = responseMessage.QueryExecute(Connection, "pkg_phase3.dpd_delete_docfile", dyParam);
            return res;
        }
    }

    public interface ICheckBookRequestRepository
    {
        ResponseMessage InitiateCheckBookRequest(WorkflowDetail workflowDetail, AppSession appSession);
        ResponseMessage GetCheckBookLeavesCharges(string pvc_leavesnumber, string pvc_appuser);
        ResponseMessage SaveCheckBookRequestDoc(string pvc_wfslno, string pvc_filename, string pvc_fileExtension, AppSession appSession);
        ResponseMessage DeleteCheckBookRequestDoc(string pvc_wfslno, string pvc_filename, AppSession appSession);
    }
}
