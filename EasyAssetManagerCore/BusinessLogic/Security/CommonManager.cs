using EasyAssetManagerCore.BusinessLogic.Common;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using EasyAssetManagerCore.Repository.Common;
using EasyAssetManagerCore.Repository.Security;
using System;
using System.Collections.Generic;
using System.Data;

namespace EasyAssetManagerCore.BusinessLogic.Security
{
    public class CommonManager: BaseService, ICommonManager
    {
        private readonly IOtpRepository otpRepository;
        private readonly SmsManager smsManager;
        private readonly ICommonRepository commonRepository;
        public CommonManager() : base((int)ConnectionStringEnum.EbankConnectionString)
        {
            otpRepository = new OtpRepository(Connection);
            smsManager = new SmsManager();
            commonRepository = new CommonRepository(Connection);
        }

        public ResponseMessage RequestFingerScan(string req_type, string user_type, string user_ref_no, string trans_ref_no, string user_Id, string userStationIp)
        {
            var otpReqResp = new ResponseMessage();
            try
            {
                Logging.WriteToOtpLog(userStationIp + "|" + user_Id + "|" + "GlobalOtpManager.cs|RequestFingerScan" + "|" + req_type + "|" + user_type + "|" + user_ref_no + "|" + trans_ref_no);
                otpReqResp = otpRepository.SetFingerScanRequest(req_type, user_type, user_ref_no, trans_ref_no, user_Id);
            }
            catch (Exception ex)
            {
                Logging.WriteToOtpErrLog(userStationIp + "|" + user_Id + "|" + "GlobalOtpManager.cs|RequestFingerScan" + "|" + ex.Message + "|" + ex.StackTrace.TrimStart());
            }
            finally
            {
            }

            return otpReqResp;
        }

