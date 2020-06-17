using EasyAssetManagerCore.BusinessLogic.Common;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Data;

namespace EasyAssetManagerCore.BusinessLogic.Operation
{
    public class BillPayCashPalliBuddyutManager : BaseService, IBillPayCashPalliBuddyutManager
    {
        private readonly ICbsDataConnectionManager cbsDataConnectionManager;
        private readonly ITransactionRepository transactionRepository;
        private readonly ICommonManager commonManager;
        private readonly ICommonRepository commonRepository;
        public BillPayCashPalliBuddyutManager() : base((int)ConnectionStringEnum.EbankConnectionString)
        {
            cbsDataConnectionManager = new CbsDataConnectionManager();
            transactionRepository = new TransactionRepository(Connection);
            commonManager = new CommonManager();
            commonRepository = new CommonRepository(Connection);
        }
        public BillPayCash InitiateTransaction(BillPayCash billPayCash, AppSession session, IHttpContextAccessor contextAccessor)
        {
            try
            {
                //TODO: Incorporate Service
                //lblCustomerNameVal.Text = usrSess.CustomerName = txtCustomerName.Text;
                //lblCustomerMobileVal.Text = txtCustomerMobile.Text;
                //lblBillerVal.Text = usrSess.BillerDesc = ddlBiller.SelectedItem.Text;
                //usrSess.BillerID = ddlBiller.SelectedValue;
                //lblBillRefNoVal.Text = usrSess.BillRefNo = txtBillRefNo.Text;
                ////lblBillDetailsVal.Text = usrSess.BillDetails = txtBillDetails.Text;
                //lblBillMonthVal.Text = usrSess.BillMonth = txtBillMonth.Text;
                //lblAmountVal.Text = usrSess.TransactionAmount = txtAmount.Text;
                //lblAmountVatVal.Text = usrSess.TransactionVatAmount = txtAmountVat.Text;
                //usrSess.TransactionAccountNo = usrSess.AgentAccount;
                //lblChargeVal.Text = lblTotalVal.Text = "";//lblVatVal.Text = 
                bool valid = true;
               
                if (valid)
                {
                    if (Connection.State != ConnectionState.Open)
                        Connection.Open();
                    session.TransactionSession.TransactionCustMobileNo = billPayCash.customerMobile;
                    session.TransactionSession.TransactionAmount = billPayCash.transactionAmount;
                    session.TransactionSession.BillRef = billPayCash.bill_ref_no;
                    session.TransactionSession.TransactionDate = billPayCash.transactionDate;
                    session.TransactionSession.TransactionAmountVat = billPayCash.transactionAmountVat;
                    session.TransactionSession.CustomerName = billPayCash.customerName;
                    session.TransactionSession.TransactionType = "03";
                    var msg = transactionRepository.InitiateTransaction(session.TransactionSession.TransactionType, billPayCash.transactionAmount,
                        billPayCash.transactionAmountVat, billPayCash.biller_id, billPayCash.bill_ref_no, "", billPayCash.transactionDate, billPayCash.customerName,
                        billPayCash.sex, billPayCash.customerType, billPayCash.customerAccount,
                        billPayCash.customerMobile, session.User.agent_cust_ac_no
                        , session.User.agent_id, session.User.outlet_id, session.User.user_id);

                    if (msg.pvc_status == "40999")
                    {
                        session.TransactionSession.TransactionID = msg.pvc_transid;
                        //usrSess.TransactionID = lblTrnRefNoVal.Text = msg[2];
                        session.TransactionSession.TransactionCharge = msg.pvc_charge;
                        session.TransactionSession.TransactionChargeVat = msg.pvc_chargevat;
                        session.TransactionSession.TransactionTotalAmount = msg.pvc_totalamount;

                        billPayCash.transactionCharge = msg.pvc_charge;
                        billPayCash.transactionChargeVat = msg.pvc_chargevat;
                        billPayCash.transactionTotalAmount = msg.pvc_totalamount;
                        billPayCash.stampValue = "0";
                        //lblChargeVal.Text = usrSess.TransactionCharge = msg[3];
                        //lblVatVal.Text = usrSess.TransactionChargeVat = msg[4];
                        //lblTotalVal.Text = usrSess.TransactionTotalAmount = msg[5];
                        if (Convert.ToDecimal(billPayCash.transactionAmount) + Convert.ToDecimal(billPayCash.transactionAmountVat) >= 400)
                            //lblStampVal.Text = "10"; 
                            billPayCash.stampValue = "10";
                        MessageHelper.Success(Message, "Transaction Initiated Successfully.");
                    }
                    else
                    {
                        MessageHelper.Error(Message, msg.pvc_statusmsg);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "BillPayCashPalliBuddyutManager-InitiateTransaction", ex.Message + "|" + ex.StackTrace.TrimStart());
                MessageHelper.Success(Message, "System Error!!.");
            }
            finally
            {
                Connection.Close();
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
                billPayCash.Message = Message;
            }


            return billPayCash;
        }


        public Message CompleteTransaction(AppSession session, IHttpContextAccessor contextAccessor)
        {
            try
            {
                var msg = new ResponseMessage();
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                var transactionXml = transactionRepository.GetTransactionXml(session.TransactionSession.TransactionID, session.User.agent_id, session.User.outlet_id, session.User.user_id);
                if (transactionXml != null)
                {
                    msg = transactionRepository.SetTransactionStatus(session.TransactionSession.TransactionDate, session.TransactionSession.TransactionID, "50200", session.User.user_id);

                    if (msg.pvc_status == "40999")
                    {
                        string[] trnErr;
                        string inputXmlString, outputXmlString;
                        var trnResp = cbsDataConnectionManager.ProcessTransaction(session.TransactionSession.TransactionID, "03", "D", transactionXml, out trnErr, out inputXmlString, out outputXmlString);

                        if (trnResp.pvc_status == "40999")
                        {
                            session.TransactionSession.UbsTransactionRefNo = trnResp.pvc_transid;
                            // lblTrnRefNoVal.Text = trnResp[2]
                            // lnkPrint.Visible = true;
                            msg = transactionRepository.SetTransactionStatus("04", session.TransactionSession.TransactionAccountNo,
                                session.TransactionSession.TransactionDate, session.TransactionSession.TransactionID, "03", session.TransactionSession.TransactionAmount, "50999", session.User.agent_id, session.User.user_id);

                            session.User.AgentBalance = Convert.ToDecimal(session.TransactionSession.TransactionAmount);
                            MessageHelper.Success(Message, "Transaction Completed Successfully.");
                            string[] smsResp = commonManager.SendSms(session.TransactionSession.TransactionCustMobileNo, "Dear Customer, Your Bill Number: " 
                                + session.TransactionSession.BillRef + ", of Month: " + session.TransactionSession.TransactionDate + ", payment request of BDT " + session.TransactionSession.TransactionAmount + 
                                " has been received by EBL Agent Outlet. Thanks. Helpline 16230");
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
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "BillPayCashPalliBuddyutManager-CompleteTransaction", ex.Message + "|" + ex.StackTrace.TrimStart());
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

    public interface IBillPayCashPalliBuddyutManager
    {
        BillPayCash InitiateTransaction(BillPayCash billPayCash, AppSession session, IHttpContextAccessor contextAccessor);
        Message CompleteTransaction(AppSession session, IHttpContextAccessor contextAccessor);
    }
}
