using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace EasyAssetManager.Controllers
{
    public class LoginController : Controller
    {
        private readonly ISettingsUsersService userService;
        private IHostingEnvironment environment;
        private readonly ICommonManager commonManager;
        public LoginController(ISettingsUsersService userService, IHostingEnvironment environment, ICommonManager commonManager)
        {
            this.userService = userService;
            this.environment = environment;
            this.commonManager = commonManager;
        }
        public IActionResult Index()
        {
            //UbsServiceManagerSoapClient client = new UbsServiceManagerSoapClient(UbsServiceManagerSoapClient.EndpointConfiguration.UbsServiceManagerSoap);
            //var deposit = client.GetCustomerDepositDetailsAsync("RD", "2011910152053", "ADMIN").Result.Any1;
            //XmlDocument outputXml = new XmlDocument();
            //outputXml.LoadXml(deposit.ToString());
            //XmlNodeList xnlNodeList = outputXml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes;
            //var cust_no = xnlNodeList[0].InnerText;
            //var cust_ac_no = xnlNodeList[1].InnerText;
            //var ac_desc = xnlNodeList[2].InnerText;
            //var acy_avl_bal = xnlNodeList[3].InnerText;
            //var mobile_number = xnlNodeList[4].InnerText;
            //UbsServiceManagerSoapClient client = new UbsServiceManagerSoapClient(UbsServiceManagerSoapClient.EndpointConfiguration.UbsServiceManagerSoap);
            //var deposit = client.GetCustomerDetailsAsync("", "2011910152053", "ADMIN").Result.Any1;
            //XmlDocument outputXml = new XmlDocument();
            //outputXml.LoadXml(deposit.ToString());
            //XmlNodeList xnlNodeList = outputXml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes;
            //var customerName = xnlNodeList[1].InnerText;
            //var mobile_number = xnlNodeList[7].InnerText;

            HttpContext.Session.Clear();
            HttpContext.Session.Remove(ApplicationConstant.GlobalSessionSession);
            return View();
        }


        [HttpPost]
        public IActionResult Login(SettingsUsers pUser)
        {
            if (ModelState.IsValid)
            {
                var remoteIpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                var appSession = new AppSession();
                var message = userService.DoLogin(pUser, out appSession);

                if (message.MessageType.HasError())
                {
                    pUser.Message = message;
                }
                else
                {
                    appSession.User.StationIp = remoteIpAddress;
                    HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, appSession);
                    MessageHelper.Success(pUser.Message, "Login Successful.");
                }
            }

            else
            {
                MessageHelper.Error(pUser.Message, "Please enter credential");
            }
            return Json(pUser);
        }

        public IActionResult SmsOtpValidation()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ValidateOtp(string otp)
        {
            var message = new Message();
            if (string.IsNullOrEmpty(otp))
            {
                MessageHelper.Error(message, "Invalid OTP.");
                return Json(message);
            }
            var appSession = HttpContext.Session.Get<AppSession>(ApplicationConstant.GlobalSessionSession);
            if (!string.IsNullOrEmpty(appSession.TransactionSession.SmsReqRefNo))
            {
                appSession.TransactionSession.SmsOtpVerifyStatus = otp == appSession.TransactionSession.SmsOtpData ? "S" : "F";

                var msg = commonManager.SetSmsOtpResponseStatus(appSession.TransactionSession.SmsReqRefNo, appSession.TransactionSession.SmsOtpData, otp, appSession.TransactionSession.SmsOtpVerifyStatus, appSession.User.user_id, appSession.User.StationIp);

                if (msg.pvc_status == "40999" && appSession.TransactionSession.SmsOtpVerifyStatus == "S")
                {
                    appSession.User.otp_req = "N"; //N - Neutral. No need to validate OTP
                    HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, appSession);
                    MessageHelper.Success(message, "OTP Verify Successfully.");
                    return Json(message);
                }
                else
                {
                    MessageHelper.Error(message, "Invalid OTP. Please try again.");
                    return Json(message);
                }

            }
            else
            {
                MessageHelper.Error(message, "OTP not yet send. Please send again.");
                return Json(message);
            }
        }

        [HttpPost]
        public IActionResult ReSendOtp()
        {
            var appSession = HttpContext.Session.Get<AppSession>(ApplicationConstant.GlobalSessionSession);
            var message = new Message();
            List<string> msg = commonManager.RequestSmsOtp("04", appSession.User.user_id, appSession.User.mobile, "", appSession.User.user_id, appSession.User.StationIp);
            if (msg[0] == "40999")
            {
                appSession.TransactionSession.SmsReqRefNo = msg[2];
                appSession.TransactionSession.SmsOtpData = msg[3];
                MessageHelper.Success(message, "OTP Re-Send Successfully.");
                return Json(message);
            }
            else
            {
                MessageHelper.Error(message, "Unable to Send OTP SMS. Please try again.");
                return Json(message);

            }
        }

        

    }

    public static class CheckIp
    {
        public static string GetIpAddress(this HttpRequest request)
        {
            var emptyValues = default(StringValues);
            StringValues header = request.Headers.FirstOrDefault(h => h.Key == "CF-Connecting-IP").Value;
            if (header == emptyValues) header = request.Headers.FirstOrDefault(h => h.Key == "X-Forwarded-For").Value;

            if (header != emptyValues)
                return header.First();
            else
                return request.HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}