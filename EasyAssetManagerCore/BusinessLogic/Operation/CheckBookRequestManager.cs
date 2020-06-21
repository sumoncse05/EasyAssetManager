using EasyAssetManagerCore.BusinessLogic.Common;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Operation;
using Microsoft.AspNetCore.Http;
using System;
using System.Data;

namespace EasyAssetManagerCore.BusinessLogic.Operation
{
    public class CheckBookRequestManager : BaseService, ICheckBookRequestManager
    {
        private readonly ICheckBookRequestRepository checkBookRequestRepository;
        private readonly ICommonManager commonManager;
        private readonly ICbsDataConnectionManager cbsDataConnectionManager;
        private readonly ICustomerRepository customerRepository;
        //private readonly IAccountOpeningRepository accountOpeningRepository;
        public CheckBookRequestManager() : base((int)ConnectionStringEnum.EbankConnectionString)
        {
            checkBookRequestRepository = new CheckBookRequestRepository(Connection);
            commonManager = new CommonManager();
            cbsDataConnectionManager = new CbsDataConnectionManager();
            customerRepository = new CustomerRepository(Connection);
            //accountOpeningRepository = new AccountOpeningRepository(Connection);
        }

        public WorkflowDetail CustomerVerification(WorkflowDetail workflowdetail, AppSession session, IHttpContextAccessor contextAccessor)
        {
            var workflowDetailOld = new WorkflowDetail();
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                if (string.IsNullOrEmpty(workflowdetail.cust_ac_no))
                {
                    MessageHelper.Error(Message, "Enter a customer accountno to continue.");
                }
                else
                {
                    
                    var charge = checkBookRequestRepository.GetCheckBookLeavesCharges(workflowdetail.checkbook_leaves, session.User.user_id);
                     workflowDetailOld = cbsDataConnectionManager.GetCustometDtlFromCbs(workflowdetail.cust_ac_no, session.User.user_id);
                    if (workflowDetailOld != null)
                    {
                        session.TransactionSession.TransactionCustomerNo = workflowDetailOld.customer_no;
                        workflowDetailOld.cust_ac_no= session.TransactionSession.TransactionAccountNo = workflowdetail.cust_ac_no;
                        session.TransactionSession.CustomerName = workflowDetailOld.customer_name;
                        session.TransactionSession.CustomerFatherName = workflowDetailOld.father_name;
                         session.TransactionSession.CustomerMotherName = workflowDetailOld.mother_name;
                        session.TransactionSession.CustomerDateofBirth = workflowDetailOld.birth_date;
                        session.TransactionSession.TransactionCustMobileNo = workflowDetailOld.mobile;
                        session.TransactionSession.CustomerSex = workflowDetailOld.sex;
                        session.TransactionSession.CustomerNID = workflowDetailOld.nid;
                        workflowDetailOld.vat_amount = session.TransactionSession.TransactionVatAmount = charge.pnm_vatamt;
                        workflowDetailOld.amount = session.TransactionSession.TransactionAmount = charge.pnm_commamt;
                        MessageHelper.Success(Message, "Customer Verify Successfully.");
                    }
                    else
                    {
                        MessageHelper.Error(Message, "Customer account not found.");
                    }
                    
                }
                        
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "CheckBookRequest-CustomerVerification", ex.Message + "|" + ex.StackTrace.TrimStart());
                MessageHelper.Error(Message, "System Error.");
            }
            finally
            {
                Connection.Close();
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
                workflowDetailOld.Message = Message;
            }
            return workflowDetailOld;
        }

        public WorkflowDetail CheckBookRequestInitiate(WorkflowDetail workflowdetail, AppSession session, IHttpContextAccessor contextAccessor)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                    var accountStatus = checkBookRequestRepository.InitiateCheckBookRequest(workflowdetail,session);

                    if (accountStatus.pvc_status == "40999")
                    {
                        session.TransactionSession.TransactionID = accountStatus.pvc_wfslno;
                        var retMsg = commonManager.RequestFingerScan("V", "04", workflowdetail.customer_no, accountStatus.pvc_wfslno, session.User.user_id, session.User.StationIp);
                        if (retMsg.pvc_status == "40999")
                        {
                            session.TransactionSession.FingerVerifyStatus = workflowdetail.finger_status = retMsg.pvc_statusmsg;
                            session.TransactionSession.FingerReqRefNo = retMsg.pvc_otpreqrefno;
                            MessageHelper.Success(Message, "Initiate CheckBook Request Successfully.");
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
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "CheckBookRequestManager-ReGenerateFingerRequest", ex.Message + "|" + ex.StackTrace.TrimStart());
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
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "CheckBookRequestManager-GetFingerVerifyStatus", ex.Message + "|" + ex.StackTrace.TrimStart());
            }
            finally
            {
                Connection.Close();
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
            }
            return msg;
        }

        public Message CompleteRequest(AppSession session, IHttpContextAccessor contextAccessor)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                if (session.TransactionSession.FingerVerifyStatus == "S")
                {
                    var AccountStatus = new ResponseMessage();//accountOpeningRepository.SetWorkflowStatus(session.TransactionSession.TransactionID, "50999", session.User.agent_id);

                    if (AccountStatus.pvc_status == "40999")
                    {
                        //session.TransactionSession.TransactionID = AccountStatus.pvc_wfslno;
                        MessageHelper.Success(Message, "Check book request submit successfully.");
                    }
                    else
                    {
                        MessageHelper.Error(Message, "Unable to submit check book request.");
                    }
                }
                else
                {
                    MessageHelper.Error(Message, "Finger not yet Verified");
                }
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "CheckBookRequestManager-CompleteRequest", ex.Message + "|" + ex.StackTrace.TrimStart());
                MessageHelper.Error(Message, "System Error.");
            }
            finally
            {
                Connection.Close();
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
            }
            return Message;
        }

        public Message SaveCheckBookRequestDoc(string pvc_wfslno, string pvc_filename, string pvc_fileExtension, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                    var AccountStatus = checkBookRequestRepository.SaveCheckBookRequestDoc(pvc_wfslno,pvc_filename,pvc_fileExtension,session);

                    if (AccountStatus.pvc_status == "40999")
                    {
                        MessageHelper.Success(Message, "Check book doc save successfully.");
                    }
                    else
                    {
                        MessageHelper.Error(Message, "Unable to save check book doc.");
                    }
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "CheckBookRequestManager-SaveCheckBookRequestDoc", ex.Message + "|" + ex.StackTrace.TrimStart());
                MessageHelper.Error(Message, "System Error.");
            }
            finally
            {
                Connection.Close();
                
            }
            return Message;
        }

        public Message DeleteCheckBookRequestDoc(string pvc_wfslno, string pvc_filename, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var AccountStatus = checkBookRequestRepository.DeleteCheckBookRequestDoc(pvc_wfslno, pvc_filename, session);

                if (AccountStatus.pvc_status == "40999")
                {
                    MessageHelper.Success(Message, "Check book doc delete successfully.");
                }
                else
                {
                    MessageHelper.Error(Message, "Unable to delete check book doc.");
                }
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "CheckBookRequestManager-DeleteCheckBookRequestDoc", ex.Message + "|" + ex.StackTrace.TrimStart());
                MessageHelper.Error(Message, "System Error.");
            }
            finally
            {
                Connection.Close();

            }
            return Message;
        }
    }
    public interface ICheckBookRequestManager
    {
        WorkflowDetail CustomerVerification(WorkflowDetail workflowdetail, AppSession session, IHttpContextAccessor contextAccessor);
        WorkflowDetail CheckBookRequestInitiate(WorkflowDetail workflowdetail, AppSession session, IHttpContextAccessor contextAccessor);
        ResponseMessage GetFingerVerifyStatus(AppSession session, IHttpContextAccessor contextAccessor);
        Message ReGenerateFingerRequest(AppSession session, IHttpContextAccessor contextAccessor);
        Message CompleteRequest(AppSession session, IHttpContextAccessor contextAccessor);
        Message SaveCheckBookRequestDoc(string pvc_wfslno, string pvc_filename, string pvc_fileExtension, AppSession appSession);
        Message DeleteCheckBookRequestDoc(string pvc_wfslno, string pvc_filename, AppSession appSession);
    }
}
