using EasyAssetManagerCore.BusinessLogic.Common;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Operation;
using System;
using System.Collections.Generic;
using System.Data;

namespace EasyAssetManagerCore.BusinessLogic.Operation
{
    public class AgentManager : BaseService, IAgentManager
    {
        private readonly IAgentRepository agentRepository;
        public AgentManager() : base((int)ConnectionStringEnum.EbankConnectionString)
        {
            agentRepository = new AgentRepository(Connection);
        }

        public IEnumerable<Account> GetAgentAccountList(string pvc_agentid, string pvc_appuser)
        {
            return agentRepository.GetAgentAccountList(pvc_agentid, pvc_appuser);
        }

        public IEnumerable<Account> GetAgentAccounts(string pvc_agentid, string pvc_customerno, string pvc_appuser)
        {
            return agentRepository.GetAgentAccounts(pvc_agentid, pvc_customerno, pvc_appuser);
        }

        public IEnumerable<Agent> GetAgentDetails(string pvc_agentid, string pvc_appuser)
        {
            return agentRepository.GetAgentDetails(pvc_agentid, pvc_appuser);
        }
        public IEnumerable<Agent> GetAgentDetails(string pvc_agentid,string pvc_agentname, string pvc_appuser)
        {
            return agentRepository.GetAgentDetails(pvc_agentid, pvc_agentname, pvc_appuser);
        }

        public IEnumerable<AgentOutlet> GetAgentOutletDetails(string pvc_agentid, string pvc_outletid, string pvc_appuser)
        {
            return agentRepository.GetAgentOutletDetails(pvc_agentid, pvc_outletid, pvc_appuser);
        }
        public IEnumerable<AgentOutlet> GetAgentOutletDetails(string pvc_agentid,string pvc_agentname, string pvc_outletname, string pvc_appuser)
        {
            return agentRepository.GetAgentOutletDetails(pvc_agentid, pvc_agentname, pvc_outletname, pvc_appuser);
        }

        public IEnumerable<Agent> GetUnAuthendicatedAgentDetails(string pvc_agentid, string pvc_appuser)
        {
            return agentRepository.GetUnAuthendicatedAgentDetails(pvc_agentid, pvc_appuser);
        }

        public IEnumerable<AgentOutlet> GetUnauthenticAgentOutlet(string pvc_agentid, string pvc_outletid, string pvc_appuser)
        {
            return agentRepository.GetUnauthenticAgentOutlet(pvc_agentid, pvc_outletid, pvc_appuser);
        }

        public Message SetAgentAccountStatus(string pvc_agentid, string pvc_custacno, string pvc_acstatus, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var msg = agentRepository.SetAgentAccountStatus(pvc_agentid, pvc_custacno, pvc_acstatus, session.User.user_id);
                if (msg.pvc_status == "40999")
                {
                    MessageHelper.Success(Message, "Update Successfully!!!");
                }
                else
                {
                    MessageHelper.Error(Message, msg.pvc_statusmsg);
                }
            }
            catch (Exception ex)
            {

                MessageHelper.Error(Message, ex.Message);
            }
            finally
            {
                Connection.Close();
            }
            return Message;
        }

        public Message SetAgentAuthorized(string pvc_agentid, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var msg = agentRepository.SetAgentAuthorized(pvc_agentid, session.User.user_id);
                if (msg.pvc_status == "40999")
                {
                    MessageHelper.Success(Message, "Authorized Successfully!!!");
                }
                else
                {
                    MessageHelper.Error(Message, msg.pvc_statusmsg);
                }
            }
            catch (Exception ex)
            {

                MessageHelper.Error(Message, ex.Message);
            }
            finally
            {
                Connection.Close();
            }
            return Message;
        }

        public Message SetAgentDetail(Agent agent, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var msg = agentRepository.SetAgentDetail(agent, session.User.user_id);
                if (msg.pvc_status == "40999")
                {
                    MessageHelper.Success(Message, "Agent Details Save Successfully. ; Agent ID: " + msg.pvc_agentid);
                }
                else
                {
                    MessageHelper.Error(Message, msg.pvc_statusmsg);
                }
            }
            catch (Exception ex)
            {

                MessageHelper.Error(Message, ex.Message);
            }
            finally
            {
                Connection.Close();
            }
            return Message;
        }

        public Message SetAgentOutletAuthorized(string pvc_agentid, string pvc_outletid, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var msg = agentRepository.SetAgentOutletAuthorized(pvc_agentid, pvc_outletid, session.User.user_id);
                if (msg.pvc_status == "40999")
                {
                    MessageHelper.Success(Message, "Authorized Successfully!!!");
                }
                else
                {
                    MessageHelper.Error(Message, msg.pvc_statusmsg);
                }
            }
            catch (Exception ex)
            {

                MessageHelper.Error(Message, ex.Message);
            }
            finally
            {
                Connection.Close();
            }
            return Message;
        }

        public Message SetAgentOutletDetails(AgentOutlet agentOutlet, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var msg = agentRepository.SetAgentOutletDetails(agentOutlet, session.User.user_id);
                if (msg.pvc_status == "40999")
                {
                    MessageHelper.Success(Message, "Agent Outlet Save Successfully. ; Outlet ID: " + msg.pvc_outletid);
                }
                else
                {
                    MessageHelper.Error(Message, msg.pvc_statusmsg);
                }
            }
            catch (Exception ex)
            {

                MessageHelper.Error(Message, ex.Message);
            }
            finally
            {
                Connection.Close();
            }
            return Message;
        }
    }
    public interface IAgentManager
    {
        IEnumerable<Account> GetAgentAccountList(string pvc_agentid, string pvc_appuser);
        IEnumerable<Account> GetAgentAccounts(string pvc_agentid, string pvc_customerno, string pvc_appuser);
        IEnumerable<Agent> GetAgentDetails(string pvc_agentid, string pvc_appuser); 
        IEnumerable<Agent> GetAgentDetails(string pvc_agentid, string pvc_agentname, string pvc_appuser);
        IEnumerable<AgentOutlet> GetAgentOutletDetails(string pvc_agentid, string pvc_outletid, string pvc_appuser);
        IEnumerable<AgentOutlet> GetAgentOutletDetails(string pvc_agentid, string pvc_agentname, string pvc_outletname, string pvc_appuser);
        IEnumerable<Agent> GetUnAuthendicatedAgentDetails(string pvc_agentid, string pvc_appuser);
        IEnumerable<AgentOutlet> GetUnauthenticAgentOutlet(string pvc_agentid, string pvc_outletid, string pvc_appuser);
        Message SetAgentAccountStatus(string pvc_agentid, string pvc_custacno, string pvc_acstatus, AppSession session);
        Message SetAgentAuthorized(string pvc_agentid, AppSession session);
        Message SetAgentDetail(Agent agent, AppSession session);
        Message SetAgentOutletAuthorized(string pvc_agentid, string pvc_outletid, AppSession session);
        Message SetAgentOutletDetails(AgentOutlet agentOutlet, AppSession session);
    }
}
