using EasyAssetManagerCore.BusinessLogic.Common;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Operation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EasyAssetManagerCore.BusinessLogic.Operation
{
    public class RemmittanceManager:BaseService, IRemmittanceManager
    {
        private readonly IRemmittanceRepository remmittanceRepository;
        public RemmittanceManager() : base((int)ConnectionStringEnum.EbankConnectionString)
        {
            remmittanceRepository = new RemmittanceRepository(Connection);
        }
        public IEnumerable<Remittance> GetRemitCompanyDetails(string remmit_id, AppSession session)
        {
            return remmittanceRepository.GetRemitCompanyDetails(remmit_id, session.User.user_id);
        }
        public IEnumerable<Remittance> GetRemitCompanyDetails(string remmit_id, string remmit_name, AppSession session)
        {
            return remmittanceRepository.GetRemitCompanyDetails(remmit_id, remmit_name, session.User.user_id);
        }
        public IEnumerable<Remittance> GetUnauthoRemitCompanyDetails(AppSession session)
        {
            return remmittanceRepository.GetUnauthoRemittanceCompany(session.User.user_id);
        }
        public Message SetRemitCompanyDetails(Remittance remmittance, AppSession session)
        {
            if (remmittance.remit_comp_name == "")
                MessageHelper.Error(Message, "Remmittance Name is required.");
            else if (remmittance.cust_ac_no == "")
                MessageHelper.Error(Message, "Account is required.");
            else if (remmittance.status == "")
                MessageHelper.Error(Message, "Select status to continue.");
            else
            {
                try
                {
                    if (Connection.State != ConnectionState.Open)
                        Connection.Open();
                    var msg = remmittanceRepository.SetRemitCompanyDetails(remmittance.remit_comp_id, remmittance.remit_comp_name, remmittance.cust_ac_no, remmittance.status, session.User.user_id);

                    if (msg.pvc_status == "40999")
                        MessageHelper.Success(Message, "RemitCompany adding Successfull.");
                    else
                        MessageHelper.Error(Message, "RemitCompany adding Failed. Please try again.");
                }
                catch (Exception ex)
                {
                    Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "RemmittanceManager-SetRemitCompanyDetails", ex.Message + "|" + ex.StackTrace.TrimStart());
                    MessageHelper.Error(Message, "System Error.");
                }
                finally
                {
                    Connection.Close();
                }
            }
            return Message;
        }

        public Message DelRemitCompanyDetails(string remmit_id, AppSession session)
        {
            if (remmit_id == "")
                MessageHelper.Error(Message, "Remmit_id Id is required.");
            else
            {
                try
                {
                    if (Connection.State != ConnectionState.Open)
                        Connection.Open();
                    var msg = remmittanceRepository.DelRemitCompanyDetails(remmit_id, session.User.user_id);

                    if (msg.pvc_status == "40999")
                        MessageHelper.Success(Message, "RemitCompany deleting Successfull.");
                    else
                        MessageHelper.Error(Message, "RemitCompany deleting Failed. Please try again.");
                }
                catch (Exception ex)
                {
                    Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "RemmittanceManager-DelRemitCompanyDetails", ex.Message + "|" + ex.StackTrace.TrimStart());
                    MessageHelper.Error(Message, "System Error.");
                }
                finally
                {
                    Connection.Close();
                }
            }
            return Message;
        }
        public Message SetRemitCompanyAuthorized(List<string> remmittanceList, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                int successCount = 0;
                int errorCount = 0;
                foreach (var item in remmittanceList)
                {
                    var response = remmittanceRepository.SetRemittanceCompanyAuthorized(item, session.User.user_id);
                    if (response.pvc_status == "40999")
                    {
                        successCount++;
                        //MessageHelper.Success(Message, "Success " + listUsers.Count() + " saved Successfully.");
                    }
                    else
                    {
                        errorCount++;
                        //MessageHelper.Error(Message, response.pvc_statusmsg);
                    }
                }
                MessageHelper.Success(Message, "Total unauthorize role: " + remmittanceList.Count() + ", saved successfully: " + successCount + ", faild: " + errorCount);
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "RemmittanceManager-SetRemitCompanyAuthorized", ex.Message + "|" + ex.StackTrace.TrimStart());
                MessageHelper.Error(Message, "System Error.");
            }
            finally
            {
                Connection.Close();
            }

            return Message;
        }
    }
    public interface IRemmittanceManager
    {
        IEnumerable<Remittance> GetRemitCompanyDetails(string remmit_id, AppSession session);
        IEnumerable<Remittance> GetRemitCompanyDetails(string remmit_id, string remmit_name, AppSession session);
        IEnumerable<Remittance> GetUnauthoRemitCompanyDetails(AppSession session);
        Message SetRemitCompanyDetails(Remittance remmittance, AppSession session);
        Message DelRemitCompanyDetails(string remmit_id, AppSession session);
        Message SetRemitCompanyAuthorized(List<string> remmittanceList, AppSession session);
    }
}
