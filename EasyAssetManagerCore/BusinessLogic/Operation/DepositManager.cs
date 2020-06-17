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
    public class DepositManager : BaseService, IDepositManager
    {
        private readonly ICbsDataConnectionManager cbsDataConnectionManager;
        private readonly ITransactionRepository transactionRepository;
        private readonly ICommonManager commonManager;
        public DepositManager() : base((int)ConnectionStringEnum.EbankConnectionString)
        {
            cbsDataConnectionManager = new CbsDataConnectionManager();
            transactionRepository = new TransactionRepository(Connection);
            commonManager = new CommonManager();
        }

        public Deposit ComfirmTransaction(Deposit deposit, AppSession session,IHttpContextAccessor contextAccessor)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                if (string.IsNullOrEmpty(deposit.transactionAccountNo))
                {
                    MessageHelper.Error(Message, "Enter account no to continue.");
                }
                else
                {
                    var AccountStatus = transactionRepository.GetAccountStatus(deposit.transactionAccountNo, "01", "A", session.User.user_id);

                    if (AccountStatus.pvc_status == "40900")
                    {
                        MessageHelper.Error(Message, AccountStatus.pvc_statusmsg);
                    }
                    else
                    {
                        session.TransactionSession.TransactionAccountNo = deposit.transactionAccountNo;
                        session.TransactionSession.TransactionAmount = deposit.transactionAmount;
                        session.TransactionSession.TransactionAccountDesc = AccountStatus.pvc_acdesc;
                        deposit.transactionAccountDesc= AccountStatus.pvc_acdesc;
                        var msg = transactionRepository.GetTransactionCharges("01", deposit.transactionAccountNo, deposit.transactionAmount, session.User.agent_id, session.User.outlet_id, "", session.User.user_id);
                        session.TransactionSession.TransactionCharge = msg.pnm_commamt;
                        session.TransactionSession.TransactionChargeVat = msg.pnm_vatamt;
                        session.TransactionSession.TransactionTotalAmount = msg.pnm_totalamt;

                        deposit.transactionCharge = msg.pnm_commamt;
                        deposit.transactionChargeVat = msg.pnm_vatamt;
                        deposit.transactionTotalAmount = msg.pnm_totalamt;
                        MessageHelper.Success(Message, "Transaction Confirm. You Can Proceed.");
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "DepositManager-ComfirmTransaction", ex.Message + "|" + ex.StackTrace.TrimStart());
                MessageHelper.Error(Message, "System Error.");
            }
            finally
            {
                Connection.Close();
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
                deposit.Message = Message;
            }
            return deposit;
        }


        public Message GenerateFingerRequest(Deposit deposit, AppSession session, IHttpContextAccessor contextAccessor)
        {
            try
            {
                if (string.IsNullOrEmpty(deposit.customer_no))
                {
                    MessageHelper.Error(Message, "Select a customer to verify Finger.");
                }
                else
                {
                    var customer = session.TransactionSession.Customers.Where(o => o.customer_no == deposit.customer_no).FirstOrDefault();

                    if (customer.customer_status != "R")
                    {
                        MessageHelper.Error(Message, "Customer is not registered in agent banking system.");
                    }
                    else
                    {
                        // lblCustomerNameVal.Text = drCustomer["customer_name"].ToString();
                        session.TransactionSession.TransactionCustomerNo = customer.customer_no;
                        session.TransactionSession.CustomerName = customer.customer_name;
                        deposit.customer_name = customer.customer_name;
                        if (deposit.bearerType == "01")
                        {
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
                }
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "DepositManager-GenerateFingerRequest", ex.Message + "|" + ex.StackTrace.TrimStart());

                MessageHelper.Error(Message, "System Error!!");
            }
            finally
            {
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
            }
            return Message;
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
                        MessageHelper.Error(Message, "Re-Generate Fingure Request Successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "DepositManager-ReGenerateFingerRequest", ex.Message + "|" + ex.StackTrace.TrimStart());
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
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "DepositManager-GetFingerVerifyStatus", ex.Message + "|" + ex.StackTrace.TrimStart());
            }
            finally
            {
                Connection.Close();
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
            }
            return msg;
        }

        public Message InitiateTransaction(Deposit deposit, AppSession session, IHttpContextAccessor contextAccessor)
        {
            if (string.IsNullOrEmpty(deposit.bearerType))
            {
                MessageHelper.Error(Message, "Select bearer type to continue.");
            }
            else
            {
                session.TransactionSession.BearerType = deposit.bearerType;
                session.TransactionSession.BearerTypeDesc = deposit.bearerTypeDesc;

                try
                {
                    if (Connection.State != ConnectionState.Open)
                        Connection.Open();
                    session.TransactionSession.TransactionType = "01";
                    var msg = transactionRepository.InitiateTransaction(session.TransactionSession.TransactionType, session.TransactionSession.TransactionAmount, session.TransactionSession.TransactionAccountNo, 
                        session.TransactionSession.TransactionAccountDesc, session.User.agent_cust_ac_no,
                        "", deposit.bearerType, session.TransactionSession.TransactionCustomerNo, "", session.User.agent_id, session.User.outlet_id, session.User.user_id);

                    if (msg.pvc_status == "40999")
                    {
                        session.TransactionSession.TransactionID = msg.pvc_transid;
                        MessageHelper.Success(Message, "Transaction Initiate Succesfully.");
                        //TODO:Need adjust logic in HTML
                        //if (ddlBearer.SelectedValue == "01")
                        //{
                        //try
                        //{
                        //    conn.OpenConnection();
                        //    DataTable dtCustomer = conn.GetDataTable(lovManager.GetAccountCustomer(txtAccountNo.Text, usrSess.UserID));

                        //    ddlCustomer.DataSource = dtCustomer;
                        //    ddlCustomer.DataTextField = "customer_name";
                        //    ddlCustomer.DataValueField = "customer_no";
                        //    ddlCustomer.DataBind();

                        //    usrSess.AccountCustomer = dtCustomer;
                        //    MessageHelper.Success(Message, "Transaction Confirm. You Can Proceed.");
                        //}
                        //catch (Exception ex)
                        //{
                        //    showMessage("E", "Error:" + ex.Message);
                        //}
                        //finally
                        //{
                        //    conn.CloseConnection();
                        //}
                        //}
                        //else if (ddlBearer.SelectedValue == "02")
                        //{
                        //    CompleteTransaction();
                        //    wzd.MoveTo(this.WizardStep5);
                        //}
                    }
                    else
                    {
                        MessageHelper.Error(Message, msg.pvc_statusmsg);
                    }
                }
                catch (Exception ex)
                {
                    Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "DepositManager-InitiateTransaction", ex.Message + "|" + ex.StackTrace.TrimStart());
                    MessageHelper.Success(Message, "System Error!!.");
                }
                finally
                {
                    Connection.Close();
                    contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
                }

            }
            return Message;
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
                    session.TransactionSession.TransactionDate = DateTime.Now.ToString();
                    msg = transactionRepository.SetTransactionStatus(session.TransactionSession.TransactionDate, session.TransactionSession.TransactionID, "50200", session.User.user_id);

                    if (msg.pvc_status == "40999")
                    {
                        string[] trnErr;
                        string inputXmlString, outputXmlString;
                        var trnResp = cbsDataConnectionManager.ProcessTransaction(session.TransactionSession.TransactionID, "01", "D", transactionXml, out trnErr, out inputXmlString, out outputXmlString);

                        if (trnResp.pvc_status == "40999")
                        {
                            session.TransactionSession.UbsTransactionRefNo = trnResp.pvc_transid;
                            // lblTrnRefNoVal.Text = trnResp[2]
                            // lnkPrint.Visible = true;
                            msg = transactionRepository.SetTransactionStatus("04", session.TransactionSession.TransactionAccountNo,
                                session.TransactionSession.TransactionDate, session.TransactionSession.TransactionID, "01", session.TransactionSession.TransactionAmount, "50999", session.User.agent_id, session.User.user_id);

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
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "DepositManager-CompleteTransaction", ex.Message + "|" + ex.StackTrace.TrimStart());
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

    public interface IDepositManager
    {
        ResponseMessage GetFingerVerifyStatus(AppSession session, IHttpContextAccessor contextAccessor);
        Message ReGenerateFingerRequest(AppSession session, IHttpContextAccessor contextAccessor);
        Deposit ComfirmTransaction(Deposit deposit, AppSession session, IHttpContextAccessor contextAccessor);
        Message InitiateTransaction(Deposit deposit, AppSession session, IHttpContextAccessor contextAccessor);
        Message CompleteTransaction(AppSession session, IHttpContextAccessor contextAccessor);
        Message GenerateFingerRequest(Deposit deposit, AppSession session, IHttpContextAccessor contextAccessor);
    }
}
