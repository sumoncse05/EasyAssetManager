using CardService;
using Dapper;
using Dapper.Oracle;
using EasyAssetManagerCore.BusinessLogic.Common;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Operation;
using FcubService;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using UbsService;
using static CardService.EBL_UBPSoapClient;

namespace EasyAssetManagerCore.BusinessLogic.Operation
{
    public class CbsDataConnectionManager : BaseService, ICbsDataConnectionManager
    {
        private readonly ICustomerRepository customerRepository;
        public CbsDataConnectionManager() : base((int)ConnectionStringEnum.CbsConnectionString)
        {
            customerRepository = new CustomerRepository(Connection);
        }

        public CustomerImage GetCustomerImage(string customerNo, string userId)
        {
            try
            {
                var customerImage = new CustomerImage();
                UbsServiceManagerSoapClient client = new UbsServiceManagerSoapClient(UbsServiceManagerSoapClient.EndpointConfiguration.UbsServiceManagerSoap);
                var result = client.GetCustomerImageAsync(customerNo, userId);
                var imageString = result.Result.Any1.Value;
                customerImage.image_text = imageString;
                return customerImage;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                Connection.Close();
            }

        }

        public ResponseMessage ProcessTransaction(string trnRefNo, string transType, string DrCrInd, TransactionXml transaction, out string[] trnErr, out string inputXmlString, out string outputXmlString)
        {
            var trnResp = new ResponseMessage { pvc_status = "40900", pvc_statusmsg = "Unknow Error Occured. Contact administrator.", pvc_transid = "" };
            trnErr = new string[] { "", "" };
            inputXmlString = outputXmlString = "";

            try
            {
                string TrnString = string.Join(", ", transaction.total_amount);
                Logging.WriteToCbsLog("------------------------------------------------------------------------------------------------------------------------");
                Logging.WriteToCbsLog(trnRefNo + "|" + DrCrInd + "|" + TrnString);

                MemoryStream msXml = CreateTransactionXmlNew(trnRefNo, transaction);
                if (msXml.Length > 0)
                {
                    Byte[] inputBuffer = new Byte[msXml.Length];
                    inputBuffer = msXml.ToArray();
                    FCUBSDEServiceSEIClient deService = new FCUBSDEServiceSEIClient(FCUBSDEServiceSEIClient.EndpointConfiguration.FCUBSDEServiceSEI);
                    inputXmlString = System.Text.Encoding.UTF8.GetString(inputBuffer).Substring(1);

                    Logging.WriteToCbsLog("Webservice Execution Started|" + inputXmlString);
                    outputXmlString = deService.MultiNewAsync(@inputXmlString).Result.MultiNewResponse1;
                    Logging.WriteToCbsLog("Webservice Execution Ended|" + Regex.Replace(outputXmlString, @"\r\n?|\n|\t|\s+", String.Empty));

                    Byte[] outputBuffer = Encoding.ASCII.GetBytes(@outputXmlString);
                    XmlDocument outputXml = new XmlDocument();
                    outputXml.LoadXml(outputXmlString);

                    string msg_stat = (outputXml.GetElementsByTagName("MSGSTAT")).Item(0).InnerText;
                    var nsmgr = new XmlNamespaceManager(outputXml.NameTable);
                    nsmgr.AddNamespace("a", "http://fcubs.iflex.com/service/FCUBSDEService/MultiNew");

                    if (msg_stat == "SUCCESS")
                    {
                        XmlNode xmlNode = outputXml.SelectSingleNode("//a:FCUBS_RES_ENV/a:FCUBS_BODY/a:Multi-Offset-Master/a:FCCREF", nsmgr);
                        trnResp = new ResponseMessage { pvc_status = "40999", pvc_statusmsg = "Transaction Completed Successfully", pvc_transid = xmlNode.InnerText };

                        Logging.WriteToCbsLog("Transaction Completed Successfully. Ubs reference no:" + trnResp.pvc_transid);
                    }
                    else
                    {
                        if (msg_stat != null)
                        {
                            XmlNodeList xnlNodeList = outputXml.SelectNodes("//a:FCUBS_RES_ENV/a:FCUBS_BODY/a:FCUBS_ERROR_RESP/a:ERROR", nsmgr);

                            if (xnlNodeList.Count > 0)
                            {
                                string[,] trnErrs = new string[2, xnlNodeList.Count];
                                string trnAllErr = "";
                                int i = 0;
                                foreach (XmlNode xmlNode in xnlNodeList)
                                {
                                    //trnErr[i] = xmlNode["ECODE"].InnerText + ": " + xmlNode["EDESC"].InnerText;
                                    trnErrs[0, i] = xmlNode["ECODE"].InnerText;
                                    trnErrs[1, i] = xmlNode["EDESC"].InnerText;
                                    i++;

                                    trnAllErr += xmlNode["ECODE"].InnerText + ": " + xmlNode["EDESC"].InnerText + ",";
                                }

                                trnErr = new string[] { trnErrs[0, i - 1], trnErrs[1, i - 1] };
                                Logging.WriteToCbsLog(trnAllErr);
                            }
                            else
                            {
                                trnErr = new string[] { "000", "Unknow Error Occured" };

                                Logging.WriteToCbsLog("Unknow Error Occured. Check the transaction log for details.");
                            }
                        }
                        else
                        {
                            trnErr = new string[] { "000", "Unknow Error Occured" };

                            Logging.WriteToCbsLog("Unknow Error Occured. Check the transaction log for details.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                trnErr = new string[] { "000", "Unknow Error Occured in UBS Services. Please try again." };
                Logging.WriteToCbsLog("Unknow Error Occured. Check the error log for details.");
                Logging.WriteToCbsErrLog(trnRefNo + "|" + ex.Message + "|" + ex.StackTrace.TrimStart());
            }

            return trnResp;
        }

        public MemoryStream CreateTransactionXmlNew(string trnRefNo, TransactionXml transaction)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            MemoryStream ms = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(ms, settings);

            writer.WriteStartDocument();
            {
                writer.WriteStartElement("FCUBS_REQ_ENV", "xmlns=http://fcubs.iflex.com/service/FCUBSDEService/MultiNew");
                {
                    writer.WriteStartElement("FCUBS_HEADER");
                    {
                        #region FCUBS_HEADER
                        writer.WriteElementString("SOURCE", TransactionXmlConstant.SOURCE);
                        writer.WriteElementString("UBSCOMP", TransactionXmlConstant.UBSCOMP);
                        writer.WriteElementString("MSGID", trnRefNo);
                        writer.WriteElementString("USERID", TransactionXmlConstant.USERID);
                        writer.WriteElementString("BRANCH", transaction.from_branch_code);
                        writer.WriteElementString("SERVICE", TransactionXmlConstant.SERVICE);
                        writer.WriteElementString("OPERATION", TransactionXmlConstant.OPERATION);
                        #endregion
                    }
                    writer.WriteEndElement();

                    writer.WriteStartElement("FCUBS_BODY");
                    {
                        writer.WriteStartElement("Multi-Offset-Master-IO");
                        {
                            ///////////////// Start of transaction details ////////////////////////////////////////////////////////////
                            #region Transaction-Details
                            #region Multi-Offset-Master-IO
                            //Input the From account details
                            writer.WriteElementString("BATCHNO", TransactionXmlConstant.BATCHNO);
                            writer.WriteElementString("ACCOUNT", transaction.from_account);
                            writer.WriteElementString("CCY", TransactionXmlConstant.CCY);
                            writer.WriteElementString("TXNMAIN", transaction.trans_code);
                            writer.WriteElementString("TXNOFFSET", transaction.trans_code);
                            writer.WriteElementString("VALUEDT", transaction.VALUEDT);
                            writer.WriteElementString("DRCR", "D");
                            writer.WriteElementString("BRN", transaction.from_branch_code);
                            writer.WriteElementString("AMT", transaction.total_amount);
                            writer.WriteElementString("AUTHSTAT", TransactionXmlConstant.AUTHSTAT);
                            writer.WriteElementString("ADDL_TXT", transaction.addl_txt);
                            #endregion Multi-Offset-Master-IO

                            //Input the to accounts details (agent/customer, commission, charge_vat)                            
                            #region Mlt-Offset-Detail
                            //Palli biddyut
                            int slno = 1;
                            if (transaction.biller_id == "BC0001")
                            {
                                writer.WriteStartElement("Mlt-Offset-Detail");
                                {
                                    writer.WriteElementString("ACCOUNT", transaction.biller_rev_ac);
                                    writer.WriteElementString("LCYAMT", transaction.AMOUNT);
                                    writer.WriteElementString("AMT", transaction.AMOUNT);
                                    writer.WriteElementString("BRN", transaction.to_branch_code);
                                    writer.WriteElementString("SLNO", slno.ToString());
                                    slno++;
                                }
                                writer.WriteEndElement();
                                writer.WriteStartElement("Mlt-Offset-Detail");
                                {
                                    writer.WriteElementString("ACCOUNT", transaction.biller_vat_ac);
                                    writer.WriteElementString("LCYAMT", transaction.amount_vat);
                                    writer.WriteElementString("AMT", transaction.amount_vat);
                                    writer.WriteElementString("BRN", transaction.to_branch_code);
                                    writer.WriteElementString("SLNO", slno.ToString());
                                    slno++;
                                }
                                writer.WriteEndElement();
                            }
                            //Others transaction
                            else
                            {
                                writer.WriteStartElement("Mlt-Offset-Detail");
                                {
                                    writer.WriteElementString("ACCOUNT", transaction.to_account);
                                    writer.WriteElementString("LCYAMT", transaction.AMOUNT);
                                    writer.WriteElementString("AMT", transaction.AMOUNT);
                                    writer.WriteElementString("BRN", transaction.to_branch_code);
                                    writer.WriteElementString("SLNO", slno.ToString());
                                    slno++;
                                }
                                writer.WriteEndElement();
                            }

                            if (!string.IsNullOrEmpty(transaction.charge) && transaction.charge != "0")
                            {
                                writer.WriteStartElement("Mlt-Offset-Detail");
                                {
                                    writer.WriteElementString("ACCOUNT", transaction.charge_gl);
                                    writer.WriteElementString("LCYAMT", transaction.charge);
                                    writer.WriteElementString("AMT", transaction.charge);
                                    writer.WriteElementString("BRN", transaction.agent_branch_code);
                                    writer.WriteElementString("SLNO", slno.ToString());
                                    slno++;
                                }
                                writer.WriteEndElement();
                            }
                            if (!string.IsNullOrEmpty(transaction.charge_vat) && transaction.charge_vat != "0")
                            {
                                writer.WriteStartElement("Mlt-Offset-Detail");
                                {
                                    writer.WriteElementString("ACCOUNT", transaction.charge_vat_gl);
                                    writer.WriteElementString("LCYAMT", transaction.charge_vat);
                                    writer.WriteElementString("AMT", transaction.charge_vat);
                                    writer.WriteElementString("BRN", transaction.agent_branch_code);
                                    writer.WriteElementString("SLNO", slno.ToString());
                                    slno++;
                                }
                                writer.WriteEndElement();
                            }
                            #endregion Mlt-Offset-Detail
                            #endregion Transaction-Details
                            ///////////////// End of transaction details ////////////////////////////////////////////////////////////

                            writer.WriteStartElement("Batch-Master");
                            {
                                #region Batch-Master
                                //Input the Customer/Biller Summary Data Here
                                writer.WriteElementString("BRANCH_CODE", transaction.from_branch_code);
                                writer.WriteElementString("BATCH_NO", TransactionXmlConstant.BATCH_NO);
                                writer.WriteElementString("DESCRIPTION", TransactionXmlConstant.DESCRIPTION);
                                writer.WriteElementString("DR_CHK_TOTAL", transaction.total_amount);
                                writer.WriteElementString("CR_CHK_TOTAL", transaction.total_amount);
                                writer.WriteElementString("BALANCING", TransactionXmlConstant.BALANCING);
                                #endregion
                            }
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            writer.WriteEndDocument();

            // Flush the write
            writer.Flush();
            writer.Close();

            return ms;
        }
        public string GetTxnCode(string trans_type)
        {
            string txn_code = "";

            switch (trans_type)
            {
                case "01"://Deposit
                    txn_code = "CAD";
                    break;
                case "02"://Withdraw
                    txn_code = "CAW";
                    break;
                case "03"://Bill Payment Cash
                    txn_code = "ABB";
                    break;
                case "04"://Bill Payment Account
                    txn_code = "ABB";
                    break;
                case "05"://Fund Transfer Within EBL
                    txn_code = "ABI";
                    break;
                case "06"://Fund Transfer Other Bank
                    txn_code = "ABD";
                    break;
                case "07"://Remittance
                    txn_code = "CWR";
                    break;
                default:
                    txn_code = "";
                    break;
            }
            return txn_code;
        }

        public WorkflowDetail GetServiceReferenceDtl(string wfslno, string wf_ref_no, string wf_type, string user_id)
        {
            var workflow = new WorkflowDetail();
            try
            { 
                UbsServiceManagerSoapClient client = new UbsServiceManagerSoapClient(UbsServiceManagerSoapClient.EndpointConfiguration.UbsServiceManagerSoap);
               
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                if (string.IsNullOrEmpty(user_id))
                {
                    MessageHelper.Error(Message, "No Direct access. Please login.");
                }
                else
                {
                    if (wf_type == "01")
                    {
                        var deposit = client.GetCustomerDepositDetailsAsync("RD", wf_ref_no, user_id).Result.Any1;
                       
                        if (deposit != null)
                        {
                            XmlDocument outputXml = new XmlDocument();
                            outputXml.LoadXml(deposit.ToString());
                            XmlNodeList xnlNodeList = outputXml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes;
                            //workflow.cust_no = xnlNodeList[0].InnerText;
                            //workflow.cu = xnlNodeList[1].InnerText;
                            workflow.customer_name = xnlNodeList[2].InnerText;
                            workflow.acy_avl_bal = xnlNodeList[3].InnerText;
                            workflow.mobile_number = xnlNodeList[4].InnerText;

                        }
                        else
                        {
                            MessageHelper.Error(Message, "Deposit details not found.");
                        }
                    }
                    else if (wf_type == "02")
                    {
                        var deposit = client.GetCustomerDepositDetailsAsync("TD", wf_ref_no, user_id).Result.Any1;
                        if (deposit != null)
                        {
                            XmlDocument outputXml = new XmlDocument();
                            outputXml.LoadXml(deposit.ToString());
                            XmlNodeList xnlNodeList = outputXml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes;
                            //workflow.cust_no = xnlNodeList[0].InnerText;
                            //workflow.cu = xnlNodeList[1].InnerText;
                            workflow.customer_name = xnlNodeList[2].InnerText;
                            workflow.acy_avl_bal = xnlNodeList[3].InnerText;
                            workflow.mobile_number = xnlNodeList[4].InnerText;
                        }
                        else
                        {
                            MessageHelper.Error(Message, "Deposit details not found.");
                        }
                    }

                    else if (wf_type == "07" || wf_type == "09")
                    {
                        if (wf_ref_no.Length == 14 || wf_ref_no.Length == 16)
                        {
                            EBL_UBPSoapClient CardServiceManager = new EBL_UBPSoapClient(EndpointConfiguration.EBL_UBPSoap);
                            CARDSTATUSDETAIL_PASS usrPass = new CARDSTATUSDETAIL_PASS();
                            usrPass.USERNAME = "eblconnect";
                            usrPass.PASSWORD = "eblconnect";

                            CARDSTATUSDETAIL cardDtl = new CARDSTATUSDETAIL();
                            cardDtl.REFERENCE_NUMBER = wfslno.Substring(3);
                            cardDtl.CARD_NUMBER = wf_ref_no;
                            cardDtl.CARDSTATUSDETAIL_PASS = usrPass;

                            CARDSTATUSDETAILRESP CardDetl = CardServiceManager.CardstatDetailAsync(cardDtl).Result.Body.CardstatDetailResult;

                            if (CardDetl.ERROR_CODE == "000")
                            {
                                workflow.customer_name = CardDetl.NAME;
                            }
                            else
                            {
                                MessageHelper.Error(Message, CardDetl.ERROR_RESPONSE);
                            }
                        }
                    }
                    else
                    {
                        var customer = client.GetCustomerDetailsAsync("", wf_ref_no, user_id).Result.Any1;
                        
                        if (customer != null)
                        {
                            XmlDocument outputXml = new XmlDocument();
                            outputXml.LoadXml(customer.ToString());
                            XmlNodeList xnlNodeList = outputXml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes;
                            workflow.customer_name = xnlNodeList[1].InnerText;
                            workflow.mobile_number = xnlNodeList[7].InnerText;
                        }
                        else
                        {
                            MessageHelper.Error(Message, "Account details not found");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageHelper.Error(Message, "Account details not found.");
            }
            finally
            {
                Connection.Close();
                workflow.Message = Message;
            }
            return workflow;
        }


        public WorkflowDetail GetCustometDtlFromCbs(string pvc_custacno, string pvc_appuser)
        {
            var workflow = new WorkflowDetail();
            try
            {
                UbsServiceManagerSoapClient client = new UbsServiceManagerSoapClient(UbsServiceManagerSoapClient.EndpointConfiguration.UbsServiceManagerSoap);
                var customer = client.GetCustomerDetailsAsync("", pvc_custacno, pvc_appuser).Result.Any1;

                if (customer != null)
                {
                    XmlDocument outputXml = new XmlDocument();
                    outputXml.LoadXml(customer.ToString());
                    XmlNodeList xnlNodeList = outputXml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes;
                    workflow.customer_no = xnlNodeList[0].InnerText;
                    workflow.customer_name = xnlNodeList[1].InnerText;
                    workflow.father_name = xnlNodeList[2].InnerText;
                    workflow.mother_name = xnlNodeList[3].InnerText;
                    workflow.birth_date = xnlNodeList[4].InnerText;
                    workflow.sex = xnlNodeList[5].InnerText;
                    workflow.nid = xnlNodeList[6].InnerText;
                    workflow.mobile = xnlNodeList[7].InnerText;
                    workflow.photo_type= xnlNodeList[8].InnerText;
                    workflow.acy_avl_bal = "0000";//xnlNodeList[9].InnerText; TODO: ADD after service modify
                }
            }catch(Exception ex)
            {

            }
            return workflow;
           
        }

    }

    public interface ICbsDataConnectionManager
    {
        CustomerImage GetCustomerImage(string customerNo, string userId);
        WorkflowDetail GetCustometDtlFromCbs(string pvc_custacno, string pvc_appuser);
        WorkflowDetail GetServiceReferenceDtl(string wfslno, string wf_ref_no, string wf_type, string user_id);
        ResponseMessage ProcessTransaction(string trnRefNo, string transType, string DrCrInd, TransactionXml transaction, out string[] trnErr, out string inputXmlString, out string outputXmlString);
    }

}
