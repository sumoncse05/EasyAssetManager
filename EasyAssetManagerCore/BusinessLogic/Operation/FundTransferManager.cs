using EasyAssetManagerCore.BusinessLogic.Common;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Data;
using System.Linq;

namespace EasyAssetManagerCore.BusinessLogic.Operation
{
    public class FundTransferManager : BaseService, IFundTransferManager
    {
        private readonly ICbsDataConnectionManager cbsDataConnectionManager;
        private readonly ITransactionRepository transactionRepository;
        private readonly ICommonManager commonManager;
        public FundTransferManager() : base((int)ConnectionStringEnum.EbankConnectionString)
        {
            cbsDataConnectionManager = new CbsDataConnectionManager();
            transactionRepository = new TransactionRepository(Connection);
            commonManager = new CommonManager();
        }

        public FundTransfer InitiateTransaction(FundTransfer fundTransfer, AppSession session, IHttpContextAccessor contextAccessor)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                if (string.IsNullOrEmpty(fundTransfer.fromAccountNo))
                {
                    MessageHelper.Error(Message, "Enter from account no to continue.");
                    fundTransfer.Message = Message;
                    return fundTransfer;
                }
                if (string.IsNullOrEmpty(fundTransfer.alternateAccountNo))
                {
                    MessageHelper.Error(Message, "Enter to account no to continue.");
                    fundTransfer.Message = Message;
                    return fundTransfer;
                }
                if (fundTransfer.fromAccountNo == fundTransfer.alternateAccountNo)
                {
                    MessageHelper.Error(Message, "From account and to account can not be same.");
                    fundTransfer.Message = Message;
                    return fundTransfer;
                }

                var fromAccountStatus = transactionRepository.GetAccountStatus(fundTransfer.fromAccountNo, "05", "C", session.User.user_id);
                var toAccountStatus = transactionRepository.GetAccountStatus(fundTransfer.alternateAccountNo, "05", "", session.User.user_id);

