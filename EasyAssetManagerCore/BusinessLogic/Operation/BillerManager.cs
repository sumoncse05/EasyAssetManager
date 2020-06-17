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
    public class BillerManager : BaseService, IBillerManager
    {
        private readonly IBillerRepository billerRepository;
        public BillerManager() : base((int)ConnectionStringEnum.EbankConnectionString)
        {
            billerRepository = new BillerRepository(Connection);
        }
        public IEnumerable<Biller> GetBillerDetails(string biller_id, AppSession session)
        {
            return billerRepository.GetBillerDetails(biller_id, session.User.user_id);
        }
        public IEnumerable<Biller> GetBillerDetails(string biller_id,string biller_name, AppSession session)
        {
            return billerRepository.GetBillerDetails(biller_id, biller_name, session.User.user_id);
        }
        public IEnumerable<Biller> GetUnauthoBillerDetails(AppSession session)
        {
            return billerRepository.GetUnauthoBillerDetails(session.User.user_id);
        }
        public Message SetBillerDetails(Biller biller, AppSession session)
        {
            if (biller.biller_desc == "")
                MessageHelper.Error(Message, "Biller Name is required.");
            else if (biller.cust_ac_no == "")
                MessageHelper.Error(Message, "Account is required.");
            else if (biller.status == "")
                MessageHelper.Error(Message, "Select status to continue.");
            else
            {
                try
                {
                    if (Connection.State != ConnectionState.Open)
                        Connection.Open();
                    var msg = billerRepository.SetBillerDetails(biller.biller_id, biller.biller_desc, biller.cust_ac_no, biller.status, session.User.user_id);

                    if (msg.pvc_status == "40999")
                        MessageHelper.Success(Message, "Biller adding Successfull.");
                    else
                        MessageHelper.Error(Message, "Biller adding Failed. Please try again.");
                }
                catch (Exception ex)
                {
                    Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "BillerManager-SetBillerDetails", ex.Message + "|" + ex.StackTrace.TrimStart());
                    MessageHelper.Error(Message, "System Error.");
                }
                finally
                {
                    Connection.Close();
                }
            }
            return Message;
        }

        public Message DelBillerDetails(string biller_id, AppSession session)
        {
            if (biller_id == "")
                MessageHelper.Error(Message, "Biller Id is required.");
            else
            {
                try
                {
                    if (Connection.State != ConnectionState.Open)
                        Connection.Open();
                    var msg = billerRepository.DelBillerDetails(biller_id, session.User.user_id);

                    if (msg.pvc_status == "40999")
                        MessageHelper.Success(Message, "Biller deleting Successfull.");
                    else
                        MessageHelper.Error(Message, "Biller deleting Failed. Please try again.");
                }
                catch (Exception ex)
                {
                    Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "BillerManager-DelBillerDetails", ex.Message + "|" + ex.StackTrace.TrimStart());
                    MessageHelper.Error(Message, "System Error.");
                }
                finally
                {
                    Connection.Close();
                }
            }
            return Message;
        }
        public Message SetBillerAuthorized(List<string> billerList, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                int successCount = 0;
                int errorCount = 0;
                foreach (var item in billerList)
                {
                    var response = billerRepository.SetBillerAuthorized(item, session.User.user_id);
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
                MessageHelper.Success(Message, "Total unauthorize role: " + billerList.Count() + ", saved successfully: " + successCount + ", faild: " + errorCount);
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "BillerManager-SetBillerAuthorized", ex.Message + "|" + ex.StackTrace.TrimStart());
                MessageHelper.Error(Message, "System Error.");
            }
            finally
            {
                Connection.Close();
            }

            return Message;
        }
    }
    public interface IBillerManager
    {
        IEnumerable<Biller> GetBillerDetails(string biller_id, AppSession session);
        IEnumerable<Biller> GetBillerDetails(string biller_id,string biller_name, AppSession session);
        Message SetBillerDetails(Biller biller, AppSession session);
        Message DelBillerDetails(string biller_id, AppSession session);
        IEnumerable<Biller> GetUnauthoBillerDetails(AppSession session);
        Message SetBillerAuthorized(List<string> billerList, AppSession session);
    }
}
