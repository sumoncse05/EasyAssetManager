using EasyAssetManagerCore.BusinessLogic.Common;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Common;
using EasyAssetManagerCore.Repository.Operation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EasyAssetManagerCore.BusinessLogic.Operation
{
    public class BalanceEnquiryManager : BaseService, IBalanceEnquiryManager
    {
        //private readonly IAccountOpeningRepository accountOpeningRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly ICommonManager commonManager;
        private readonly ICbsDataConnectionManager cbsDataConnectionManager;
        public BalanceEnquiryManager() : base((int)ConnectionStringEnum.EbankConnectionString)
        {
           // accountOpeningRepository = new AccountOpeningRepository(Connection);
            transactionRepository = new TransactionRepository(Connection);
            commonManager = new CommonManager();
            cbsDataConnectionManager = new CbsDataConnectionManager();
        }
        public WorkflowDetail CustomerVerification(string customer_no, AppSession session, IHttpContextAccessor contextAccessor)
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
                    workflowdetail.cust_name = session.TransactionSession.CustomerName = customer.customer_name;
                    workflowdetail.mobile = session.TransactionSession.TransactionCustMobileNo = customer.mobile_number;
                    workflowdetail.cust_ac_no = session.TransactionSession.TransactionAccountNo;
                    workflowdetail.ac_desc = session.TransactionSession.TransactionAccountDesc;

                    var balanceEnquiryStatus = transactionRepository.InitiateBalanceEnquery(customer_no,session.User.agent_id,session.User.outlet_id ,session.User.user_id);
                    if (balanceEnquiryStatus.pvc_status == "40999")
                    {
                        session.TransactionSession.TransactionID = balanceEnquiryStatus.pvc_transid;
                        var retMsg = commonManager.RequestFingerScan("V", "04", customer_no, balanceEnquiryStatus.pvc_transid, session.User.user_id, session.User.StationIp);
                        if (retMsg.pvc_status == "40999")
                        {
                            session.TransactionSession.FingerVerifyStatus = workflowdetail.finger_status = retMsg.pvc_statusmsg;
                            session.TransactionSession.FingerReqRefNo = retMsg.pvc_otpreqrefno;
                            MessageHelper.Success(Message, "Scan Request Generated successfully. Scan Finger to Verify.");
                        }
                        else
                        {
                            session.TransactionSession.FingerReqRefNo = retMsg.pvc_otpreqrefno;
                            session.TransactionSession.FingerVerifyStatus = "N";
                            workflowdetail.finger_status = retMsg.pvc_statusmsg;
                            MessageHelper.Error(Message, balanceEnquiryStatus.pvc_statusmsg);
                        }
                    }
                    else
                    {
                        MessageHelper.Error(Message, balanceEnquiryStatus.pvc_statusmsg);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "BalanceEnquiry-CustomerVerification", ex.Message + "|" + ex.StackTrace.TrimStart());
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
        public Message SendAccountBalance(AppSession session, IHttpContextAccessor contextAccessor)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                if (session.TransactionSession.FingerVerifyStatus == "S")
                {
                    if (session.TransactionSession.TransactionCustMobileNo != "")
                    {
                        var accountBalance = transactionRepository.GetAccountBalance(session.TransactionSession.TransactionAccountNo,session.User.agent_id,session.User.user_id );
                            //.SetWorkflowStatus(session.TransactionSession.TransactionID, "50999", session.User.agent_id);

                        if (accountBalance.pvc_status == "40999")
                        {
                            string sms_message = "Dear Customer, your account balance is currently BDT " + accountBalance.pvc_balance + ". Thanks. EBL Helpline 16230";
                            var retMsg = commonManager.RequestSmsAlert(session.User.user_id, session.TransactionSession.TransactionCustMobileNo, sms_message, session.User.user_id, session.User.StationIp);
                            if (retMsg[0] == "S")
                            {
                                //session.TransactionSession.TransactionID = accountBalance.pvc_wfslno;
                                MessageHelper.Success(Message, "Transaction Completed Successfully.");
                            }
                            else
                            {
                                MessageHelper.Error(Message, "Unable to send SMS. Please try again.");
                            }
                        }
                        else
                        {
                            MessageHelper.Error(Message, "Unable to send SMS. Please try again.");
                        }
                    }
                    else
                    {
                        MessageHelper.Error(Message, "Customer Mobile No not found. Please try again.");
                    }
                }
                else
                {
                    MessageHelper.Error(Message, "Finger not yet verified or Verification Failed.");
                }
                
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "BalanceEnquiry-CustomerFingerVerification", ex.Message + "|" + ex.StackTrace.TrimStart());
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
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "BalanceEnquiry-GetFingerVerifyStatus", ex.Message + "|" + ex.StackTrace.TrimStart());
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
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "BalanceEnquiry-ReGenerateFingerRequest", ex.Message + "|" + ex.StackTrace.TrimStart());
                MessageHelper.Error(Message, "System Error.");
            }
            finally
            {
                Connection.Close();
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
            }
            return Message;
        }
    }
    public interface IBalanceEnquiryManager 
    {
        WorkflowDetail CustomerVerification(string customer_no, AppSession session, IHttpContextAccessor contextAccessor);
        Message SendAccountBalance(AppSession session, IHttpContextAccessor contextAccessor);
        ResponseMessage GetFingerVerifyStatus(AppSession session, IHttpContextAccessor contextAccessor);
        Message ReGenerateFingerRequest(AppSession session, IHttpContextAccessor contextAccessor);
    }
}
