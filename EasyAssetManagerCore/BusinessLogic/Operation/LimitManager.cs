using EasyAssetManagerCore.BusinessLogic.Common;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Operation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EasyAssetManagerCore.BusinessLogic.Operation
{
    public class LimitManager : BaseService, ILimitManager
    {
        private readonly ICbsDataConnectionManager cbsDataConnectionManager;
        private readonly ILimitRepository limitRepository;
       // private readonly IAccountRepository accountRepository;
       // private readonly IAccountOpeningRepository accountOpeningRepository;
        private readonly ICommonManager commonManager;
        public LimitManager() : base((int)ConnectionStringEnum.EbankConnectionString)
        {
            cbsDataConnectionManager = new CbsDataConnectionManager();
            limitRepository = new LimitRepository(Connection);
            commonManager = new CommonManager();
           // accountRepository = new FileProcessRepository(Connection);
           // accountOpeningRepository = new AccountOpeningRepository(Connection);
        }

        public Message AuthorizeLimitPackage(List<string> pvc_limitid, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var msg = limitRepository.AuthorizeLimitPackage(pvc_limitid, session.User.user_id);
                if (msg.pvc_status == "40999")
                {
                    MessageHelper.Success(Message, "Authorize Package Count: " + pvc_limitid.Count);
                }
                else
                {
                    MessageHelper.Error(Message, msg.pvc_statusmsg);
                }
            }
            catch (Exception ex)
            {

                MessageHelper.Error(Message, ex.Message);
            }
            finally
            {
                Connection.Close();
            }
            return Message;
        }

        public Message AuthorizeTransLimit(List<string> pvc_limitslno, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var msg = limitRepository.AuthorizeTransLimit(pvc_limitslno, session.User.user_id);
                if (msg.pvc_status == "40999")
                {
                    MessageHelper.Success(Message, "Authorize Package Count: " + pvc_limitslno.Count);
                }
                else
                {
                    MessageHelper.Error(Message, msg.pvc_statusmsg);
                }
            }
            catch (Exception ex)
            {

                MessageHelper.Error(Message, ex.Message);
            }
            finally
            {
                Connection.Close();
            }
            return Message;
        }

        public Message DeleteLimitPackage(string pvc_limitid, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var msg = limitRepository.DeleteLimitPackage(pvc_limitid, session.User.user_id);
                if (msg.pvc_status == "40999")
                {
                    MessageHelper.Success(Message, msg.pvc_statusmsg);
                }
                else
                {
                    MessageHelper.Error(Message, msg.pvc_statusmsg);
                }
            }
            catch (Exception ex)
            {

                MessageHelper.Success(Message, "System Error!!.");
            }
            finally
            {
                Connection.Close();
            }
            return Message;
        }

        public Message DeleteTransactionLimit(string pvc_limitslno, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var msg = limitRepository.DeleteTransactionLimit(pvc_limitslno, session.User.user_id);
                if (msg.pvc_status == "40999")
                {
                    MessageHelper.Success(Message, msg.pvc_statusmsg);
                }
                else
                {
                    MessageHelper.Error(Message, msg.pvc_statusmsg);
                }
            }
            catch (Exception ex)
            {

                MessageHelper.Success(Message, "System Error!!.");
            }
            finally
            {
                Connection.Close();
            }
            return Message;
        }

        public LimitPackage GetLimitDetails(string limit_id, string pvc_appuser)
        {
            try
            {
                var limitPackage = limitRepository.GetLimitPackages(limit_id, pvc_appuser).FirstOrDefault();
                if (limitPackage != null)
                {
                    var dtGlobalLimit = limitRepository.GetLimitDetails(limit_id, "00", pvc_appuser).FirstOrDefault();
                    if (dtGlobalLimit != null)
                    {
                        limitPackage.maxtransno00 = dtGlobalLimit.max_trans_no;
                        limitPackage.tottransamt00 = dtGlobalLimit.tot_trans_amount;
                        limitPackage.maxtransamt00 = dtGlobalLimit.max_trans_amount;
                    }
                    dtGlobalLimit = limitRepository.GetLimitDetails(limit_id, "01", pvc_appuser).FirstOrDefault();
                    if (dtGlobalLimit != null)
                    {
                        limitPackage.maxtransno01 = dtGlobalLimit.max_trans_no;
                        limitPackage.tottransamt01 = dtGlobalLimit.tot_trans_amount;
                        limitPackage.maxtransamt01 = dtGlobalLimit.max_trans_amount;
                    }
                    dtGlobalLimit = limitRepository.GetLimitDetails(limit_id, "02", pvc_appuser).FirstOrDefault();
                    if (dtGlobalLimit != null)
                    {
                        limitPackage.maxtransno02 = dtGlobalLimit.max_trans_no;
                        limitPackage.tottransamt02 = dtGlobalLimit.tot_trans_amount;
                        limitPackage.maxtransamt02 = dtGlobalLimit.max_trans_amount;
                    }
                    dtGlobalLimit = limitRepository.GetLimitDetails(limit_id, "05", pvc_appuser).FirstOrDefault();
                    if (dtGlobalLimit != null)
                    {
                        limitPackage.maxtransno05 = dtGlobalLimit.max_trans_no;
                        limitPackage.tottransamt05 = dtGlobalLimit.tot_trans_amount;
                        limitPackage.maxtransamt05 = dtGlobalLimit.max_trans_amount;
                    }
                }
                return limitPackage;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
            }
        }

        public IEnumerable<LimitPackage> GetLimitPackageList(string pvc_appuser)
        {
            return limitRepository.GetLimitPackageList(pvc_appuser);
        }

        public IEnumerable<LimitPackage> GetLimitPackages(string pvc_limitid, string pvc_appuser)
        {
            return limitRepository.GetLimitPackages(pvc_limitid, pvc_appuser);
        }

        public IEnumerable<LimitPackage> GetUnauthLimitPackages(string pvc_appuser)
        {
            return limitRepository.GetUnauthLimitPackages(pvc_appuser);
        }

        public IEnumerable<LimitPackage> GetUnauthTransLimit(string pvc_appuser)
        {
            return limitRepository.GetUnauthTransLimit(pvc_appuser);
        }

        public Message SetLimitPackage(LimitPackage limitPackage, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var msg = limitRepository.SetLimitPackage(limitPackage, session.User.user_id);
                if (msg.pvc_status == "40999")
                {
                    if (string.IsNullOrEmpty(limitPackage.limit_id))
                    {
                        MessageHelper.Success(Message, "Package Created Successfully.Package ID:" + msg.pvc_limitid + " Authorization waiting...");
                    }
                    else
                    {
                        MessageHelper.Success(Message, "Package Updates Successfully.Package ID:" + msg.pvc_limitid + " Authorization waiting...");
                    }

                }
                else
                {
                    MessageHelper.Error(Message, msg.pvc_statusmsg);
                }
            }
            catch (Exception ex)
            {

                MessageHelper.Error(Message, ex.Message);
            }
            finally
            {
                Connection.Close();
            }
            return Message;
        }

        public Message SetTransLimitPackage(LimitPackage limitPackage, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                limitPackage.limit_slno = "";
                limitPackage.exp_date = "";
                var msg = limitRepository.SetTransLimitPackage(limitPackage, session.User.user_id);
                if (msg.pvc_status == "40999")
                {
                    if (string.IsNullOrEmpty(limitPackage.limit_slno))
                    {
                        MessageHelper.Success(Message, "Limit Package Assigned Successfully.Limit SLNO:" + msg.pvc_limitslno + "Authorization waiting...");
                    }
                    else
                    {
                        MessageHelper.Success(Message, "Limit Package Assigned Successfully.Limit SLNO:" + msg.pvc_limitslno + "Authorization waiting...");
                    }

                }
                else
                {
                    MessageHelper.Error(Message, msg.pvc_statusmsg);
                }
            }
            catch (Exception ex)
            {

                MessageHelper.Success(Message, "System Error!!.");
            }
            finally
            {
                Connection.Close();
            }
            return Message;
        }

        public LimitPackage SubmitTransactionLimitPackage(LimitPackage limitPackage, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                if (limitPackage.limit_scope == "S")
                {
                    if (limitPackage.usertype == "04")
                    {
                        var dtCustomerDtl = "";//accountRepository.GeAccountDetails(limitPackage.user_ref_no, session.User.user_id).FirstOrDefault();
                        if (dtCustomerDtl != null)
                        {
                            //limitPackage.user_name = " - " + dtCustomerDtl.ac_desc;
                            MessageHelper.Success(Message, "Success");
                        }
                        else
                        {
                            MessageHelper.Error(Message, "Invalid Customer Account No or account is not active.");
                        }
                    }
                    else if (limitPackage.usertype == "03")
                    {
                        var dtCustomerDtl = ""; //accountOpeningRepository.GetAgentList(limitPackage.user_ref_no, session.User.user_id).FirstOrDefault();
                        if (dtCustomerDtl != null)
                        {
                            // limitPackage.user_name = dtCustomerDtl.AGENT_NAME;
                            MessageHelper.Success(Message, "Success");
                        }
                        else
                        {
                            MessageHelper.Error(Message, "Invalid Agent ID");
                        }
                    }
                }
                else
                {
                    MessageHelper.Success(Message, "Success");
                }

            }
            catch (Exception ex)
            {

                MessageHelper.Error(Message, "System Error!!.");
            }
            finally
            {
                Connection.Close();
                limitPackage.message = Message;
            }
            return limitPackage;
        }
    }

    public interface ILimitManager
    {
        IEnumerable<LimitPackage> GetUnauthLimitPackages(string pvc_appuser);
        Message AuthorizeLimitPackage(List<string> pvc_limitid, AppSession session);
        Message DeleteLimitPackage(string pvc_limitid, AppSession session);
        IEnumerable<LimitPackage> GetLimitPackages(string pvc_limitid, string pvc_appuser);
        Message SetLimitPackage(LimitPackage limitPackage, AppSession session);
        LimitPackage GetLimitDetails(string limit_id, string pvc_appuser);
        IEnumerable<LimitPackage> GetUnauthTransLimit(string pvc_appuser);
        Message DeleteTransactionLimit(string pvc_limitslno, AppSession session);
        Message SetTransLimitPackage(LimitPackage limitPackage, AppSession session);
        Message AuthorizeTransLimit(List<string> pvc_limitslno, AppSession session);
        LimitPackage SubmitTransactionLimitPackage(LimitPackage limitPackage, AppSession session);
        IEnumerable<LimitPackage> GetLimitPackageList(string pvc_appuser);
    }
}
