using EasyAssetManagerCore.BusinessLogic.Operation;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace EasyAssetManager.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly ICustomerManager customerManager;
        private IHostingEnvironment environment;
        private readonly ICommonManager commonManager;
        private readonly IHttpContextAccessor contextAccessor;
        // public AppSession Session { get; set; }
        public CustomerController(ICustomerManager customerManager, IHostingEnvironment environment, ICommonManager commonManager, IHttpContextAccessor contextAccessor)
        {
            this.customerManager = customerManager;
            this.environment = environment;
            this.commonManager = commonManager;
            this.contextAccessor = contextAccessor;
        }
       [ScreenPermission("50125")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CustomerRegistrationInitialization(Customer customer)
        {
            var message = customerManager.CustomerRegistrationInitialization(customer, Session,contextAccessor);
            return Json(message);
        }
        [HttpPost]
        public IActionResult VerifyOtp(string otp)
        {
            Random random = new Random();
            var customer = customerManager.VerifyOtp(otp, Session,contextAccessor);
            if (customer.CustomerImage != null)
            {
                if (!string.IsNullOrEmpty(customer.CustomerImage.image_text))
                {
                    byte[] imgByteArray = Convert.FromBase64String(customer.CustomerImage.image_text);
                    string imgName = "UserImageUbs.jpg";
                    string imgPath = environment.WebRootPath + "/UserSpace/" + Session.User.user_id;
                    string imgfile = Path.Combine(imgPath, imgName);

                    if (!Directory.Exists(imgPath))
                        Directory.CreateDirectory(imgPath);

                    System.IO.File.WriteAllBytes(imgfile, imgByteArray);
                    customer.CustomerImage.ImageUrl ="UserSpace" + $@"\{Session.User.user_id}"  + $@"\{imgName}" ;
                }
            }

            return Json(customer);
        }


        [HttpPost]
        public IActionResult ResendOtp()
        {
            var message = customerManager.ResendOtpSms(Session,contextAccessor);
            return Json(message);
        }

        [HttpPost]
        public IActionResult FingurePrintAndWebCamImageSave(string olpicUrl, string captureImagePath)
        {
            var message = new Message();
            message = customerManager.FingurePrintAndWebCamImageSave(olpicUrl, captureImagePath, Session);
            if (message.MessageType == MessageTypes.Success)
            {
                if (!string.IsNullOrEmpty(captureImagePath))
                {
                    string[] photoPathSplitted = captureImagePath.Split('.');
                    Session.TransactionSession.TransactionCustPhotoType = photoPathSplitted[photoPathSplitted.Length - 1].Split('"')[0];
                }

                if (!SetPhoto(Session.TransactionSession.TransactionID, captureImagePath, olpicUrl, Session.TransactionSession.TransactionCustomerNo, Session.TransactionSession.TransactionCustPhotoType))
                {
                    MessageHelper.Error(message, "Unable to save photo. Save photo again.");
                }
                else
                {
                    MessageHelper.Success(message, message.MessageString + " and Customer photo uploaded successfully.");
                }
            }
            var data = new
            {
                message = message,
                TransactionID = Session.TransactionSession.TransactionID
            };
            return Json(data);
        }

        [HttpPost]
        public IActionResult ClearRegistrationSession()
        {
            Session.TransactionSession=new TransactionSession();
            Session.Customer = new Customer();
            HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, Session);
            return Json(1);
        }
        [HttpPost]
        public IActionResult GetFingerEnrollStatus()
        {
            var msg = customerManager.GetFingerEnrollStatus(Session);
            return Json(msg);
        }

        [HttpPost]
        public IActionResult ReGenerateFingerRequest()
        {
            var message = customerManager.ReGenerateFingerRequest(Session, contextAccessor);
            return Json(message);
        }
        private bool SetPhoto(string fileName, string photoFile, string photoFileCbs, string cust_no, string photoType)
        {
            bool Success = false;
            string sourceFile =Path.Combine(environment.WebRootPath,photoFile);
            string destFile = environment.WebRootPath+ "\\photo\\" + cust_no + "." + photoType;

            Logging.WriteToLog(Session.User.StationIp, Session.User.user_id, "CustomerController-SetPhoto", sourceFile + "|" + destFile);


            if (sourceFile != destFile)
            {
                try
                {
                    if (System.IO.File.Exists(Path.Combine(@destFile)))
                         System.IO.File.Delete(@destFile);

                        System.IO.File.Copy(@sourceFile, @destFile, true);
                        System.IO.File.Delete(@sourceFile);
                        if (!string.IsNullOrEmpty(photoFileCbs))
                        {
                            sourceFile = Path.Combine(environment.WebRootPath, photoFileCbs);
                            destFile = Path.Combine(environment.WebRootPath, "photo\\archive\\" + cust_no + "." + photoType);
                        if (System.IO.File.Exists(Path.Combine(@destFile)))
                              System.IO.File.Delete(@destFile);

                            System.IO.File.Copy(@sourceFile, @destFile, true);
                            System.IO.File.Delete(@sourceFile);
                        }
                        Success = true;
                    
                }
                catch (Exception ex)
                {
                    Logging.WriteToErrLog(Session.User.StationIp, Session.User.user_id, "CustomerController-SetPhoto", ex.Message + "|" + ex.StackTrace.TrimStart());
                }
            }
            else
            {
                Success = true;
            }
            return Success;
        }

        [HttpPost]
        public IActionResult PhotoUpload(string name)
        {
            var files = HttpContext.Request.Form.Files;
            var filepath = string.Empty;
            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                         filepath = Path.Combine(environment.WebRootPath, "UserSpace") + $@"\{Session.User.user_id}" + "\\UserImage.jpg";
                        var directory = Path.Combine(environment.WebRootPath, "UserSpace") + $@"\{Session.User.user_id}";
                        if (!string.IsNullOrEmpty(filepath))
                        {
                            if (System.IO.File.Exists(Path.Combine(filepath)))
                                System.IO.File.Delete(Path.Combine(filepath));
                            if (!Directory.Exists(directory))
                                Directory.CreateDirectory(directory);
                            StoreInFolder(file, filepath);
                        }

                    }
                }
                return Json("UserSpace" + $@"\{Session.User.user_id}" + "\\UserImage.jpg");
            }
            else
            {
                return Json(filepath);
            }

        }

        private void StoreInFolder(IFormFile file, string fileName)
        {
            using (FileStream fs = System.IO.File.Create(fileName))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
        }


    }
}