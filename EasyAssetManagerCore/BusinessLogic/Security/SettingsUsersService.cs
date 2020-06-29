using EasyAssetManagerCore.BusinessLogic.Common;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Security;
using Microsoft.AspNetCore.Http;
using SecurityService;
using SmsService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EasyAssetManagerCore.BusinessLogic.Security
{
    public class SettingsUsersService : BaseService, ISettingsUsersService
    {
        private readonly ISettingsUsersRepository userRepository;
        private readonly ICommonManager commonManager;
        public SettingsUsersService() : base((int)ConnectionStringEnum.EsecConnectionString)
        {
            userRepository = new SettingsUsersRepository(Connection);
            commonManager = new CommonManager();
        }

        public Message Delete(SettingsUsers user, AppSession appSession)
        {
            throw new NotImplementedException();
        }

        public SettingsUsers Get(int userId)
        {
            return userRepository.Get(userId);
        }


        public Message Insert(SettingsUsers user, AppSession appSession)
        {

            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                var olduser = userRepository.Get(user.user_id);
                if (olduser == null)
                {
                    user.user_name = appSession.User.user_name;
                    //user.IsLockedOut = false;
                    // user.IsActive = true;
                    user.SelfServiceCategoryId = 0;
                    // user.EntryUserId = appSession.User.user_id;
                    // user.SetDate = DateTime.Now;
                    //user.Password = Encription.Encrypt(user.Password);
                    var insert = userRepository.Insert(user);
                    if (insert > 0) MessageHelper.Success(Message, "Data saved successfully."); else MessageHelper.Error(Message, "Data not saved .");
                }
                else
                {
                    olduser.branch_name = user.branch_name;
                    olduser.agent_id = user.agent_id;
                    olduser.roleid = user.roleid;
                    var update = userRepository.Update(olduser);
                    if (update > 0) MessageHelper.Success(Message, "Data update successfully."); else MessageHelper.Error(Message, "Data not update .");
                }
            }
            catch (Exception ex)
            {

                MessageHelper.Error(Message, "Data not saved.");

            }
            finally
            {
                Connection.Close();
            }
            return Message;
        }

        public SettingsUsers Get(string userId)
        {
            return userRepository.Get(userId);
        }

        public Message UpdatePassword(SettingsUsers user, string newPassword)
        {
            try
            {
                var oldUser = userRepository.Get(user.user_id);
                if (oldUser != null)
                {
                    //if (Encription.Encrypt(user.Password) != oldUser.Password) { MessageHelper.Error(Message, "User Id & Password Not Matched."); return Message; }
                    //oldUser.Password = Encription.Encrypt(newPassword);
                    var isUpdate = userRepository.Update(oldUser);
                    if (isUpdate > 0)
                    {
                        MessageHelper.Success(Message, "User Password Change Successfully.");
                        return Message;
                    }
                    else
                    {
                        MessageHelper.Error(Message, "User Password Not Updated.");
                        return Message;
                    }

                }
                else
                {
                    MessageHelper.Error(Message, "User Not Found.");
                    return Message;
                }
            }
            catch
            {
                MessageHelper.Error(Message, "Database Error!!!");
                return Message;
            }
        }

        public Message ResetPassword(string userId, string reason, AppSession appSession)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                string reset_code = RandomPassword.Generate(20);
                var responseMessage = userRepository.RequestResetPassword(userId, reset_code, "", reason, appSession.User.user_id);
                if (responseMessage.pvc_status == "40999")
                {

                    MessageHelper.Success(Message, responseMessage.pvc_statusmsg);
                    return Message;

                }
                else
                {
                    MessageHelper.Error(Message, responseMessage.pvc_statusmsg);
                    return Message;
                }
            }
            catch
            {
                MessageHelper.Error(Message, "Database Error!!!");
                return Message;
            }
            finally
            {
                Connection.Close();
            }
        }

        public Message LockUnLockUser(string userId)
        {
            try
            {
                var oldUser = userRepository.Get(userId);
                if (oldUser != null)
                {
                    //oldUser.IsLockedOut = !oldUser.IsLockedOut;
                    var isUpdate = userRepository.Update(oldUser);
                    if (isUpdate > 0)
                    {
                        MessageHelper.Success(Message, "User Lock Status Change Successfully");
                        return Message;
                    }
                    else
                    {
                        MessageHelper.Error(Message, "User Lock Status not changed.");
                        return Message;
                    }

                }
                else
                {
                    MessageHelper.Error(Message, "User Not Found.");
                    return Message;
                }
            }
            catch
            {
                MessageHelper.Error(Message, "Database Error!!!");
                return Message;
            }
        }

        public Message ActiveInActiveUser(string userId)
        {
            try
            {
                var oldUser = userRepository.Get(userId);
                if (oldUser != null)
                {
                    // oldUser.IsActive = !oldUser.IsActive;
                    var isUpdate = userRepository.Update(oldUser);
                    if (isUpdate > 0)
                    {
                        MessageHelper.Success(Message, "User Status Change Successfully");
                        return Message;
                    }
                    else
                    {
                        MessageHelper.Error(Message, "User Status not changed.");
                        return Message;
                    }

                }
                else
                {
                    MessageHelper.Error(Message, "User Not Found.");
                    return Message;
                }
            }
            catch
            {
                MessageHelper.Error(Message, "Database Error!!!");
                return Message;
            }
        }

        public Message DoLogin(SettingsUsers pUser, out AppSession appSession)
        {
            appSession = new AppSession();
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var transaction = new TransactionSession();

                SecurityServicesSoapClient client = new SecurityServicesSoapClient(SecurityServicesSoapClient.EndpointConfiguration.SecurityServicesSoap);
                var result = client.AuthenticateUserAsync(pUser.user_id, pUser.Password, "P003", "", "");
                var response = result.Result.Body.AuthenticateUserResult;
                if (response.StatusCode == "40999")
                {
                    var user = userRepository.GetLoginInfo(pUser.user_id, new Encription().Encrypt(pUser.Password), pUser.StationIp, pUser.SessionId);
                    if (user != null)
                    {
                        if (user.Active == "Y")
                        {
                            var screenAccessPermissions = userRepository.GetRoleWisePermission(user.roleid, "", user.user_id);
                            var screens = userRepository.GetScreens(user.user_id);
                            appSession = new AppSession
                            {
                                User = user,
                                ScreenAccessPermissions = screenAccessPermissions,
                                Screens = screens,
                                TransactionSession = transaction

                            };
                            MessageHelper.Success(Message, "Login Successfull.");
                        }
                        else

                        {
                            MessageHelper.Error(Message, "Your are not yet activated. Please contact administrator.");
                        }

                    }
                    else
                    {
                        MessageHelper.Error(Message, "User name and password doesn't match. Please try with another.");
                    }
                }
                else
                {
                    MessageHelper.Error(Message, "User name and password doesn't match. Please try with another.");
                }
            }
            catch (Exception ex)
            {
                MessageHelper.Error(Message, "System Error!!");
            }
            finally
            {
                Connection.Close();
            }
            return Message;
        }

        public Message SetUserPassword(SettingsUsers settingsUser, AppSession session, IHttpContextAccessor contextAccessor)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                if (settingsUser.NewPassword.Equals(settingsUser.ConfPassword) == false)
                {
                    MessageHelper.Info(Message, "Password confirmation missmatch.");
                    return Message;
                }
                var msg = userRepository.SetUserPassword(session.User.user_id, new Encription().Encrypt(settingsUser.Password),
                    new Encription().Encrypt(settingsUser.NewPassword), "", "", session.User.StationIp, contextAccessor.HttpContext.Session.Id, session.User.user_id);
                if (msg.pvc_status == "40900")
                {

                    MessageHelper.Info(Message, msg.pvc_statusmsg + ": Invalid current password.");
                }
                else
                {
                    MessageHelper.Success(Message, "Password Change Successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageHelper.Error(Message, "System Error!!");
            }
            finally
            {
                Connection.Close();
            }
            return Message;
        }

        public UserRole GetRole(string pvc_roleid, string pvc_appuser)
        {
            return userRepository.GetRole(pvc_roleid, pvc_appuser);
        }

        public IEnumerable<UserRole> GetRoleList(string pvc_appuser)
        {
            return userRepository.GetRoleList(pvc_appuser);
        }

        public Message SaveRole(UserRole userRole, AppSession session)
        {
            try
            {
                if (string.IsNullOrEmpty(userRole.rolE_ID))
                {
                    MessageHelper.Error(Message, "Please provide Role Name.");
                }
                else
                {
                    var response = userRepository.SaveRole(userRole.rolE_ID, userRole.ROLE_DESC, session.User.user_id);
                    if (response.pvc_status == "40999")
                    {
                        if (userRole.ScreenAccessPermissions != null && userRole.ScreenAccessPermissions.Any())
                        {
                            var length = userRole.ScreenAccessPermissions.Count();
                            var screen = new List<string>();
                            var update = new List<string>();
                            var delete = new List<string>();
                            var view = new List<string>();
                            // var i = 0;
                            foreach (var access in userRole.ScreenAccessPermissions)
                            {
                                if (access.can_update == "Y" || access.can_delete == "Y" || access.can_view == "Y")
                                {
                                    screen.Add(access.SCR_ID);
                                    update.Add(access.can_update == null ? "N" : access.can_update);
                                    delete.Add(access.can_delete == null ? "N" : access.can_delete);
                                    view.Add(access.can_view == null ? "N" : access.can_view);
                                }

                                // i++;
                            }
                            response = userRepository.SaveScreenAccessPermission(userRole.rolE_ID, screen.ToArray(), update.ToArray(), delete.ToArray(), view.ToArray(), session.User.user_id);
                            if (response.pvc_status == "40999")
                            {
                                MessageHelper.Success(Message, "Role Details and Access permission Setup Successfully.");
                            }
                            else
                            {
                                MessageHelper.Error(Message, response.pvc_statusmsg);
                            }
                        }
                        else
                        {
                            MessageHelper.Success(Message, "Role Details Setup Successfully.");
                        }

                    }
                    else
                    {
                        MessageHelper.Error(Message, response.pvc_statusmsg);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.Error(Message, ex.Message);
            }
            finally
            {
            }
            return Message;
        }

        public Message DeleteRole(string pvc_roleid, AppSession session)
        {
            try
            {
                if (string.IsNullOrEmpty(pvc_roleid))
                {
                    MessageHelper.Error(Message, "Please provide Role Name.");
                }
                else
                {
                    var response = userRepository.DeleteRole(pvc_roleid, session.User.user_id);
                    if (response.pvc_status == "40999")
                    {
                        MessageHelper.Success(Message, "Delete Role Successfully.");

                    }

                    else
                    {
                        MessageHelper.Error(Message, response.pvc_statusmsg);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.Error(Message, "System Error!!");
            }
            finally
            {
            }
            return Message;
        }

        public UserRole RoleScreenAccessPermission(string roleId, string moduleId, string user_id)
        {
            var role = userRepository.GetRole(roleId, user_id);
            if (role != null)
            {
                role.ScreenAccessPermissions = userRepository.GetRoleWisePermission(roleId, moduleId, user_id);
            }
            return role;
        }

        public IEnumerable<ScreenAccessPermission> UserScreenAccessPermission(string user_id)
        {
            return userRepository.GetInterfaceList("", "", user_id);
        }

        public IEnumerable<User> GetUnAuthorizeUserList(AppSession session)
        {
            return userRepository.GetUnAuthorizeUserList(session.User.user_id);
        }
        public Message AuthUsers(List<string> listUsers, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var response = userRepository.AuthUsers(listUsers.ToArray(), session.User.user_id);
                if (response.pvc_status == "40999")
                {
                    MessageHelper.Success(Message, "Total " + listUsers.Count() + "users authrize Successfully.");
                }
                else
                {
                    MessageHelper.Error(Message, response.pvc_statusmsg);
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

        public IEnumerable<UserRole> GetUnAuthorizeUserRoleList(AppSession session)
        {
            return userRepository.UnauthUserRole(session.User.user_id);
        }
        public Message AuthUsersRole(List<string> listUsersRole, AppSession session)
        {
            try
            {
                var successCount = 0;
                var errorCount = 0;
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                foreach (var item in listUsersRole)
                {
                    var response = userRepository.AuthUsersRole(item, session.User.user_id);
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
                MessageHelper.Success(Message, "Total unauthorize role: " + listUsersRole.Count() + ", saved successfully: " + successCount + ", faild: " + errorCount);
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
        public IEnumerable<User> GetUserPassworEmailList(AppSession session)
        {
            return userRepository.PasswordEmailList("", session.User.user_id);
        }
        public Message EmailResetPassword(string rstslno, string user_id, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var userInfo = userRepository.PasswordEmailList(rstslno, session.User.user_id).FirstOrDefault();
                if (userInfo != null && userInfo.USER_ID != null)
                {
                    try
                    {
                        string emailSubject = "Agent Banking User Details";
                        string emailBody = "Password: " + new Encription().Decrypt(userInfo.USER_PASS).ToString();
                        EmailUtility emailUtility = new EmailUtility();
                        if (emailUtility.SendMail("agentbank@ebl-bd.com", userInfo.EMAIL, emailSubject, emailBody))
                        {
                            var response = userRepository.SetPasswordEmailStatus(rstslno, user_id, "S", "", session.User.user_id);
                            if (response.pvc_status == "40999")
                                MessageHelper.Success(Message, "Password Emailed Successfully");
                            else
                                MessageHelper.Error(Message, response.pvc_statusmsg);
                        }
                    }
                    catch (Exception ex)
                    {
                        var response = userRepository.SetPasswordEmailStatus(rstslno, user_id, "E", ex.Message, session.User.user_id);
                        if (response.pvc_status == "40999")
                            MessageHelper.Success(Message, "Password Emailed Successfully");
                        else
                            MessageHelper.Error(Message, response.pvc_statusmsg);
                    }
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
        public IEnumerable<User> GetInactiveUser(AppSession session)
        {
            return userRepository.GetInactiveUser(session.User.user_id);
        }
        public Message UpdateUserStatus(string actv_slno, string user_id, string update_reason, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var response = userRepository.UpdateUserStatus(actv_slno, user_id, "", update_reason, session.User.user_id);

                if (response.pvc_status == "40999")
                    MessageHelper.Success(Message, "Password Emailed Successfully");
                else
                    MessageHelper.Error(Message, response.pvc_statusmsg);
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
        public IEnumerable<SettingsUsers> GetActiveUsers(string user_id1, string user_name, string user_type, string user_id2)
        {
            return userRepository.GetActiveUsers(user_type, user_id1, user_name, user_id2);
        }

        public IEnumerable<SettingsUsers> GetAuthorizePasswordResetRequest(string user_id)
        {
            return userRepository.GetUnauthResetPasswordUsers(user_id);
        }

        public Message SaveAuthorizePasswordResetRequest(IEnumerable<SettingsUsers> users, AppSession session)
        {
            try
            {
                var responseMessage = new ResponseMessage();
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                string password = RandomPassword.Generate(8);
                foreach (var user in users)
                {
                    responseMessage = userRepository.AuthorizeResetPassword(user.rst_slno, user.user_id, user.reset_code, new Encription().Encrypt(password), session.User.user_id);
                }
                MessageHelper.Success(Message, responseMessage.pvc_statusmsg);
                return Message;
                //if (responseMessage.pvc_status == "40999")
                //{
                //    MessageHelper.Success(Message, responseMessage.pvc_statusmsg);
                //    return Message;
                //}
                //else
                //{
                //    MessageHelper.Error(Message, responseMessage.pvc_statusmsg);
                //    return Message;
                //}
            }
            catch
            {
                MessageHelper.Error(Message, "Database Error!!!");
                return Message;
            }
            finally
            {
                Connection.Close();
            }
            return Message;
        }

        public IEnumerable<User> LoogedinUsers(AppSession session)
        {
            return userRepository.LoogedinUsers(session.User.user_id);
        }
        public Message ClearLoginSession(string user_id, string station_ip, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var response = userRepository.ClearLoginSession(user_id, station_ip, session.User.user_id);

                if (response.pvc_status == "40999")
                    MessageHelper.Success(Message, "Session clear successfully.");
                else
                    MessageHelper.Error(Message, "Unable to clear session! " + response.pvc_statusmsg);
            }
            catch (Exception ex)
            {
                MessageHelper.Error(Message, "Unable to clear session! " + ex.Message);
            }
            finally
            {
                Connection.Close();
            }
            return Message;
        }
        public IEnumerable<User> GetUserList(string sc_user_type, string sc_user_idno, string sc_user_name, AppSession session)
        {
            return userRepository.GetUserList(sc_user_type, sc_user_idno, sc_user_name, session.User.user_id);
        }
        public Message SetUserInactive(string user_id, string user_status, string update_reason, AppSession session)
        {

            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var response = userRepository.SetUserInactive(user_id, update_reason, session.User.user_id);

                if (response.pvc_status == "40999")
                    MessageHelper.Success(Message, "User Status Updated Successfully.");
                else
                    MessageHelper.Error(Message, response.pvc_statusmsg);
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
        public Message DeleteUser(string user_id, AppSession session)
        {

            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var response = userRepository.DeleteUser(user_id, session.User.user_id);

                if (response.pvc_status == "40999")
                    MessageHelper.Success(Message, "User Deleted Successfully.");
                else
                    MessageHelper.Error(Message, response.pvc_statusmsg);
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
        public Message EmailUserDetails(string user_id, AppSession session)
        {

            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var userInfo = userRepository.GetUser(user_id, session.User.user_id);

                if (userInfo != null)
                {
                    string emailSubject1 = "EBL Agent Banking User Details";
                    string emailSubject2 = "EBL Agent Banking User Password";
                    string emailBody1 = "<p>Dear Sir, <br /> Please use the following details to get access to Agent Banking Application:</p> " + "" +
                        "               <p>User ID&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:&nbsp;" + userInfo.USER_ID + "</p>";

                    string emailBody2 = "<p>Dear Sir, <br /> Please use the following details to get access to Agent Banking Application:</p> " + "" +
                        "               <p>Password&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:&nbsp;" + new Encription().Decrypt(userInfo.USER_PASS).ToString() + "</p>";

                    EmailUtility emailUtility = new EmailUtility();
                    emailUtility.SendMail("agentbank@ebl-bd.com", userInfo.EMAIL, emailSubject1, emailBody1);

                    emailUtility = new EmailUtility();
                    emailUtility.SendMail("agentbank@ebl-bd.com", userInfo.EMAIL, emailSubject2, emailBody2);
                    MessageHelper.Success(Message, "Email Sending Successful.");

                }
                else
                    MessageHelper.Error(Message, "Email Sending faild.");
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
        public User GetUserById(string user_id, AppSession session)
        {
            return userRepository.GetUser(user_id, session.User.user_id);
        }
        public IEnumerable<Process> GetProcessRunStatus(string user_id, AppSession session)
        {
            return userRepository.GetProcessRunStatus(user_id, session.User.user_id);
        }
        public IEnumerable<User> CmdUserList(string user_id)
        {
            return userRepository.CmdUserList(user_id);
        }
        public IEnumerable<UserRole> GetAssignedRole(string user_id, AppSession session)
        {
            return userRepository.GetAssignedRole(user_id, session.User.user_id);
        }
        public IEnumerable<UserRole> GetFreeRole(string user_id, AppSession session)
        {
            return userRepository.GetFreeRole(user_id, session.User.user_id);
        }
        public Message SetUserRole(string user_id, string role_id, string action, AppSession session)
        {

            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var response = userRepository.SetUserRole(user_id, role_id, action, session.User.user_id);

                if (response.pvc_status == "40999")
                    MessageHelper.Success(Message, "User Role Add Successfully.");
                else
                    MessageHelper.Error(Message, response.pvc_statusmsg);
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
        public IEnumerable<UserType> GetUserTypeList(string pvc_appuser)
        {
            return userRepository.GetUserTypeList(pvc_appuser);
        }
    }
    public interface ISettingsUsersService
    {
        Message UpdatePassword(SettingsUsers user, string newPassword);
        SettingsUsers Get(string userId);
        Message DoLogin(SettingsUsers pUser, out AppSession appSession);
        Message ResetPassword(string userId, string reason, AppSession appSession);
        Message LockUnLockUser(string userId);
        Message ActiveInActiveUser(string userId);
        Message SetUserPassword(SettingsUsers settingsUser, AppSession session, IHttpContextAccessor contextAccessor);
        UserRole GetRole(string pvc_roleid, string pvc_appuser);
        IEnumerable<UserRole> GetRoleList(string pvc_appuser);
        Message SaveRole(UserRole userRole, AppSession session);
        Message DeleteRole(string pvc_roleid, AppSession session);
        UserRole RoleScreenAccessPermission(string roleId, string moduleId, string user_id);
        IEnumerable<ScreenAccessPermission> UserScreenAccessPermission(string user_id);
        IEnumerable<User> GetUnAuthorizeUserList(AppSession session);
        Message AuthUsers(List<string> listUsers, AppSession session);
        IEnumerable<UserRole> GetUnAuthorizeUserRoleList(AppSession session);
        Message AuthUsersRole(List<string> listUsersRole, AppSession session);
        IEnumerable<User> GetUserPassworEmailList(AppSession session);
        Message EmailResetPassword(string rstslno, string user_id, AppSession session);
        IEnumerable<User> GetInactiveUser(AppSession session);
        Message UpdateUserStatus(string actv_slno, string user_id, string update_reason, AppSession session);
        IEnumerable<SettingsUsers> GetActiveUsers(string user_id1, string user_name, string user_type, string user_id2);
        IEnumerable<SettingsUsers> GetAuthorizePasswordResetRequest(string user_id);
        IEnumerable<User> GetUserList(string sc_user_type, string sc_user_idno, string sc_user_name, AppSession session);
        Message SetUserInactive(string user_id, string user_status, string update_reason, AppSession session);
        Message DeleteUser(string user_id, AppSession session);
        Message EmailUserDetails(string user_id, AppSession session);

        User GetUserById(string user_id, AppSession session);
        Message SaveAuthorizePasswordResetRequest(IEnumerable<SettingsUsers> users, AppSession session);
        IEnumerable<User> LoogedinUsers(AppSession session);
        Message ClearLoginSession(string user_id, string station_ip, AppSession session);
        IEnumerable<Process> GetProcessRunStatus(string user_id, AppSession session);
        IEnumerable<User> CmdUserList(string pvc_appuser);
        IEnumerable<UserRole> GetAssignedRole(string user_id, AppSession session);
        IEnumerable<UserRole> GetFreeRole(string user_id, AppSession session);
        Message SetUserRole(string user_id, string role_id, string action, AppSession session);
        IEnumerable<UserType> GetUserTypeList(string pvc_appuser);
    }
}
