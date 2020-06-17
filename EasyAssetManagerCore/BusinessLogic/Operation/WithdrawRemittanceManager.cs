using EasyAssetManagerCore.BusinessLogic.Common;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EasyAssetManagerCore.BusinessLogic.Operation
{
    public class WithdrawRemittanceManager : BaseService, IWithdrawRemittanceManager
    {
        private readonly ICbsDataConnectionManager cbsDataConnectionManager;
        private readonly ITransactionRepository transactionRepository;
        private readonly ICommonManager commonManager;
        private readonly ICommonRepository commonRepository;
        public WithdrawRemittanceManager() : base((int)ConnectionStringEnum.EbankConnectionString)
        {
            cbsDataConnectionManager = new CbsDataConnectionManager();
            transactionRepository = new TransactionRepository(Connection);
            commonManager = new CommonManager();
            commonRepository = new CommonRepository(Connection);
        }

        public IEnumerable<RemittanceCompany> GetRemittanceCompany(string pvc_remitcompid, string pvc_appuser)
        {
            return commonRepository.GetRemittanceCompany(pvc_remitcompid, pvc_appuser);
        }
        public Message InitiateTransaction(Remittance remittance, AppSession session, IHttpContextAccessor contextAccessor)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                session.TransactionSession.TransactionType = "07";
                session.TransactionSession.TransactionAmount = remittance.amount;
                session.TransactionSession.TransactionAccountNo = session.TransactionSession.RemittanceAccount;
                var msg = transactionRepository.InitiateTransaction(session.TransactionSession.TransactionType, remittance.amount,
                    session.TransactionSession.RemittanceAccount, session.User.agent_cust_ac_no,
                    remittance.remit_sec_code, remittance.remit_receiver_name, remittance.remit_sex,
                    remittance.remit_receiver_address, remittance.remit_receiver_mobile, remittance.remit_receiver_type, remittance.remit_receiver_accountno,
                    remittance.remit_sender_name, remittance.remit_sender_country, remittance.remit_comp_id, remittance.remarks
                    , session.User.agent_id, session.User.outlet_id, session.User.user_id);
                if (msg.pvc_status == "40999")
                {
                    session.TransactionSession.TransactionID = msg.pvc_transid;
                    //usrSess.TransactionID = lblTrnRefNoVal.Text = msg[2];
                    MessageHelper.Success(Message, "Transaction Initiated Successfully. Authorization pending.");
                    //TODO:Need adjust logic in Controller
                    //if (!SetPhoto(usrSess.TransactionID, hf_image_path.Value, usrSess.TransactionCustPhotoType))
                    //{
                    //    showMessage("E", "Unable to save photo. Save photo again.");
                    //}
                    //else
                    //{
                    //    showMessage("C", "Customer photo uploaded successfully.");
                    //}
                }
                else
                {
                    MessageHelper.Error(Message, msg.pvc_statusmsg);
                }
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "WithdrawRemittanceManager-InitiateTransaction", ex.Message + "|" + ex.StackTrace.TrimStart());
                MessageHelper.Success(Message, "System Error!!.");
            }
            finally
            {
                Connection.Close();
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
            }


            return Message;

        }

        public Remittance ComfirmTransaction(Remittance remittance, AppSession session, IHttpContextAccessor contextAccessor)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                if (!string.IsNullOrEmpty(remittance.customer_image_path))
                {
                    string[] photoPathSplitted = remittance.customer_image_path.Split('.');
                    session.TransactionSession.TransactionCustPhotoType = photoPathSplitted[photoPathSplitted.Length - 1].Split('?')[0];
                }

                var remittanceCompany = commonRepository.GetRemittanceCompany(remittance.remit_comp_id, session.User.user_id).FirstOrDefault();

                if (remittanceCompany != null)
                {
                    session.TransactionSession.RemittanceAccount = remittanceCompany.cust_ac_no;
                    MessageHelper.Success(Message, "Check Successfully.");
                }
                else
                {
                    session.TransactionSession.RemittanceAccount = "";

                    //lblSecurityCodeVal.Text = txtSecurityCode.Text;
                    //lblReceiverNameVal.Text = txtReceiverName.Text;
                    //lblReceiverAddressVal.Text = txtReceiverAddress.Text;
                    //lblReceiverMobileVal.Text = txtReceiverMobile.Text;
                    //lblSenderNameVal.Text = txtSenderName.Text;
                    //lblSenderCountryVal.Text = txtSenderCountry.Text;
                    //lblAmountVal.Text = txtAmount.Text;
                    //lblRemittanceCompanyVal.Text = ddlRemittanceCompany.SelectedItem.Text;
                    //lblRemarksVal.Text = txtRemarks.Text;
                    MessageHelper.Error(Message, "Company Not Found.");
                }
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "WithdrawRemittanceManager-ComfirmTransaction", ex.Message + "|" + ex.StackTrace.TrimStart());
                MessageHelper.Error(Message, "System Error.");
            }
            finally
            {
                Connection.Close();
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
                remittance.Message = Message;
            }
            return remittance;
        }

        public IEnumerable<Remittance> GetUnathRemittance(string pvc_transid, string pvc_appuser)
        {
            return transactionRepository.GetUnathRemittance(pvc_transid, pvc_appuser);
        }

        public Message SetRemittanceAmount(string pvc_transid, string pvc_amount, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                var msg = transactionRepository.SetRemittanceAmount(pvc_transid, pvc_amount, session.User.user_id);
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

        public Message DeclineTransaction(string trans_id, string reason, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                var msg = transactionRepository.SetTransactionIo(trans_id, "", "", "000", reason, "", session.User.user_id);
                if (msg.pvc_status == "40999")
                {
                    msg = transactionRepository.SetTransactionStatus(DateTime.Now.ToString(), trans_id, "50800", session.User.user_id);
                    if (msg.pvc_status == "40999")
                    {
                        MessageHelper.Success(Message, msg.pvc_statusmsg);
                    }
                    else
                    {
                        MessageHelper.Error(Message, msg.pvc_statusmsg);
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

        public Message AuthorizeRemittance(string trans_id, string agent_id, string outlet_id, AppSession session)
        {
            try
            {
                var msg = new ResponseMessage();
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                var transactionXml = transactionRepository.GetTransactionXml(trans_id, agent_id, outlet_id, session.User.user_id);
                if (transactionXml != null)
                {
                    session.TransactionSession.TransactionID = transactionXml.trans_id;
                    session.TransactionSession.TransactionAccountNo = transactionXml.from_account;
                    session.TransactionSession.TransactionAmount = transactionXml.VALUEDT;
                    session.TransactionSession.TransactionDate = transactionXml.AMOUNT;
                    msg = transactionRepository.SetTransactionStatus(session.TransactionSession.TransactionDate, trans_id, "50200", session.User.user_id);

                    if (msg.pvc_status == "40999")
                    {
                        string[] trnErr;
                        string inputXmlString, outputXmlString;
                        var trnResp = cbsDataConnectionManager.ProcessTransaction(session.TransactionSession.TransactionID, "07", "C", transactionXml, out trnErr, out inputXmlString, out outputXmlString);

                        if (trnResp.pvc_status == "40999")
                        {
                            session.TransactionSession.UbsTransactionRefNo = trnResp.pvc_transid;
                            msg = transactionRepository.SetTransactionStatus("04", session.TransactionSession.TransactionAccountNo,
                                session.TransactionSession.TransactionDate, trans_id, "07", session.TransactionSession.TransactionAmount, "50999", agent_id, session.User.user_id);
                            MessageHelper.Success(Message, "Transaction Completed Successfully.");
                            string[] smsResp = commonManager.SendSms(transactionXml.bene_mobile, "Dear Customer, Your remittance transaction has been completed with txnid " + trans_id + ". Please collect BDT " + session.TransactionSession.TransactionAmount + " from agent outlet.");
                        }
                        else
                        {
                            msg = transactionRepository.SetTransactionStatus(session.TransactionSession.TransactionDate, trans_id, "50900", session.User.user_id);
                            MessageHelper.Error(Message, string.Join(",", trnErr));
                        }

                        msg = transactionRepository.SetTransactionIo(trans_id, inputXmlString, outputXmlString, trnErr[0], trnErr[1], session.TransactionSession.UbsTransactionRefNo, session.User.user_id);
                    }
                    else
                    {
                        MessageHelper.Error(Message, msg.pvc_statusmsg);
                    }
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

    }

    public interface IWithdrawRemittanceManager
    {
        IEnumerable<RemittanceCompany> GetRemittanceCompany(string pvc_remitcompid, string pvc_appuser);
        Message InitiateTransaction(Remittance remittance, AppSession session, IHttpContextAccessor contextAccessor);
        Remittance ComfirmTransaction(Remittance remittance, AppSession session, IHttpContextAccessor contextAccessor);
        IEnumerable<Remittance> GetUnathRemittance(string pvc_transid, string pvc_appuser);
        Message SetRemittanceAmount(string pvc_transid, string pvc_amount, AppSession session);
        Message DeclineTransaction(string trans_id, string reason, AppSession session);
        Message AuthorizeRemittance(string trans_id, string agent_id, string outlet_id, AppSession session);
    }
}
