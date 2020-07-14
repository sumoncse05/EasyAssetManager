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
        public SettingsUsersService()
        {
            userRepository = new SettingsUsersRepository(Connection);
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
                Logging.WriteToErrLog(appSession.User.StationIp, appSession.User.user_id, "ISettingsUsersService-DoLogin", ex.Message + "|" + ex.StackTrace.TrimStart());
                MessageHelper.Error(Message, "System Error!!");
            }
            finally
            {
                Connection.Close();
            }
            return Message;
        }

    }
    public interface ISettingsUsersService
    {
        Message DoLogin(SettingsUsers pUser, out AppSession appSession);
    }
}
