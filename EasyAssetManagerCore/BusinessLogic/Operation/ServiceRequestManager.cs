using EasyAssetManagerCore.BusinessLogic.Common;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Operation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UbsService;

namespace EasyAssetManagerCore.BusinessLogic.Operation
{
    public class ServiceRequestManager : BaseService, IServiceRequestManager
    {
       // private readonly IAccountOpeningRepository accountOpeningRepository;
        private readonly ICommonManager commonManager;
        private readonly ICbsDataConnectionManager cbsDataConnectionManager;
        public ServiceRequestManager() : base((int)ConnectionStringEnum.EbankConnectionString)
        {
            //accountOpeningRepository = new AccountOpeningRepository(Connection);
            commonManager = new CommonManager();
            cbsDataConnectionManager = new CbsDataConnectionManager();
        }

        public WorkflowDetail CustomerVerification(string customer_no, string wf_type, string remarks, AppSession session, IHttpContextAccessor contextAccessor)
        {
            var workflowdetail = new WorkflowDetail();
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                if (string.IsNullOrEmpty(customer_no))
                {
                    MessageHelper.Error(Message, "Select a customer no to continue.");
                }
                else
                {
                    var customer = session.TransactionSession.Customers.Where(x => x.customer_no == customer_no).FirstOrDefault();
                    workflowdetail.cust_no = session.TransactionSession.TransactionCustomerNo = customer.customer_no;
                    workflowdetail.cust_ac_no = session.TransactionSession.TransactionAccountNo;
                    workflowdetail.ac_desc = session.TransactionSession.TransactionAccountDesc;
                    workflowdetail.cust_name = session.TransactionSession.CustomerName = customer.customer_name;
                    workflowdetail.father_name = session.TransactionSession.CustomerFatherName = customer.father_name;
                    workflowdetail.mother_name = session.TransactionSession.CustomerMotherName = customer.mother_name;
                    workflowdetail.birth_date = session.TransactionSession.CustomerDateofBirth = customer.date_of_birth;
                    workflowdetail.mobile = session.TransactionSession.TransactionCustMobileNo = customer.mobile_number;
                    session.TransactionSession.CustomerSex = customer.sex;
                    workflowdetail.sex = customer.sex == "M" ? "Male" : "Female";
                    workflowdetail.nid = session.TransactionSession.CustomerNID = customer.nid;
                    workflowdetail.remarks = remarks;

                    var accountStatus = new ResponseMessage(); //accountOpeningRepository.InitWorkflowDtl("", wf_type, customer.customer_no, session.TransactionSession.TransactionAccountNo,
                                              // "", session.TransactionSession.TransactionAccountDesc, customer.customer_name, customer.father_name,
                                             //  customer.mother_name, customer.date_of_birth.ToString(), customer.sex, customer.nid, customer.mobile_number, "",
                                            //   "", remarks, session.User.user_id, session.User.agent_id, session.User.StationIp);

                    if (accountStatus.pvc_status == "40999")
                    {
                        session.TransactionSession.TransactionID = accountStatus.pvc_wfslno;
                        var retMsg = commonManager.RequestFingerScan("V", "04", customer_no, accountStatus.pvc_wfslno, session.User.user_id, session.User.StationIp);
                        if (retMsg.pvc_status == "40999")
                        {
                            session.TransactionSession.FingerVerifyStatus = workflowdetail.finger_status = retMsg.pvc_statusmsg;
                            session.TransactionSession.FingerReqRefNo = retMsg.pvc_otpreqrefno;
                            MessageHelper.Success(Message, "Re-Generate Fingure Request Successfully.");
                        }
                        else
                        {
                            session.TransactionSession.FingerReqRefNo = retMsg.pvc_otpreqrefno;
                            session.TransactionSession.FingerVerifyStatus = "N";
                            workflowdetail.finger_status = retMsg.pvc_statusmsg;
                            MessageHelper.Error(Message, accountStatus.pvc_statusmsg);
                        }
                    }
                    else
                    {
                        MessageHelper.Error(Message, accountStatus.pvc_statusmsg);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "ServiceRequest-CustomerVerification", ex.Message + "|" + ex.StackTrace.TrimStart());
                MessageHelper.Error(Message, "System Error.");
            }
            finally
            {
                Connection.Close();
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
                workflowdetail.Message = Message;
            }
            return workflowdetail;
        }
        public Message CustomerFingerVerification(string wf_desc, AppSession session, IHttpContextAccessor contextAccessor)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                if (session.TransactionSession.FingerVerifyStatus == "S")
                {
                    var AccountStatus = new ResponseMessage(); //accountOpeningRepository.SetWorkflowStatus(session.TransactionSession.TransactionID, "50999", session.User.agent_id);

                    if (AccountStatus.pvc_status == "40999")
                    {
                        //session.TransactionSession.TransactionID = AccountStatus.pvc_wfslno;
                        MessageHelper.Success(Message, "Service request for " + wf_desc + " saved successfully.");
                    }
                    else
                    {
                        MessageHelper.Error(Message, "Unable to save Service request for " + wf_desc);
                    }
                }
                else
                {
                    MessageHelper.Error(Message, "Finger not yet Verified");
                }
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "ServiceRequest-CustomerFingerVerification", ex.Message + "|" + ex.StackTrace.TrimStart());
                MessageHelper.Error(Message, "System Error.");
            }
            finally
            {
                Connection.Close();
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
            }
            return Message;
        }
        public ResponseMessage GetFingerVerifyStatus(AppSession session, IHttpContextAccessor contextAccessor)
        {
            var msg = new ResponseMessage();
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                msg = commonManager.GetFingerScanStatus(session.TransactionSession.FingerReqRefNo, session.User.user_id, session.User.StationIp);
                session.TransactionSession.FingerVerifyStatus = msg.pvc_statusmsg;
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "ServiceRequestManager-GetFingerVerifyStatus", ex.Message + "|" + ex.StackTrace.TrimStart());
            }
            finally
            {
                Connection.Close();
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
            }
            return msg;
        }

        public Message ReGenerateFingerRequest(AppSession session, IHttpContextAccessor contextAccessor)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var msg = commonManager.GetFingerScanStatus(session.TransactionSession.FingerReqRefNo, session.User.user_id, session.User.StationIp);
                if (msg.pvc_status == "40999" && msg.pvc_statusmsg == "S")
                {
                    MessageHelper.Error(Message, "Can not Re-Generate. Already Verified. Refreh.");
                }
                else
                {
                    var retMsg = commonManager.RequestFingerScan("V", "04", session.TransactionSession.TransactionCustomerNo, session.TransactionSession.TransactionID, session.User.user_id, session.User.StationIp);
                    if (retMsg.pvc_status == "40999")
                    {
                        session.TransactionSession.FingerReqRefNo = retMsg.pvc_otpreqrefno;
                        MessageHelper.Success(Message, "Re-Generate Fingure Request Successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "ServiceRequest-ReGenerateFingerRequest", ex.Message + "|" + ex.StackTrace.TrimStart());
                MessageHelper.Error(Message, "System Error.");
            }
            finally
            {
                Connection.Close();
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
            }
            return Message;
        }

        public IEnumerable<WorkflowDetail> GetUnauthWorkflowRequest(string wfslno, string wftype, string user_id)
        {
            //return accountOpeningRepository.GetUnauthWorkflowRequest(wfslno, wftype, user_id);
            return null;
        }

        public Message AuthorizeServiceRequest(string wfslno, string wf_ref_no, string wf_type, string wf_resp_dtl, string user_id)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                var AccountStatus = new ResponseMessage(); //accountOpeningRepository.AuthorizeServiceRequest(wfslno, wf_ref_no, wf_type, wf_resp_dtl, user_id);

                if (AccountStatus.pvc_status == "40999")
                {
                    MessageHelper.Success(Message, "Service Request Authorized Successfully!!!");
                }
                else
                {
                    MessageHelper.Error(Message, AccountStatus.pvc_statusmsg);
                }

            }
            catch (Exception ex)
            {

                MessageHelper.Error(Message, "System Error.");
            }
            finally
            {
                Connection.Close();
            }
            return Message;
        }

        public WorkflowDetail GetServiceReferenceDtl(string wfslno, string wf_ref_no,string wf_type, string user_id)
        {
            var record= cbsDataConnectionManager.GetServiceReferenceDtl(wfslno, wf_ref_no, wf_type, user_id);
            if (record != null) 
            {
                MessageHelper.Success(Message, "Recor is Found ");
            }
            else
            {
                record = new WorkflowDetail();
                MessageHelper.Error(Message,"Record is not found.");
            }
            record.Message = Message;
            return record;
        }
    }
    public interface IServiceRequestManager
    {
        WorkflowDetail CustomerVerification(string customer_no, string wf_type, string remarks, AppSession session, IHttpContextAccessor contextAccessor);
        Message CustomerFingerVerification(string wf_desc, AppSession session, IHttpContextAccessor contextAccessor);
        ResponseMessage GetFingerVerifyStatus(AppSession session, IHttpContextAccessor contextAccessor);
        Message ReGenerateFingerRequest(AppSession session, IHttpContextAccessor contextAccessor);
        IEnumerable<WorkflowDetail> GetUnauthWorkflowRequest(string pvc_transid, string pvc_custno, string user_id);
        Message AuthorizeServiceRequest(string wfslno, string wf_ref_no, string wf_type, string wf_resp_dtl, string user_id);
        WorkflowDetail GetServiceReferenceDtl(string wfslno, string wf_ref_no,string wf_type, string user_id);
    }
}
