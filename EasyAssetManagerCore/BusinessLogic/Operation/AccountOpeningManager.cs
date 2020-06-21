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

namespace EasyAssetManagerCore.BusinessLogic.Operation
{
    public class AccountOpeningManager : BaseService, IAccountOpeningManager
    {
        //private readonly IAccountOpeningRepository accountOpeningRepository;
        private readonly ICommonManager commonManager;
        private readonly ICbsDataConnectionManager cbsDataConnectionManager;
        private readonly ICustomerRepository customerRepository;
       // private readonly IAccountRepository accountRepository;
        public AccountOpeningManager() : base((int)ConnectionStringEnum.EbankConnectionString)
        {
          //  accountOpeningRepository = new AccountOpeningRepository(Connection);
            commonManager = new CommonManager();
            cbsDataConnectionManager = new CbsDataConnectionManager();
            customerRepository = new CustomerRepository(Connection);
           // accountRepository = new FileProcessRepository(Connection);
        }
        public Message NewAccountOpening(AccountOpening accountOpening, AppSession session, IHttpContextAccessor contextAccessor)
        {
            if (string.IsNullOrEmpty(accountOpening.ac_customer_type))
            {
                MessageHelper.Error(Message, "Select Account Type to continue.");
            }
            else if (string.IsNullOrEmpty(accountOpening.joint_ac_indicator))
            {
                MessageHelper.Error(Message, "Select Operating Mode to continue.");
            }
            else
            {
                try
                {
                    if (Connection.State != ConnectionState.Open)
                        Connection.Open();
                    var msg = new ResponseMessage(); //accountOpeningRepository.CmdInitiateAccountOpening(accountOpening.ac_reg_slno, accountOpening.ac_customer_type, accountOpening.joint_ac_indicator
                                                                         //   , accountOpening.no_of_customer.ToString(), accountOpening.ac_name, (accountOpening.cheque_book == "Yes" ? "Y" : "N"), (accountOpening.debit_card == "Yes" ? "Y" : "N")
                                                                         //   , accountOpening.remarks, session.User.user_id, session.User.agent_id, session.User.StationIp);
                    if (msg.pvc_status == "40999")
                    {
                        session.TransactionSession.TransactionID = msg.pvc_acregslno;
                        MessageHelper.Success(Message, "Account Opening Successfull.");
                    }
                    else
                    {
                        MessageHelper.Error(Message, "Account Opening Failed. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "AccountOpeningManager-NewAccountOpening", ex.Message + "|" + ex.StackTrace.TrimStart());
                    MessageHelper.Error(Message, "System Error.");
                }
                finally
                {
                    Connection.Close();
                    contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
                }
            }
            return Message;
        }
        public IEnumerable<WorkFlowType> GetWorkflowType(string pvc_appuser)
        {
            return null;//accountOpeningRepository.GetWorkflowType(pvc_appuser);
        }

        public IEnumerable<Branch> GetUserBranchList(string pvc_appuser)
        {
            return null; //accountOpeningRepository.GetUserBranchList(pvc_appuser);
        }

        public IEnumerable<AccountOpeningReq> GetAccountOpeningRequest(string pvc_branch, string pvc_acregslno, string pvc_appuser)
        {
            return null; //accountOpeningRepository.GetAccountOpeningRequest(pvc_branch, pvc_acregslno, pvc_appuser);
        }

        public Message AuthorizeAccountOpeningRequest(string pvc_acregslno, string pvc_custacno, string pvc_acdesc, string pvc_acopendate, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var msg = new ResponseMessage();  //accountOpeningRepository.AuthorizeAccountOpeningRequest(pvc_acregslno,pvc_custacno,pvc_acdesc,pvc_acopendate, session.User.user_id);
                if (msg.pvc_status == "40999")
                {
                    var customers = customerRepository.GetCustomersByAccountNumber(pvc_custacno, session.User.user_id);
                    if(customers!=null && customers.Any() && customers.Where(o => o.mobile_number != null).Count() > 0)
                    {
                        commonManager.SendSms(customers.FirstOrDefault().mobile_number, "Dear Customer, your account in EBL has been created. Please contact with Agent Outlet for Fingerprint Registration if required. Thanks. Helpline 16230");
                    }
                    MessageHelper.Success(Message, "Authorized Successfully!!!");
                }
                else
                {
                    MessageHelper.Error(Message, "Unable to process!  "+msg.pvc_statusmsg);
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

        public IEnumerable<Account> GeAccountDetails(string customerAccountNo, string userId)
        {
            return null;//accountRepository.GeAccountDetails(customerAccountNo, userId);
        }
    }
    public interface IAccountOpeningManager
    {
        Message NewAccountOpening(AccountOpening accountOpening, AppSession session, IHttpContextAccessor contextAccessor);
        IEnumerable<WorkFlowType> GetWorkflowType(string pvc_appuser);
        IEnumerable<Branch> GetUserBranchList(string pvc_appuser);
        IEnumerable<AccountOpeningReq> GetAccountOpeningRequest(string pvc_branch, string pvc_acregslno, string pvc_appuser);
        Message AuthorizeAccountOpeningRequest(string pvc_acregslno, string pvc_custacno, string pvc_acdesc, string pvc_acopendate, AppSession session);
        IEnumerable<Account> GeAccountDetails(string customerAccountNo, string userId);
    }
}
