using EasyAssetManagerCore.BusinessLogic.Common;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Operation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;

namespace EasyAssetManagerCore.BusinessLogic.Operation
{
    public class AppUserSetupManager : BaseService, IAppUserSetupManager
    {
        private readonly IAccountOpeningRepository accountOpeningRepository;
        private readonly ICommonManager commonManager;
        private readonly ICbsDataConnectionManager cbsDataConnectionManager;
        public AppUserSetupManager() : base((int)ConnectionStringEnum.EbankConnectionString)
        {
            accountOpeningRepository = new AccountOpeningRepository(Connection);
            commonManager = new CommonManager();
            cbsDataConnectionManager = new CbsDataConnectionManager();
        }
        public IEnumerable<Module> GetModiuleList(string pvc_appuser)
        {
            return accountOpeningRepository.GetModiuleList(pvc_appuser);
        }
        public IEnumerable<UserType> GetUserTypeList(string pvc_appuser)
        {
            return accountOpeningRepository.GetUserTypeList(pvc_appuser);
        }
        public IEnumerable<Branch> GetBranchList(string pvc_appuser)
        {
            return accountOpeningRepository.GetBranchList(pvc_appuser);
        }
        public IEnumerable<Department> GetDepartmentList(string pvc_appuser)
        {
            return accountOpeningRepository.GetDepartmentList(pvc_appuser);
        }
        public IEnumerable<Agent> GetAgentList(string pvc_appuser)
        {
            return accountOpeningRepository.GetAgentList("", pvc_appuser);
        }
        public IEnumerable<AgentOutlet> GetAgentOutletList(string pvc_agentid, string pvc_appuser)
        {
            return accountOpeningRepository.GetAgentOutletList(pvc_agentid, pvc_appuser);
        }
        public Message CreateUser(User user, AppSession session, IHttpContextAccessor contextAccessor)
        {
            var temp = user.OTP_REQ == "true" ? "Y" : "N";
            var temp2 = user.BIND_IP == "true" ? "Y" : "N";
            if (string.IsNullOrEmpty(user.USER_ID))
            {
                MessageHelper.Error(Message, "Enter User Id to continue");
            }
            else if (user.hf_user_status == "1")
            {
                MessageHelper.Error(Message, "Invalid User Name.");
            }
            else if (string.IsNullOrEmpty(user.USER_NAME))
            {
                MessageHelper.Error(Message, "Enter User Name to continue");
            }
            else if (string.IsNullOrEmpty(user.EMAIL))
            {
                MessageHelper.Error(Message, "Enter email to continue");
            }
            else if (string.IsNullOrEmpty(user.PHONE))
            {
                MessageHelper.Error(Message, "Enter Phone to continue");
            }
            else if (string.IsNullOrEmpty(user.USER_TYPE))
            {
                MessageHelper.Error(Message, "Enter User Type to continue");
            }
            else
            {
                try
                {
                    if (user.USER_TYPE == "01")
                    {
                        if (string.IsNullOrEmpty(user.EMP_ID))
                        {
                            MessageHelper.Error(Message, "Enter emp ID to continue");
                            return Message;
                        }
                        else if (string.IsNullOrEmpty(user.BRANCH_CODE))
                        {
                            MessageHelper.Error(Message, "Select branch to continue.");
                            return Message;
                        }
                        else if (string.IsNullOrEmpty(user.DEPT_CODE))
                        {
                            MessageHelper.Error(Message, "Select department to continue.");
                            return Message;
                        }
                    }
                    else if (user.USER_TYPE == "02")
                    {
                        if (string.IsNullOrEmpty(user.AGENT_ID))
                        {
                            MessageHelper.Error(Message, "Select Agent to continue.");
                            return Message;
                        }
                        else if (string.IsNullOrEmpty(user.OUTLET_ID))
                        {
                            MessageHelper.Error(Message, "Select Agent outlet to continue.");
                            return Message;
                        }
                        else if (string.IsNullOrEmpty(user.BRANCH_CODE))
                        {
                            MessageHelper.Error(Message, "Select branch to continue.");
                            return Message;
                        }
                    }
                    string password = RandomPassword.Generate(8);
                    if (Connection.State != ConnectionState.Open)
                        Connection.Open();
                    var msg = accountOpeningRepository.SetUserDtl(user.USER_ID, new Encription().Encrypt(password), user.USER_NAME, user.EMAIL, user.PHONE, user.USER_TYPE,
                                                                  user.OTP_REQ == "true" ? "Y" : "N", session.User.StationIp, user.BIND_IP == "true" ? "Y" : "N",
                                                                  user.MOD_ID, user.EMP_ID, user.BRANCH_CODE, user.DEPT_CODE, user.AGENT_ID, user.OUTLET_ID, session.User.user_id);
                    if (msg.pvc_status == "40999")
                    {
                        MessageHelper.Success(Message, "User Creation Successfull. " + msg.pvc_statusmsg);
                        string emailSubject = "EBL Agent Banking User Details";
                        string emailBody = "<p>Dear Sir, <br /> Please use the following details to get access to Agent Banking Application:</p> " + "" +
                            "               <p>User ID&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:&nbsp;" + user.USER_ID + " <br />" +
                                              "Password&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:&nbsp;" + password + "</p>";

                        EmailUtility emailUtility = new EmailUtility();
                        emailUtility.SendMail("agentbank@ebl-bd.com", user.EMAIL, emailSubject, emailBody);
                    }
                    else
                    {
                        MessageHelper.Error(Message, "User Creation Failed. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "AppUserSetupManager-CreateUser", ex.Message + "|" + ex.StackTrace.TrimStart());
                    MessageHelper.Error(Message, "System Error.");
                }
                finally
                {
                    Connection.Close();
                    //contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
                }
            }
            return Message;
        }
        public Message CheckExistingUserName(string user_id, AppSession session, IHttpContextAccessor contextAccessor)
        {
            if (string.IsNullOrEmpty(user_id))
            {
                MessageHelper.Error(Message, "Enter User Id to continue");
            }
            else
            {
                try
                {
                    if (Connection.State != ConnectionState.Open)
                        Connection.Open();
                    var msg = accountOpeningRepository.CheckUserName(user_id);
                    if (msg == 0)
                    {
                        //session.TransactionSession.TransactionID = msg.pvc_acregslno;
                        MessageHelper.Success(Message, "User Name Free");

                    }
                    else
                    {
                        MessageHelper.Error(Message, "User Name Exist");
                    }
                }
                catch (Exception ex)
                {
                    Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "AppUserSetupManager-CreateUser", ex.Message + "|" + ex.StackTrace.TrimStart());
                    MessageHelper.Error(Message, "System Error.");
                }
                finally
                {
                    Connection.Close();
                    //contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
                }
            }
            return Message;
        }
    }
    public interface IAppUserSetupManager
    {
        IEnumerable<Module> GetModiuleList(string pvc_appuser);
        IEnumerable<UserType> GetUserTypeList(string pvc_appuser);
        IEnumerable<Branch> GetBranchList(string pvc_appuser);
        IEnumerable<Department> GetDepartmentList(string pvc_appuser);
        IEnumerable<Agent> GetAgentList(string pvc_appuser);
        IEnumerable<AgentOutlet> GetAgentOutletList(string pvc_agentid, string pvc_appuser);
        Message CreateUser(User user, AppSession session, IHttpContextAccessor contextAccessor);
        Message CheckExistingUserName(string user_id, AppSession session, IHttpContextAccessor contextAccessor);
    }
}