                if (fromAccountStatus.pvc_status == "40900")
                {
                    MessageHelper.Error(Message, fromAccountStatus.pvc_statusmsg);
                }
                else if (toAccountStatus.pvc_status == "40900")
                {
                    MessageHelper.Error(Message, toAccountStatus.pvc_statusmsg);
                }
                else
                {
                    //lblFromAccountNoVal.Text = usrSess.TransactionAccountNo = txtFromAccountNo.Text;
                    //lblFromAccountDescVal.Text = usrSess.TransactionAccountDesc = FromAccountStatus[2];
                    //usrSess.AccountOperatingMode = Convert.ToInt32(FromAccountStatus[3]);
                    //lblToAccountNoVal.Text = usrSess.AlternateAccountNo = txtToAccountNo.Text;
                    //lblToAccountDescVal.Text = usrSess.AlternateAccountDesc = ToAccountStatus[2];
                    //lblAmountVal.Text = usrSess.TransactionAmount = txtAmount.Text;
                    //lblChargeVal.Text = lblVatVal.Text = lblTotalVal.Text = "";
                    //usrSess.CustomerValidated = 0;

                    session.TransactionSession.TransactionAccountNo = fundTransfer.fromAccountNo;
                    session.TransactionSession.TransactionAmount = fundTransfer.transactionAmount;
                    session.TransactionSession.TransactionAccountDesc = fromAccountStatus.pvc_acdesc;
                    session.TransactionSession.AccountOperatingMode = Convert.ToInt32(fromAccountStatus.pvc_operationmode);
                    session.TransactionSession.AlternetAccountNo = fundTransfer.alternateAccountNo;
                    session.TransactionSession.AlternetAccountDesc = toAccountStatus.pvc_acdesc;
                    fundTransfer.accountOperatingMode= Convert.ToInt32(fromAccountStatus.pvc_operationmode);
                    fundTransfer.alternateAccountDesc = toAccountStatus.pvc_acdesc;
                    fundTransfer.fromTransactionAccountDesc = fromAccountStatus.pvc_acdesc;
                    session.TransactionSession.TransactionType = "05";
                    session.TransactionSession.CustomerValidated = 0;

                    var msg = transactionRepository.InitiateTransaction(session.TransactionSession.TransactionType, session.TransactionSession.TransactionAmount, session.TransactionSession.TransactionAccountNo,
                        session.TransactionSession.TransactionAccountDesc, fundTransfer.alternateAccountNo, fundTransfer.alternateAccountDesc,
                        "", "", "", session.User.agent_id, session.User.outlet_id, session.User.user_id);
                    if (msg.pvc_status == "40999")
                    {
                        session.TransactionSession.TransactionID = msg.pvc_transid;
                        session.TransactionSession.TransactionCharge = msg.pvc_charge;
                        session.TransactionSession.TransactionChargeVat = msg.pvc_chargevat;
                        session.TransactionSession.TransactionTotalAmount = msg.pvc_totalamount;

                        fundTransfer.transactionCharge = msg.pvc_charge;
                        fundTransfer.transactionChargeVat = msg.pvc_chargevat;
                        fundTransfer.transactionTotalAmount = msg.pvc_totalamount;

                        MessageHelper.Success(Message, "Transaction Initiate Successfully.");
                    }
                    else
                    {
                        MessageHelper.Error(Message, msg.pvc_statusmsg);
                    }
                }

            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "FundTransferManager-InitiateTransaction", ex.Message + "|" + ex.StackTrace.TrimStart());
                MessageHelper.Error(Message, "System Error.");
            }
            finally
            {
                Connection.Close();
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
                fundTransfer.Message = Message;
            }
            return fundTransfer;


        }

        public Message GenerateFingerRequest(FundTransfer fundTransfer, AppSession session, IHttpContextAccessor contextAccessor)
        {
            try
            {
                if (string.IsNullOrEmpty(fundTransfer.customer_no))
                {
                    MessageHelper.Error(Message, "Select a customer to verify Finger.");
                }
                else
                {
                    var customer = session.TransactionSession.Customers.Where(o => o.customer_no == fundTransfer.customer_no).FirstOrDefault();
                    session.TransactionSession.TransactionCustomerNo = customer.customer_no;
                    session.TransactionSession.TransactionCustMobileNo = customer.mobile_number;
                    var reqResp = commonManager.RequestFingerScan("V", "04", session.TransactionSession.TransactionCustomerNo, session.TransactionSession.TransactionID, session.User.user_id, session.User.StationIp);

                    if (reqResp.pvc_status == "40999")
                    {
                        session.TransactionSession.FingerReqRefNo = reqResp.pvc_otpreqrefno;
                        MessageHelper.Success(Message, "Finger Scan Request Generated successfully. Scan Finger to Verify.");
                    }
                    else
                    {
                        MessageHelper.Error(Message, reqResp.pvc_statusmsg);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "FundTransferManager-GenerateFingerRequest", ex.Message + "|" + ex.StackTrace.TrimStart());
            }
            finally
            {
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
            }
            return Message;
        }

        public ResponseMessage ReGenerateFingerRequest(AppSession session, IHttpContextAccessor contextAccessor)
        {
            var retMsg = new ResponseMessage();
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var msg = commonManager.GetFingerScanStatus(session.TransactionSession.FingerReqRefNo, session.User.user_id, session.User.StationIp);
                if (msg.pvc_status == "40999" && msg.pvc_statusmsg == "S")
                {
                    retMsg.pvc_statusmsg = "Can not Re-Generate. Already Verified. Refreh.";
                }
                else
                {
                    retMsg = commonManager.RequestFingerScan("V", "04", session.TransactionSession.TransactionCustomerNo, session.TransactionSession.TransactionID, session.User.user_id, session.User.StationIp);
                    if (retMsg.pvc_status == "40999")
                    {
                        session.TransactionSession.FingerReqRefNo = retMsg.pvc_otpreqrefno;
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "FundTransferManager-ReGenerateFingerRequest", ex.Message + "|" + ex.StackTrace.TrimStart());
            }
            finally
            {
                Connection.Close();
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
            }
            return retMsg;

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
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "FundTransferManager-GetFingerVerifyStatus", ex.Message + "|" + ex.StackTrace.TrimStart());
            }
            finally
            {
                Connection.Close();
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
            }
            return msg;

        }

        public Message CompleteTransaction(AppSession session, IHttpContextAccessor contextAccessor)
        {
            try
            {
                var msg = new ResponseMessage();
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                //TODO: Incorporate in HTML
                //if (hf_otp_status.Value == "1")
                //{
                //    lblOtpStatus.Text = "Verified Successfully";
                //    lblOtpStatus.ForeColor = System.Drawing.Color.Green;
                //    lnkGenerate.Visible = false;
                //}
                var transactionXml = transactionRepository.GetTransactionXml(session.TransactionSession.TransactionID, session.User.agent_id, session.User.outlet_id, session.User.user_id);
                if (transactionXml != null)
                {
                    msg = transactionRepository.SetTransactionStatus(session.TransactionSession.TransactionDate, session.TransactionSession.TransactionID, "50200", session.User.user_id);

                    if (msg.pvc_status == "40999")
                    {
                        string[] trnErr;
                        string inputXmlString, outputXmlString;
                        var trnResp = cbsDataConnectionManager.ProcessTransaction(session.TransactionSession.TransactionID, "05", "C", transactionXml, out trnErr, out inputXmlString, out outputXmlString);

                        if (trnResp.pvc_status == "40999")
                        {
                            session.TransactionSession.UbsTransactionRefNo = trnResp.pvc_transid;
                            // lblTrnRefNoVal.Text = trnResp[2]
                            // lnkPrint.Visible = true;
                            msg = transactionRepository.SetTransactionStatus("04", session.TransactionSession.TransactionAccountNo,
                                session.TransactionSession.TransactionDate, session.TransactionSession.TransactionID, "05", session.TransactionSession.TransactionAmount, "50999", session.User.agent_id, session.User.user_id);

                            session.User.AgentBalance = -1 * Convert.ToDecimal(session.TransactionSession.TransactionAmount);
                            MessageHelper.Success(Message, "Transaction Completed Successfully.");
                        }
                        else
                        {
                            msg = transactionRepository.SetTransactionStatus(session.TransactionSession.TransactionDate, session.TransactionSession.TransactionID, "50900", session.User.user_id);
                            MessageHelper.Error(Message, string.Join(",", trnErr));
                        }

                        msg = transactionRepository.SetTransactionIo(session.TransactionSession.TransactionID, inputXmlString, outputXmlString, trnErr[0], trnErr[1], session.TransactionSession.UbsTransactionRefNo, session.User.user_id);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "FundTransferManager-CompleteTransaction", ex.Message + "|" + ex.StackTrace.TrimStart());
                MessageHelper.Error(Message, "System Error.");
            }
            finally
            {
                Connection.Close();
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
            }
            return Message;


        }

        public Message ReRequestSmsOtp(AppSession session, IHttpContextAccessor contextAccessor)
        {
            var retMsg = new ResponseMessage();

            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var msg = commonManager.RequestSmsOtp("04", session.TransactionSession.TransactionCustomerNo, session.TransactionSession.TransactionCustMobileNo, session.TransactionSession.TransactionID, session.User.user_id, session.User.StationIp);
                if (msg[0] == "40999")
                {
                    session.TransactionSession.SmsReqRefNo = msg[2];
                    session.TransactionSession.SmsOtpData = msg[3];
                    MessageHelper.Success(Message, "OTP Resend successfully.");
                }
                else
                {
                    retMsg = new ResponseMessage { pvc_status = "40900", pvc_statusmsg = "Unable to Send OTP SMS. Please try again" };
                    MessageHelper.Error(Message, "Unable to Send OTP SMS. Please try again.");
                }
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "FundTransferManager-ReRequestSmsOtp", ex.Message + "|" + ex.StackTrace.TrimStart());
                retMsg = new ResponseMessage { pvc_status = "40900", pvc_statusmsg = "Unable to Send OTP SMS. Please try again" };
                MessageHelper.Error(Message, "Unable to Send OTP SMS. Please try again.");
            }
            finally
            {
                Connection.Close();
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
            }
            return Message;
        }

        public Message RequestSmsOtp(AppSession session, IHttpContextAccessor contextAccessor)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                if (session.TransactionSession.FingerVerifyStatus != "S")
                {
                    MessageHelper.Error(Message, "Finger not yet verified or Verification Failed.");
                }
                else
                {
                    //if (hf_otp_status.Value == "1")
                    //{
                    //lblOtpStatus.Text = "Verified Successfully";
                    //lblOtpStatus.ForeColor = System.Drawing.Color.Green;
                    //lnkGenerate.Visible = false;
                    //}

                    if (!string.IsNullOrEmpty(session.TransactionSession.TransactionCustMobileNo))
                    {
                        var msg = commonManager.RequestSmsOtp("04", session.TransactionSession.TransactionCustomerNo, session.TransactionSession.TransactionCustMobileNo, session.TransactionSession.TransactionID, session.User.user_id, session.User.StationIp);
                        if (msg[0] == "40999")
                        {
                            session.TransactionSession.SmsReqRefNo = msg[2];
                            session.TransactionSession.SmsOtpData = msg[3];
                            MessageHelper.Success(Message, "OTP Send Successfully.");
                        }
                        else
                        {
                            MessageHelper.Error(Message, "Unable to Send OTP SMS. Please try again");
                        }
                    }
                    else
                    {
                        MessageHelper.Error(Message, "Customer Mobile No not found. Please try again.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.Error(Message, "System Error!!!");
            }
            finally
            {
                Connection.Close();
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
            }
            return Message;
        }

        public Message VerifyOtp(string otp, AppSession session, IHttpContextAccessor contextAccessor)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                if (!string.IsNullOrEmpty(session.TransactionSession.SmsReqRefNo))
                {
                    if (!string.IsNullOrEmpty(otp))
                    {
                        session.TransactionSession.SmsOtpVerifyStatus = otp == session.TransactionSession.SmsOtpData ? "S" : "F";
                        var msg = commonManager.SetSmsOtpResponseStatus(session.TransactionSession.SmsReqRefNo, session.TransactionSession.SmsOtpData, otp, session.TransactionSession.SmsOtpVerifyStatus, session.User.user_id, session.User.StationIp);

                        if (msg.pvc_status == "40999")
                        {
                            if (session.TransactionSession.SmsOtpVerifyStatus == "S")
                            {
                                session.TransactionSession.CustomerValidated++;
                                //  lblVerificationStatus.Text = session.CustomerValidated.ToString() + " Customer(s) verified out of " + session.FundTransfer.AccountOperatingMode.ToString();
                                //if (session.TransactionSession.CustomerValidated == session.TransactionSession.AccountOperatingMode)
                                //{
                                //    Message = CompleteTransaction(session, contextAccessor);
                                //}
                                //else
                                //{
                                //    //ddlCustomer.Items.RemoveAt(ddlCustomer.SelectedIndex);
                                //    //wzd.MoveTo(this.WizardStep3);
                                //    //wzd.ActiveStepIndex = 2;

                                //    //hf_otp_status.Value = "0";
                                //    //lblOtpStatus.Text = "Not Yet Verified";
                                //    //lblOtpStatus.Attributes.CssStyle.Add("color", "Red");
                                //    //lnkGenerate.Visible = true;
                                //}
                                MessageHelper.Success(Message, "OTP Verify successfully.");
                            }
                            else
                            {
                                MessageHelper.Error(Message, "Invalid OTP. Please try again.");
                                session.TransactionSession.SmsOtpVerifyStatus = "F";
                            }
                        }
                    }
                    else
                    {
                        MessageHelper.Error(Message, "Ask Customer to Check Mobile and Input OTP here.");
                    }
                }
                else
                {
                    MessageHelper.Error(Message, "OTP not yet send. Please send again.");
                }
            }
            catch (Exception ex)
            {
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

    public interface IFundTransferManager
    {
        ResponseMessage GetFingerVerifyStatus(AppSession session, IHttpContextAccessor contextAccessor);
        ResponseMessage ReGenerateFingerRequest(AppSession session, IHttpContextAccessor contextAccessor);
        FundTransfer InitiateTransaction(FundTransfer fundTransfer, AppSession session, IHttpContextAccessor contextAccessor);
        Message GenerateFingerRequest(FundTransfer fundTransfer, AppSession session, IHttpContextAccessor contextAccessor);
       Message ReRequestSmsOtp(AppSession session, IHttpContextAccessor contextAccessor);
        Message RequestSmsOtp(AppSession session, IHttpContextAccessor contextAccessor);
        Message VerifyOtp(string otp, AppSession session, IHttpContextAccessor contextAccessor);
        Message CompleteTransaction(AppSession session, IHttpContextAccessor contextAccessor);
    }
}
