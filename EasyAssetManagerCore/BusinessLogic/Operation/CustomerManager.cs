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
    public class CustomerManager : BaseService, ICustomerManager
    {
        private readonly ICustomerRepository customerRepository;
        private readonly ICommonManager commonManager;
        private readonly ICbsDataConnectionManager cbsDataConnectionManager;
        public CustomerManager() : base((int)ConnectionStringEnum.EbankConnectionString)
        {
            customerRepository = new CustomerRepository(Connection);
            commonManager = new CommonManager();
            cbsDataConnectionManager = new CbsDataConnectionManager();
        }

        public Message CustomerRegistrationInitialization(Customer customer, AppSession session, IHttpContextAccessor contextAccessor)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var olCustomer = customerRepository.GetCustomer(customer.customer_no, session.User.user_id);
                if (olCustomer != null)
                {
                    if (olCustomer.customer_status == "R")
                    {
                        //httpContext.HttpContext.Session.Get<AppSession>(ApplicationConstant.GlobalSessionSession);
                        //httpContext.HttpContext.Session.SetString("","");
                        MessageHelper.Error(Message, "Customer already registered. Please do transaction.");
                        return Message;
                       
                    }
                    else
                    {
                        if (olCustomer.mobile_number != customer.mobile_number)
                        {
                            MessageHelper.Error(Message, "Mobile no mismatch. Please try again.");
                            return Message;
                        }
                        else if (olCustomer.date_of_birth != customer.date_of_birth)
                        {
                            MessageHelper.Error(Message, "Birth date mismatch. Please try again.");
                            return Message;

                        }
                        else
                        {
                            session.TransactionSession.TransactionCustomerNo = olCustomer.customer_no;
                            session.TransactionSession.TransactionCustMobileNo = olCustomer.mobile_number;
                            session.Customer = olCustomer;
                            var msg = customerRepository.CustomerInitiateRegistration(olCustomer, session);
                            if (msg.pvc_status == "40999")
                            {
                                session.TransactionSession.TransactionID = msg.pvc_regslno;
                                if (!string.IsNullOrEmpty(session.TransactionSession.TransactionCustMobileNo))
                                {
                                   var msg2 = commonManager.RequestSmsOtp("04", session.TransactionSession.TransactionCustomerNo, session.TransactionSession.TransactionCustMobileNo, session.TransactionSession.TransactionID, session.User.user_id, session.User.StationIp);
                                    if (msg2[0] == "40999")
                                    {
                                        session.TransactionSession.SmsReqRefNo = msg2[2];
                                        session.TransactionSession.SmsOtpData = msg2[3];
                                        MessageHelper.Success(Message, "Customer Registration Initiate Successfull.");
                                    }
                                    else
                                    {
                                        MessageHelper.Error(Message, "Unable to Send OTP SMS. Please try again");
                                    }

                                }
                                else
                                {
                                    MessageHelper.Error(Message, "Customer Mobile No not found. Please try again.");
                                }
                            }
                            else
                            {
                                MessageHelper.Error(Message, msg.pvc_statusmsg);
                            }
                        }
                    }
                }
                else
                {
                    MessageHelper.Error(Message, "Invalid customer no. Please check.");
                }
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "CustomerManager-CustomerRegistrationInitialization", ex.Message + "|" + ex.StackTrace.TrimStart());
                MessageHelper.Error(Message, "System Error.");
            }
            finally
            {
                Connection.Close();
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
            }
            return Message;
        }

        public Message FingurePrintAndWebCamImageSave(string olpicUrl, string captureImagePath, AppSession session)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                //check finger status
                if (session.TransactionSession.FingerVerifyStatus == "E")
                {
                    MessageHelper.Error(Message, "Finger not yet Enrolled.");
                    return Message;
                }
                else if (string.IsNullOrEmpty(captureImagePath))
                {
                    MessageHelper.Error(Message, "Please capture customer image.");
                    return Message;
                }
                else
                {
                    var msg = customerRepository.SetRegistrationCompleted(session.TransactionSession.TransactionID, "50999", session.User.user_id);
                    if (msg.pvc_status == "40999")
                    {
                        MessageHelper.Success(Message, "Customer registration completed successfully.");
                    }
                    else
                    {
                        MessageHelper.Error(Message, "Unable to complete registration request.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.Error(Message, "System Error");
            }
            finally
            {
                Connection.Close();
            }
            return Message;
        }

        public Message ReGenerateFingerRequest(AppSession session, IHttpContextAccessor contextAccessor)
        {
            var retMsg = new ResponseMessage();
            var message = new Message();
            try
            {
                var msg = commonManager.GetFingerScanStatus(session.TransactionSession.FingerReqRefNo, session.User.user_id, session.User.StationIp);
                if (msg.pvc_status == "40999" && msg.pvc_statusmsg == "S")
                {
                    MessageHelper.Error(message, "Can not Re-Generate. Already Enrolled. Refreh.");
                }
                else
                {
                    retMsg = commonManager.RequestFingerScan("E", "04", session.TransactionSession.TransactionCustomerNo, session.TransactionSession.TransactionID, session.User.user_id, session.User.StationIp);

                    if (retMsg.pvc_status == "40999")
                    {
                        session.TransactionSession.FingerReqRefNo = retMsg.pvc_otpreqrefno;
                        MessageHelper.Success(message, "Scan Request Generated successfully. Scan Thumb to Register.");

                    }

                }
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "CustomerManager-ReGenerateFingerRequest", ex.Message + "|" + ex.StackTrace.TrimStart());
                MessageHelper.Error(message, "System Error.");
            }
            finally
            {
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
            }
            return Message;
        }

        public Customer GetCustomer(string customerNo, string userId)
        {
            return customerRepository.GetCustomer(customerNo, userId);
        }

        public ResponseMessage GetFingerEnrollStatus(AppSession session)
        {
            var msg = new ResponseMessage();
            try
            {
                msg = commonManager.GetFingerScanStatus(session.TransactionSession.FingerReqRefNo, session.User.user_id, session.User.StationIp);

            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "CustomerManager-GetFingerEnrollStatus", ex.Message + "|" + ex.StackTrace.TrimStart());
            }
            finally
            {
            }
            return msg;
        }

        public Message ResendOtpSms(AppSession session, IHttpContextAccessor contextAccessor)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                var msg = commonManager.RequestSmsOtp("04", session.TransactionSession.TransactionCustomerNo, session.TransactionSession.TransactionCustMobileNo, session.TransactionSession.TransactionID, session.User.user_id, session.User.StationIp);
                if (msg[0] == "40999")
                {
                    session.TransactionSession.SmsReqRefNo = msg[2];
                    session.TransactionSession.SmsOtpData = msg[3];
                    MessageHelper.Success(Message, "SMS OTP send again. Ask Customer to Check Mobile and Input OTP here.");
                }
                else
                {
                    MessageHelper.Error(Message, "Unable to Send OTP SMS. Please try again.");
                }
            }
            catch (Exception ex)
            {
                Logging.WriteToErrLog(session.User.StationIp, session.User.user_id, "CustomerManager-ResendOtpSms", ex.Message + "|" + ex.StackTrace.TrimStart());
                MessageHelper.Error(Message, "Unable to Send OTP SMS. Please try again.");
            }
            finally
            {
                Connection.Close();
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
            }
            return Message;
        }

        public Customer VerifyOtp(string otp, AppSession session, IHttpContextAccessor contextAccessor)
        {
            var customer = new Customer();
            Random random = new Random();
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                if (!string.IsNullOrEmpty(session.TransactionSession.SmsReqRefNo))
                {
                    if (!string.IsNullOrEmpty(otp))
                    {
                        session.TransactionSession.SmsOtpVerifyStatus = otp == session.TransactionSession.SmsOtpData ? "S" : "F";
                        var msg = commonManager.SetSmsOtpResponseStatus(session.TransactionSession.SmsReqRefNo, session.TransactionSession.SmsOtpData, otp, session.TransactionSession.SmsOtpVerifyStatus, session.User.user_id, session.User.StationIp);

                        if (msg.pvc_status == "40999")
                        {
                            if (session.TransactionSession.SmsOtpVerifyStatus == "S")
                            {
                                var reqResp = commonManager.RequestFingerScan("E", "04", session.TransactionSession.TransactionCustomerNo, session.TransactionSession.TransactionID, session.User.user_id, session.User.StationIp);

                                if (reqResp.pvc_status == "40999")
                                {
                                    MessageHelper.Success(Message, "Scan Request Generated successfully. Scan Thumb to Register.");
                                    session.TransactionSession.FingerReqRefNo = reqResp.pvc_otpreqrefno;
                                    customer = session.Customer;

                                    customer.CustomerImage = cbsDataConnectionManager.GetCustomerImage(session.TransactionSession.TransactionCustomerNo, session.User.user_id);
                                }
                                else
                                {
                                    MessageHelper.Error(Message, reqResp.pvc_statusmsg);
                                }
                            }
                            else
                            {
                                MessageHelper.Error(Message, "Invalid OTP. Please try again.");
                                session.TransactionSession.SmsOtpVerifyStatus = "F";
                            }
                        }
                    }
                    else
                    {
                        MessageHelper.Error(Message, "Ask Customer to Check Mobile and Input OTP here.");
                    }
                }
                else
                {
                    MessageHelper.Error(Message, "OTP not yet send. Please send again.");
                }
            }
            catch (Exception ex)
            {
                MessageHelper.Error(Message, "System Error.");
            }
            finally
            {
                Connection.Close();
                contextAccessor.HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, session);
                customer.Message = Message;
            }
            return customer;
        }

        public IEnumerable<Customer> GetCustomersByAccountNumber(string pvc_custacno, string pvc_appuser)
        {
            return customerRepository.GetCustomersByAccountNumber(pvc_custacno, pvc_appuser);
        }       
    }

    public interface ICustomerManager
    {
        Customer GetCustomer(string customerNo, string userId);
        Message CustomerRegistrationInitialization(Customer customer, AppSession session, IHttpContextAccessor contextAccessor);
        Customer VerifyOtp(string otp, AppSession session, IHttpContextAccessor contextAccessor);
        Message ResendOtpSms(AppSession session, IHttpContextAccessor contextAccessor);
        Message FingurePrintAndWebCamImageSave(string olpicUrl, string captureImagePath, AppSession session);
        ResponseMessage GetFingerEnrollStatus(AppSession session);
        Message ReGenerateFingerRequest(AppSession session, IHttpContextAccessor contextAccessor);
        IEnumerable<Customer> GetCustomersByAccountNumber(string pvc_custacno, string pvc_appuser);
    }
}
