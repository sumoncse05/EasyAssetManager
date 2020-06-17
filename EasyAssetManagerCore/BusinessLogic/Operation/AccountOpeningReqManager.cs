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


namespace EasyAssetManagerCore.BusinessLogic.Operation
{
    public class AccountOpeningReqManager : BaseService, IAccountOpeningReqManager
    {
        private readonly IAccountOpeningRepository accountOpeningRepository;
        private readonly ICommonManager commonManager;
        private readonly ICbsDataConnectionManager cbsDataConnectionManager;
        public AccountOpeningReqManager() : base((int)ConnectionStringEnum.EbankConnectionString)
        {
            accountOpeningRepository = new AccountOpeningRepository(Connection);
            commonManager = new CommonManager();
            cbsDataConnectionManager = new CbsDataConnectionManager();
        }
        public IEnumerable<AccountOpeningReq> GetAccountOpenRequest(string pvc_acregslono, string pvc_acname, AppSession session)
        {
            return accountOpeningRepository.GetAccountOpenRequest(pvc_acregslono, pvc_acname, session.User.user_id);
        }
        public Message DelAccountOpeningRequest(string pvc_acregslno, string pvc_delreason, AppSession session, IHttpContextAccessor contextAccessor)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var msg = accountOpeningRepository.DelAccountOpeningRequest(pvc_acregslno, pvc_delreason, session.User.user_id);
                if (msg.pvc_status == "40999")
                {
                    //session.TransactionSession.SmsReqRefNo = msg[2];
                    MessageHelper.Success(Message, "Account Opening Request deleted successful.");
                }
                else
                {
                    MessageHelper.Error(Message, "Account Opening Request is not deleted. Please try again.");
                }
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "AccountOpeningReqManager-DelAccountOpeningRequest", ex.Message + "|" + ex.StackTrace.TrimStart());
                MessageHelper.Error(Message, "Account Opening Request is not deleted. Please try again.");
            }
            finally
            {
                Connection.Close();
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
            }
            return Message;
        }
        public Message UpdateAccountOpening(AccountOpening accountOpening, AppSession session, IHttpContextAccessor contextAccessor)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var msg = accountOpeningRepository.CmdInitiateAccountOpening(accountOpening.ac_reg_slno, accountOpening.ac_customer_type, accountOpening.joint_ac_indicator
                                                                        , accountOpening.no_of_customer.ToString(), accountOpening.ac_name, (accountOpening.cheque_book == "Yes" ? "Y" : "N"), (accountOpening.debit_card == "Yes" ? "Y" : "N")
                                                                        , accountOpening.remarks, session.User.user_id, session.User.agent_id, session.User.StationIp);
                if (msg.pvc_status == "40999")
                {
                   // session.TransactionSession.TransactionID = msg.pvc_acregslno;
                    MessageHelper.Success(Message, "Account Update Successfull.");
                }
                else
                {
                    MessageHelper.Error(Message, "Account Update Failed. Please try again.");
                }
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "AccountOpeningManager-UpdateAccountOpening", ex.Message + "|" + ex.StackTrace.TrimStart());
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
    public interface IAccountOpeningReqManager
    {
        IEnumerable<AccountOpeningReq> GetAccountOpenRequest(string pvc_acregslono, string pvc_acname, AppSession session);
        Message UpdateAccountOpening(AccountOpening accountOpening, AppSession session, IHttpContextAccessor contextAccessor);
        Message DelAccountOpeningRequest(string pvc_acregslno, string pvc_delreason, AppSession session, IHttpContextAccessor contextAccessor);
    }
}
