using Dapper;
using Dapper.Oracle;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Common;
using Oracle.ManagedDataAccess.Client;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EasyAssetManagerCore.Repository.Operation
{
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        public CustomerRepository(OracleConnection connection) : base(connection)
        {

        }
        public Customer GetCustomer(string customerNo, string userId)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_customerno", customerNo, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("pvc_appuser", userId, OracleMappingType.Varchar2, ParameterDirection.Input);

            dyParam.Add("pcr_customerdtl", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Customer>("pkg_lov_manager.dpd_get_customerdtl", dyParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
        }

        public IEnumerable<Customer> GetCustomersByAccountNumber(string pvc_custacno, string pvc_appuser)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_custacno", pvc_custacno, OracleMappingType.Varchar2, ParameterDirection.Input,20);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input,50);

            dyParam.Add("pcr_accountcustomer", 0, OracleMappingType.RefCursor, ParameterDirection.Output);
            return Connection.Query<Customer>("pkg_lov_manager.dpd_get_accountcustomer", dyParam, commandType: CommandType.StoredProcedure);
        }

        public ResponseMessage CustomerInitiateRegistration(Customer customer,AppSession appSession)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_regslno", appSession.TransactionSession.TransactionID, OracleMappingType.Varchar2, ParameterDirection.InputOutput,20);
            dyParam.Add("pvc_custno", customer.customer_no, OracleMappingType.Varchar2, ParameterDirection.Input,9);
            dyParam.Add("pvc_custname", customer.customer_name, OracleMappingType.Varchar2, ParameterDirection.Input,100);
            dyParam.Add("pvc_fathername", customer.father_name, OracleMappingType.Varchar2, ParameterDirection.Input,100);
            dyParam.Add("pvc_mothername", customer.mother_name, OracleMappingType.Varchar2, ParameterDirection.Input,100);
            dyParam.Add("pvc_dob", customer.date_of_birth, OracleMappingType.Varchar2, ParameterDirection.Input,100);
            dyParam.Add("pvc_sex", customer.sex, OracleMappingType.Varchar2, ParameterDirection.Input,1);
            dyParam.Add("pvc_nid", customer.nid, OracleMappingType.Varchar2, ParameterDirection.Input,20);
            dyParam.Add("pvc_mobile", customer.mobile_number, OracleMappingType.Varchar2, ParameterDirection.Input,20);
            dyParam.Add("pvc_phototype", "jpg", OracleMappingType.Varchar2, ParameterDirection.Input,10);
            dyParam.Add("pvc_appuser", appSession.User.user_id, OracleMappingType.Varchar2, ParameterDirection.Input,50);
            dyParam.Add("pvc_agentid", appSession.User.agent_id, OracleMappingType.Varchar2, ParameterDirection.Input,20);
            dyParam.Add("pvc_stationip", appSession.User.StationIp, OracleMappingType.Varchar2, ParameterDirection.Input,100);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output,10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output,255);
            var res = responseMessage.QueryExecute(Connection, "pkg_pms_manager.dpd_init_custregistration", dyParam);
            return res;
        }

        public CustomerImage GetCustomerImage(string customerNo,string userId)
        {

            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_customerno", customerNo, OracleMappingType.Varchar2, ParameterDirection.Input);
            return Connection.Query<CustomerImage>(@"SELECT image_text
                                                  FROM sttm_cust_image i
                                                  WHERE i.customer_no = :pvc_customerno", dyParam, commandType: CommandType.Text).FirstOrDefault();
        }

        public ResponseMessage SetRegistrationCompleted(string pvc_transid, string pvc_statuscode,
                                                  string pvc_appuser)
        {
            var responseMessage = new ResponseMessage();
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("pvc_regslno", pvc_transid, OracleMappingType.Varchar2, ParameterDirection.Input,20);
            dyParam.Add("pvc_statuscode", pvc_statuscode, OracleMappingType.Varchar2, ParameterDirection.Input,5);
            dyParam.Add("pvc_appuser", pvc_appuser, OracleMappingType.Varchar2, ParameterDirection.Input,50);

            dyParam.Add("pvc_status", 0, OracleMappingType.Varchar2, ParameterDirection.Output,10);
            dyParam.Add("pvc_statusmsg", 0, OracleMappingType.Varchar2, ParameterDirection.Output,255);
            var res = responseMessage.QueryExecute(Connection, "pkg_pms_manager.dpd_set_registrationcompleted", dyParam);
            return res;
        }
    }

    #region Interface
    public interface ICustomerRepository
    {
       Customer GetCustomer(string customerNo, string userId);
        ResponseMessage CustomerInitiateRegistration(Customer customer, AppSession appSession);
        CustomerImage GetCustomerImage(string customerNo, string userId);
        ResponseMessage SetRegistrationCompleted(string pvc_transid, string pvc_statuscode,
                                                  string pvc_appuser);
        IEnumerable<Customer> GetCustomersByAccountNumber(string pvc_custacno, string pvc_appuser);

    }

    #endregion
}
