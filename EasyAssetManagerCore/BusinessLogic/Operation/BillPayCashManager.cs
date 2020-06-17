using CardService;
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
using System.Text;
using static CardService.EBL_UBPSoapClient;

namespace EasyAssetManagerCore.BusinessLogic.Operation
{
    public class BillPayCashManager : BaseService, IBillPayCashManager
    {
        private readonly ICbsDataConnectionManager cbsDataConnectionManager;
        private readonly ITransactionRepository transactionRepository;
        private readonly ICommonManager commonManager;
        private readonly ICommonRepository commonRepository;
        public BillPayCashManager() : base((int)ConnectionStringEnum.EbankConnectionString)
        {
            cbsDataConnectionManager = new CbsDataConnectionManager();
            transactionRepository = new TransactionRepository(Connection);
            commonManager = new CommonManager();
            commonRepository = new CommonRepository(Connection);
        }

        public IEnumerable<Biller> GetBillerDetail(string pvc_billerid, string pvc_appuser)
        {
            return commonRepository.GetBillerDetail(pvc_billerid, pvc_appuser);
        }
        public BillPayCash InitiateTransaction(BillPayCash billPayCash, AppSession session, IHttpContextAccessor contextAccessor)
        {
            try
            {
                //TODO: Incorporate Service
                //lblCustomerNameVal.Text = txtCustomerName.Text;
                //lblCustomerMobileVal.Text = txtCustomerMobile.Text;
                //lblBillerVal.Text = usrSess.BillerDesc = ddlBiller.SelectedItem.Text;
                //usrSess.BillerID = ddlBiller.SelectedValue;
                //lblBillRefNoVal.Text = usrSess.BillRefNo = txtBillRefNo.Text;
                //lblBillDetailsVal.Text = usrSess.BillDetails = txtBillDetails.Text;
                //lblBillDateVal.Text = usrSess.TransactionDate = txtBillDate.Text;
                //lblAmountVal.Text = usrSess.TransactionAmount = txtAmount.Text;
                //usrSess.TransactionAccountNo = usrSess.AgentAccount;
                //lblChargeVal.Text = lblVatVal.Text = lblTotalVal.Text = "";
                bool valid = true;
                if (billPayCash.biller_id == "BC0019" || billPayCash.biller_id == "BC0023" || billPayCash.biller_id == "BC0030" || billPayCash.biller_id == "BC0033")
                {
                   // lblBillRefNoVal.Text = billPayCash.bill_ref_no.Substring(0, 6) + "******" + billPayCash.bill_ref_no.Substring(10);

                    EBL_UBPSoapClient CardServiceManager = new EBL_UBPSoapClient(EndpointConfiguration.EBL_UBPSoap);

                    
                    CARDSTATUSDETAIL_PASS usrPass = new CARDSTATUSDETAIL_PASS();
                    usrPass.USERNAME = ApplicationConstant.CardServiceUserId;
                    usrPass.PASSWORD = ApplicationConstant.CardServiceUserPassword;
                    //if (configReader.Value("AppSettings", "AppMode") == "Live")
                    //{
                    //    usrPass.USERNAME = configReader.Value("CardServiceLiveSettings", "UserId");
                    //    usrPass.PASSWORD = conn.Decrypt(configReader.Value("CardServiceLiveSettings", "Password"));
                    //}
                    //else
                    //{
                    //    usrPass.USERNAME = configReader.Value("CardServiceTestSettings", "UserId");
                    //    usrPass.PASSWORD = conn.Decrypt(configReader.Value("CardServiceTestSettings", "Password"));
                    //}

                    CARDSTATUSDETAIL cardDtl = new CARDSTATUSDETAIL();
                    cardDtl.REFERENCE_NUMBER = "123456789123";
                    cardDtl.CARD_NUMBER = billPayCash.bill_ref_no;
                    cardDtl.CARDSTATUSDETAIL_PASS = usrPass;

                    CARDSTATUSDETAILRESP CardDetl = CardServiceManager.CardstatDetailAsync(cardDtl).Result.Body.CardstatDetailResult;

                    if (CardDetl.ERROR_CODE == "000")
                    {
                        if (CardDetl.PRODUCT_TYPE == "DC")
                        {
                            valid = false;
                            MessageHelper.Error(Message, "Debit card can not be used");
                        }
                        else if ((billPayCash.biller_id == "BC0019" || billPayCash.biller_id == "BC0023") && CardDetl.PRODUCT_TYPE != "CR")
                        {
                            valid = false;
                            MessageHelper.Error(Message, "Invalid credit card. May be you have provided wrong credit card no.");
                        }
                        else if ((billPayCash.biller_id == "BC0030" || billPayCash.biller_id == "BC0033") && CardDetl.PRODUCT_TYPE != "PP")
                        {
                            MessageHelper.Error(Message, "Invalid prepaid card. May be you have provided wrong prepaid card no.");
                            valid = false;
                        }
                        else
                        {
                            billPayCash.cardName= CardDetl.NAME;
                            billPayCash.productName = CardDetl.PRODUCT_NAME;
                            // lblCardNameVal.Text = CardDetl.NAME;
                            //  lblCardProductVal.Text = CardDetl.PRODUCT_NAME;
                        }
                    }
                    else
                    {
                        valid = false;
                        MessageHelper.Error(Message, CardDetl.ERROR_RESPONSE);
                    }
                }
                if (valid)
                {
                    if (Connection.State != ConnectionState.Open)
                        Connection.Open();
                    session.TransactionSession.TransactionType = "03";
                    session.TransactionSession.TransactionAmountVat = billPayCash.transactionAmountVat;
                    session.TransactionSession.CustomerName = billPayCash.customerName;
                    session.TransactionSession.TransactionCustMobileNo = billPayCash.customerMobile;
                    session.TransactionSession.TransactionAmount = billPayCash.transactionAmount;
                    session.TransactionSession.BillRef = billPayCash.bill_ref_no;
                    session.TransactionSession.TransactionDate=billPayCash.transactionDate;
                    session.TransactionSession.TransactionAccountNo = session.User.agent_cust_ac_no;
                   var msg = transactionRepository.InitiateTransaction(session.TransactionSession.TransactionType, billPayCash.transactionAmount,
                        "", billPayCash.biller_id, billPayCash.bill_ref_no, billPayCash.billDetails, "", billPayCash.customerName,
                        billPayCash.sex, billPayCash.customerType, billPayCash.customerAccount,
                        billPayCash.customerMobile, session.User.agent_cust_ac_no
                        , session.User.agent_id, session.User.outlet_id, session.User.user_id);
                    if (msg.pvc_status == "40999")
                    {
                        session.TransactionSession.TransactionID = msg.pvc_transid;

                        session.TransactionSession.TransactionCharge = msg.pvc_charge;
                        session.TransactionSession.TransactionChargeVat = msg.pvc_chargevat;
                        session.TransactionSession.TransactionTotalAmount = msg.pvc_totalamount;

                        billPayCash.transactionCharge = msg.pvc_charge;
                        billPayCash.transactionChargeVat = msg.pvc_chargevat;
                        billPayCash.transactionTotalAmount = msg.pvc_totalamount;
                        //lblChargeVal.Text = usrSess.TransactionCharge = msg[3];
                        //lblVatVal.Text = usrSess.TransactionChargeVat = msg[4];
                        //lblTotalVal.Text = usrSess.TransactionTotalAmount = msg[5];
                        //usrSess.TransactionID = lblTrnRefNoVal.Text = msg[2];
                        MessageHelper.Success(Message, "Transaction Initiated Successfully.");
                        billPayCash.bill_ref_no = billPayCash.bill_ref_no.Substring(0, 6) + "******" + billPayCash.bill_ref_no.Substring(10);
                    }
                    else
                    {
                        MessageHelper.Error(Message, msg.pvc_statusmsg);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "BillPayCashManager-InitiateTransaction", ex.Message + "|" + ex.StackTrace.TrimStart());
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
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "BillPayCashManager-CompleteTransaction", ex.Message + "|" + ex.StackTrace.TrimStart());
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

    public interface IBillPayCashManager
    {
        IEnumerable<Biller> GetBillerDetail(string pvc_billerid, string pvc_appuser);
        BillPayCash InitiateTransaction(BillPayCash billPayCash, AppSession session, IHttpContextAccessor contextAccessor);
        Message CompleteTransaction(AppSession session, IHttpContextAccessor contextAccessor);
    }
}
