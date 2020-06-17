using Dapper;
using Dapper.Oracle;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EasyAssetManagerCore.Repository.Common
{
    public class TransactionRepository : BaseRepository, ITransactionRepository
    {
        public TransactionRepository(OracleConnection connection) : base(connection)
        {

        }

        public ResponseMessage GetAccountStatus(string pvc_custacno, string pvc_transtype, string pvc_validscope, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_custacno", pvc_custacno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_transtype", pvc_transtype, OracleMappingType.Varchar2, ParameterDirection.Input, 2);
            dyParam.Add("pvc_validscope", pvc_validscope, OracleMappingType.Varchar2, ParameterDirection.Input, 2);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            dyParam.Add("pvc_acdesc", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 100);
            dyParam.Add("pvc_operationmode", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 1);
            var responseMessage = new ResponseMessage();
            var res = responseMessage.QueryExecute(Connection, "pkg_transaction_manager.dpd_get_accountstatus", dyParam);
            return res;
        }
        public ResponseMessage GetTransactionCharges(string pvc_transtype, string pvc_custacno, string pnm_amount,
                                                   string pvc_agentid, string pvc_outletid, string pvc_billerid, string pvc_appuser)
        {

            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_transtype", pvc_transtype, OracleMappingType.Varchar2, ParameterDirection.Input, 2);
            dyParam.Add("pvc_custacno", pvc_custacno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pnm_amount", pnm_amount, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_agentid", pvc_agentid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_outletid", pvc_outletid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_billerid", pvc_billerid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);

            dyParam.Add("pnm_commamt", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pnm_vatamt", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            dyParam.Add("pnm_totalamt", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 100);
            var responseMessage = new ResponseMessage();
            var res = responseMessage.QueryExecute(Connection, "pkg_transaction_manager.dpd_get_transactioncharge", dyParam);
            return res;
        }
        public ResponseMessage InitiateTransaction(string pvc_transtype, //string pvc_transcode,
                                                 string pvc_transamount, //string pvc_addltxt,
                                                 string pvc_custacno, string pvc_custacdesc, string pvc_altacno, string pvc_altacdesc,
                                                 string pvc_bearertype, string pvc_bearerrefno, string pvc_bearername,
                                                 string pvc_agentid, string pvc_outletid, string pvc_appuser)
        {

            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_transtype", pvc_transtype, OracleMappingType.Varchar2, ParameterDirection.Input, 2);
            dyParam.Add("pvc_transamount", pvc_transamount, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_addltxt", "", OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_custacno", pvc_custacno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_custacdesc", pvc_custacdesc, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_altacno", pvc_altacno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_altacdesc", pvc_altacdesc, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_bearertype", pvc_bearertype, OracleMappingType.Varchar2, ParameterDirection.Input, 2);
            dyParam.Add("pvc_bearerrefno", pvc_bearerrefno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_bearername", pvc_bearername, OracleMappingType.Varchar2, ParameterDirection.Input, 100);
            dyParam.Add("pvc_agentid", pvc_agentid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_outletid", pvc_outletid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            dyParam.Add("pvc_transid", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 20);
            dyParam.Add("pvc_charge", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 20);
            dyParam.Add("pvc_chargevat", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 20);
            dyParam.Add("pvc_totalamount", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 20);
            var responseMessage = new ResponseMessage();
            var res = responseMessage.QueryExecute(Connection, "pkg_transaction_manager.dpd_init_transaction", dyParam);
            return res;

        }
        public ResponseMessage InitiateTransaction(string pvc_transtype, //string pvc_transcode,
                                                 string pvc_transamount, string pvc_transamountvat,//string pvc_addltxt, 
                                                 string pvc_billerid, string pvc_billerrefno, string pvc_billdtl, string pvc_billmonth,
                                                 string pvc_customername, string pvc_customersex, string pvc_customertype, string pvc_customeracno, string pvc_customermobile,
                                                 string pvc_altacno, string pvc_agentid, string pvc_outletid, string pvc_appuser)
        {

            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_transtype", pvc_transtype, OracleMappingType.Varchar2, ParameterDirection.Input, 2);
            dyParam.Add("pvc_transamount", pvc_transamount, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_addltxt", "", OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_transamountvat", pvc_transamountvat, OracleMappingType.Varchar2, ParameterDirection.Input, 15);
            dyParam.Add("pvc_billerid", pvc_billerid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_altacno", pvc_altacno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_billerrefno", pvc_billerrefno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_billdtl", pvc_billdtl, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_billmonth", pvc_billmonth, OracleMappingType.Varchar2, ParameterDirection.Input, 7);
            dyParam.Add("pvc_customername", pvc_customername, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_customersex", pvc_customersex, OracleMappingType.Varchar2, ParameterDirection.Input, 2);
            dyParam.Add("pvc_customertype", pvc_customertype, OracleMappingType.Varchar2, ParameterDirection.Input, 1);
            dyParam.Add("pvc_customeracno", pvc_customeracno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_customermobile", pvc_customermobile, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_agentid", pvc_agentid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_outletid", pvc_outletid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            dyParam.Add("pvc_transid", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 20);
            dyParam.Add("pvc_charge", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 20);
            dyParam.Add("pvc_chargevat", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 20);
            dyParam.Add("pvc_totalamount", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 20);
            var responseMessage = new ResponseMessage();
            var res = responseMessage.QueryExecute(Connection, "pkg_transaction_manager.dpd_init_transaction", dyParam);
            return res;
        }
        public ResponseMessage InitiateTransaction(string pvc_transtype, //string pvc_transcode,
                                                 string pvc_transamount, //string pvc_addltxt, 
                                                 string pvc_custacno, string pvc_altacno,
                                                 string pvc_securitycode, string pvc_receivername, string pvc_receiversex, string pvc_receiveraddress, string pvc_receivermobile,
                                                 string pvc_receivertype, string pvc_receiveracno, string pvc_sendername, string pvc_sendercountry, string pvc_remitcompany,
                                                 string pvc_remarks, string pvc_agentid, string pvc_outletid, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_transtype", pvc_transtype, OracleMappingType.Varchar2, ParameterDirection.Input, 2);
            dyParam.Add("pvc_transamount", pvc_transamount, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_addltxt", "", OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_custacno", pvc_custacno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_securitycode", pvc_securitycode, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_altacno", pvc_altacno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_receivername", pvc_receivername, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_receiversex", pvc_receiversex, OracleMappingType.Varchar2, ParameterDirection.Input, 2);
            dyParam.Add("pvc_receiveraddress", pvc_receiveraddress, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_receivermobile", pvc_receivermobile, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_receivertype", pvc_receivertype, OracleMappingType.Varchar2, ParameterDirection.Input, 1);
            dyParam.Add("pvc_receiveracno", pvc_receiveracno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_sendername", pvc_sendername, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_sendercountry", pvc_sendercountry, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_remitcompany", pvc_remitcompany, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_remarks", pvc_remarks, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_agentid", pvc_agentid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_outletid", pvc_outletid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            dyParam.Add("pvc_transid", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 20);

            var responseMessage = new ResponseMessage();
            var res = responseMessage.QueryExecute(Connection, "pkg_transaction_manager.dpd_init_transaction", dyParam);
            return res;
        }
        public TransactionXml GetTransactionXml(string pvc_transid, string pvc_agentid, string pvc_outletid, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_transid", pvc_transid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_agentid", pvc_agentid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_outletid", pvc_outletid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pcr_transactiondata", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<TransactionXml>("pkg_transaction_manager.dpd_get_transactionxmldata", dyParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
        }
        public ResponseMessage SetTransactionStatus(string pvc_businessdate, string pvc_transrefno, string pvc_transstatus, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_usertype", "", OracleMappingType.Varchar2, ParameterDirection.Input, 2);
            dyParam.Add("pvc_userrefno", "", OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_businessdate", pvc_businessdate, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_transrefno", pvc_transrefno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_transtype", "", OracleMappingType.Varchar2, ParameterDirection.Input, 2);
            dyParam.Add("pnm_transamount", "", OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_transstatus", pvc_transstatus, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_agentrefno", "", OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var responseMessage = new ResponseMessage();
            var res = responseMessage.QueryExecute(Connection, "pkg_transaction_manager.dpd_set_transactionstatus", dyParam);
            return res;
        }
        public ResponseMessage SetTransactionStatus(string pvc_usertype, string pvc_userrefno, string pvc_businessdate,
                                                  string pvc_transrefno, string pvc_transtype, string pnm_transamount,
                                                  string pvc_transstatus, string pvc_agentrefno,
                                                  string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_usertype", pvc_usertype, OracleMappingType.Varchar2, ParameterDirection.Input, 2);
            dyParam.Add("pvc_userrefno", pvc_userrefno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_businessdate", pvc_businessdate, OracleMappingType.Varchar2, ParameterDirection.Input, 10);
            dyParam.Add("pvc_transrefno", pvc_transrefno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_transtype", pvc_transtype, OracleMappingType.Varchar2, ParameterDirection.Input, 2);
            dyParam.Add("pnm_transamount", pnm_transamount, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_transstatus", pvc_transstatus, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_agentrefno", pvc_agentrefno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var responseMessage = new ResponseMessage();
            var res = responseMessage.QueryExecute(Connection, "pkg_transaction_manager.dpd_set_transactionstatus", dyParam);
            return res;
        }
        public ResponseMessage SetTransactionIo(string pvc_transid, string pvc_reqxml, string pvc_resxml,
                                              string pvc_ecode, string pvc_edesc, string pvc_ubsrefno,
                                              string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_transid", pvc_transid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_reqxml", pvc_reqxml, OracleMappingType.Varchar2, ParameterDirection.Input, pvc_reqxml.Length);
            dyParam.Add("pvc_resxml", pvc_resxml, OracleMappingType.Varchar2, ParameterDirection.Input, pvc_resxml.Length);
            dyParam.Add("pvc_ecode", pvc_ecode, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_edesc", pvc_edesc, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_ubsrefno", pvc_ubsrefno, OracleMappingType.Varchar2, ParameterDirection.Input, 200);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var responseMessage = new ResponseMessage();
            var res = responseMessage.QueryExecute(Connection, "pkg_transaction_manager.dpd_set_transactionio", dyParam);
            return res;
        }
        public IEnumerable<Remittance> GetUnathRemittance(string pvc_transid, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_transid", pvc_transid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 20);

            dyParam.Add("pcr_unauthremittance", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Remittance>("pkg_transaction_manager.dpd_get_unauthremittance", dyParam, commandType: CommandType.StoredProcedure);

        }
        public ResponseMessage SetRemittanceAmount(string pvc_transid, string pvc_amount, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_transid", pvc_transid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_amount", pvc_amount, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var responseMessage = new ResponseMessage();
            var res = responseMessage.QueryExecute(Connection, "pkg_transaction_manager.dpd_set_remittanceamount", dyParam);
            return res;
        }

        public ResponseMessage InitiateBalanceEnquery(string pvc_custacno, string pvc_agentid, string pvc_outletid, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_custacno", pvc_custacno, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_agentid", pvc_agentid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_outletid", pvc_outletid, OracleMappingType.Varchar2, ParameterDirection.Input, 20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            dyParam.Add("pvc_transid", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 20);
            var responseMessage = new ResponseMessage();
            var res = responseMessage.QueryExecute(Connection, "pkg_transaction_manager.dpd_init_balanceenquery", dyParam);
            return res;

        }
        public ResponseMessage GetAccountBalance(string pvc_userrefno, string pvc_agentrefno, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_userrefno", pvc_userrefno, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_agentrefno", pvc_agentrefno, OracleMappingType.Varchar2, ParameterDirection.Input, 50);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input, 50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.ReturnValue, 10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            dyParam.Add("pvc_balance", 0, OracleMappingType.Varchar2, ParameterDirection.Output, 255);
            var responseMessage = new ResponseMessage();
            var res = responseMessage.QueryExecute(Connection, "pkg_transaction_manager.dfn_get_balance_ubs", dyParam);
            return res;
        }
    }

    #region Interface
    public interface ITransactionRepository
    {
        ResponseMessage GetAccountStatus(string pvc_custacno, string pvc_transtype, string pvc_validscope, string pvc_appuser);
        ResponseMessage GetTransactionCharges(string pvc_transtype, string pvc_custacno, string pnm_amount,
                                                   string pvc_agentid, string pvc_outletid, string pvc_billerid, string pvc_appuser);
        ResponseMessage InitiateTransaction(string pvc_transtype,
                                                 string pvc_transamount,
                                                 string pvc_custacno, string pvc_custacdesc, string pvc_altacno, string pvc_altacdesc,
                                                 string pvc_bearertype, string pvc_bearerrefno, string pvc_bearername,
                                                 string pvc_agentid, string pvc_outletid, string pvc_appuser);
        ResponseMessage InitiateTransaction(string pvc_transtype,
                                                 string pvc_transamount, string pvc_transamountvat,
                                                 string pvc_billerid, string pvc_billerrefno, string pvc_billdtl, string pvc_billmonth,
                                                 string pvc_customername, string pvc_customersex, string pvc_customertype, string pvc_customeracno, string pvc_customermobile,
                                                 string pvc_altacno, string pvc_agentid, string pvc_outletid, string pvc_appuser);
        ResponseMessage InitiateTransaction(string pvc_transtype,
                                                 string pvc_transamount,
                                                 string pvc_custacno, string pvc_altacno,
                                                 string pvc_securitycode, string pvc_receivername, string pvc_receiversex, string pvc_receiveraddress, string pvc_receivermobile,
                                                 string pvc_receivertype, string pvc_receiveracno, string pvc_sendername, string pvc_sendercountry, string pvc_remitcompany,
                                                 string pvc_remarks, string pvc_agentid, string pvc_outletid, string pvc_appuser);
        TransactionXml GetTransactionXml(string pvc_transid, string pvc_agentid, string pvc_outletid, string pvc_appuser);
        ResponseMessage SetTransactionStatus(string pvc_businessdate, string pvc_transrefno, string pvc_transstatus, string pvc_appuser);
        ResponseMessage SetTransactionStatus(string pvc_usertype, string pvc_userrefno, string pvc_businessdate,
                                                  string pvc_transrefno, string pvc_transtype, string pnm_transamount,
                                                  string pvc_transstatus, string pvc_agentrefno,
                                                  string pvc_appuser);
        ResponseMessage SetTransactionIo(string pvc_transid, string pvc_reqxml, string pvc_resxml,
                                              string pvc_ecode, string pvc_edesc, string pvc_ubsrefno,
                                              string pvc_appuser);
        ResponseMessage SetRemittanceAmount(string pvc_transid, string pvc_amount, string pvc_appuser);
        ResponseMessage InitiateBalanceEnquery(string pvc_custacno, string pvc_agentid, string pvc_outletid, string pvc_appuser);
        ResponseMessage GetAccountBalance(string pvc_userrefno, string pvc_agentrefno, string pvc_appuser);
        IEnumerable<Remittance> GetUnathRemittance(string pvc_transid, string pvc_appuser);
    }

    #endregion
}