        public ResponseMessage GetFingerScanStatus(string otp_ref_no,string userId, string userStationIp)
        {
            // N - New, S - Successfully Verified, F - Verification failed

            var statusMsg = new ResponseMessage();
            try
            {
                Logging.WriteToOtpLog(userStationIp + "|" + userId + "|" + "GlobalOtpManager.cs|GetFingerScanStatus" + "|" + otp_ref_no);
                statusMsg = otpRepository.GetFingerScanStatus(otp_ref_no, userId);
            }
            catch (Exception ex)
            {
                Logging.WriteToOtpErrLog(userStationIp + "|" + userId + "|" + "GlobalOtpManager.cs|GetFingerScanStatus" + "|" + ex.Message + "|" + ex.StackTrace.TrimStart());
            }
            finally
            {
            }

            return statusMsg;
        }
        public List<string> RequestSmsOtp(string user_type, string user_ref_no, string user_mob_no, string trans_ref_no,string user_Id, string userStationIp)
        {
            List<string> otpReqResp = new List<string> { "40900", "", "" };
            try
            {
                Logging.WriteToOtpLog(userStationIp + "|" + user_Id + "|" + "GlobalOtpManager.cs|RequestSmsOtp" + "|" + user_type + "|" + user_ref_no + "|" + user_mob_no + "|" + trans_ref_no);
                otpReqResp = otpRepository.SetSmsOtpRequest(user_type, user_ref_no, user_mob_no, trans_ref_no, user_Id);

                if (otpReqResp[0] == "40999")
                {
                    string[] smsResp = smsManager.SendSms("005", user_mob_no, otpReqResp[3] + " is your One Time Password (OTP) for EBL Agent Banking. EBL Helpline 16230");
                    if (smsResp[2] == "")
                    {
                        otpReqResp = new List<string> { "40900", "Unable to send sms. Contact administrator." };
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteToOtpErrLog(userStationIp + "|" + user_Id + "|" + "GlobalOtpManager.cs|RequestSmsOtp" + "|" + ex.Message + "|" + ex.StackTrace.TrimStart());
                otpReqResp = new List<string> { "40900", "Unable to send sms. Contact administrator." };
            }
            finally
            {
            }

            return otpReqResp;
        }

        public ResponseMessage SetSmsOtpResponseStatus(string otp_ref_no, string req_sms_data, string res_sms_data, string res_sms_status, string user_Id, string userStationIp)
        {
            var verifyMsg = new ResponseMessage();
            try
            {
                Logging.WriteToOtpLog(userStationIp + "|" + user_Id + "|" + "GlobalOtpManager.cs|SetSmsOtpResponseStatus" + "|" + otp_ref_no + "|" + req_sms_data + "|" + res_sms_data + "|" + res_sms_status);
                verifyMsg = otpRepository.SetSmsOtpResponse(otp_ref_no, req_sms_data, res_sms_data, res_sms_status, user_Id);
            }
            catch (Exception ex)
            {
                Logging.WriteToOtpErrLog(userStationIp + "|" + user_Id + "|" + "GlobalOtpManager.cs|SetSmsOtpResponseStatus" + "|" + ex.Message + "|" + ex.StackTrace.TrimStart());
            }
            finally
            {

            }

            return verifyMsg;
        }

        public string[] RequestSmsAlert(string user_ref_no, string user_mob_no, string sms_message, string user_Id, string userStationIp)
        {
            string[] smsResp = new string[] { "S", "", "" };
            try
            {
                Logging.WriteToOtpLog(userStationIp + "|" + user_Id + "|" + "GlobalOtpManager.cs|RequestFingerScan" + "|" + user_ref_no + "|" + user_mob_no + "|" + sms_message);
                smsResp = smsManager.SendSms("005", user_mob_no, sms_message);
            }
            catch (Exception ex)
            {
                Logging.WriteToOtpErrLog(userStationIp + "|" + user_Id + "|" + "GlobalOtpManager.cs|RequestSmsAlert" + "|" + ex.Message + "|" + ex.StackTrace.TrimStart());
                smsResp = new string[] { "E", "Unable to send sms. Contact administrator.", "" };
            }
            finally
            {

            }

            return smsResp;
        }

        public string[] SendSms(string mobileno,string message)
        {
            string[] smsResp = new string[] { "S", "", "" };
            try
            {
                smsResp = smsManager.SendSms("005", mobileno, message);
            }
            catch (Exception ex)
            {
                smsResp = new string[] { "E", "Unable to send sms. Contact administrator.", "" };
            }
            finally
            {

            }

            return smsResp;
        }

        public IEnumerable<BatchProcess> GetProcessErrMsgCommand(string pRunId)
        {
            return commonRepository.GetProcessErrMsgCommand(pRunId);
        }

        public IEnumerable<BatchProcess> GetProcessMsgCommand(string pRunId)
        {
            return commonRepository.GetProcessMsgCommand(pRunId);
        }

        public IEnumerable<BatchProcess> GetProcessRunStatus(string pRunId, string pUser)
        {
            return commonRepository.GetProcessRunStatus(pRunId, pUser);
        }

        public Message GetAccountStatus(BatchProcess batchProcess, AppSession session, out string runId)
        {
            runId = "0";
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var msg = commonRepository.GetAccountStatus(batchProcess, session.User.user_id);
                runId = msg.pnm_run_id;
                MessageHelper.Success(Message, msg.pvc_msg);
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

    public interface ICommonManager
    {
        List<string> RequestSmsOtp(string user_type, string user_ref_no, string user_mob_no, string trans_ref_no, string user_Id, string userStationIp);
        ResponseMessage GetFingerScanStatus(string otp_ref_no, string userId, string userStationIp);
        ResponseMessage RequestFingerScan(string req_type, string user_type, string user_ref_no, string trans_ref_no, string user_Id, string userStationIp);
        ResponseMessage SetSmsOtpResponseStatus(string otp_ref_no, string req_sms_data, string res_sms_data, string res_sms_status, string user_Id, string userStationIp);
        string[] RequestSmsAlert(string user_ref_no, string user_mob_no, string sms_message, string user_Id, string userStationIp);
        string[] SendSms(string mobileno, string message);

        Message GetAccountStatus(BatchProcess batchProcess, AppSession appSession, out string runId);
        IEnumerable<BatchProcess> GetProcessRunStatus(string pRunId, string pUser);
        IEnumerable<BatchProcess> GetProcessMsgCommand(string pRunId);
        IEnumerable<BatchProcess> GetProcessErrMsgCommand(string pRunId);
    }
}
