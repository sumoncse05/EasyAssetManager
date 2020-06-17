using EasyAssetManagerCore.BusinessLogic.Operation;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;

namespace EasyAssetManager.Controllers
{
    public class WithdrawRemittanceController : BaseController
    {
        private readonly IWithdrawRemittanceManager withdrawRemittanceManager;
        private IHostingEnvironment environment;
        private readonly IHttpContextAccessor contextAccessor;
        public WithdrawRemittanceController(IWithdrawRemittanceManager withdrawRemittanceManager, IHostingEnvironment environment,IHttpContextAccessor contextAccessor)
        {
            this.withdrawRemittanceManager = withdrawRemittanceManager;
            this.environment = environment;
            this.contextAccessor = contextAccessor;
        }
        #region AgentPart
        [ScreenPermission("30123")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ComfirmTransaction(Remittance remittance)
        {
            var msg = withdrawRemittanceManager.ComfirmTransaction(remittance, Session, contextAccessor);
            return Json(msg);
        }
        [HttpPost]
        public IActionResult InitiateTransaction(Remittance remittance)
        {
            var msg = withdrawRemittanceManager.InitiateTransaction(remittance, Session, contextAccessor);
            if (msg.MessageType == MessageTypes.Success)
            {
                if (!SetPhoto(Session.TransactionSession.TransactionID, remittance.customer_image_path, Session.TransactionSession.TransactionCustPhotoType))
                {
                    MessageHelper.Error(msg, "Unable to save photo. Save photo again.");
                }
                else
                {
                    MessageHelper.Success(msg, msg.MessageString + " and Customer photo uploaded successfully.");
                }
            }
            var data = new
            {
                message = msg,
                TransactionID = Session.TransactionSession.TransactionID
            };
            return Json(data);
        }
        [HttpPost]
        public IActionResult ClearRegistrationSession()
        {
            Session.TransactionSession = new TransactionSession();
            HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, Session);
            return Json(1);
        }
        private bool SetPhoto(string fileName, string photoFile, string photoType)
        {
            bool Success = false;
            string sourceFile = Path.Combine(environment.WebRootPath, photoFile);
            string destFile = environment.WebRootPath + "\\RemittancePhoto\\" + fileName + "." + photoType;

            Logging.WriteToLog(Session.User.StationIp, Session.User.user_id, "WithdrawRemittance-SetPhoto", sourceFile + "|" + destFile);


            if (sourceFile != destFile)
            {
                try
                {
                    if (System.IO.File.Exists(Path.Combine(@destFile)))
                        System.IO.File.Delete(@destFile);

                    System.IO.File.Copy(@sourceFile, @destFile, true);
                    System.IO.File.Delete(@sourceFile);

                    Success = true;

                }
                catch (Exception ex)
                {
                    Logging.WriteToErrLog(Session.User.StationIp, Session.User.user_id, "WithdrawRemittance-SetPhoto", ex.Message + "|" + ex.StackTrace.TrimStart());
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

        #endregion

        #region AdminPart
        //[ScreenPermission("30125")]
        public IActionResult WithdrawAuthorize()
        {
            return View();
        }
        public IActionResult WithdrawAuthorizeList(string trans_id="")
        {
            var unAuthWithdraws = withdrawRemittanceManager.GetUnathRemittance(trans_id, Session.User.user_id);
            return PartialView("_WithdrawAuthorizeList", unAuthWithdraws);
        }
        //[ScreenPermission("30125")]
        public IActionResult WithdrawAuthorizePreview(string trans_id)
        {
            var unAuthWithdraw = withdrawRemittanceManager.GetUnathRemittance(trans_id, Session.User.user_id).FirstOrDefault();
            return View(unAuthWithdraw);
        }

        [HttpPost]
        public IActionResult UpdateAmount(string trans_id, string amount)
        {
            var msg = withdrawRemittanceManager.SetRemittanceAmount(trans_id, amount, Session);
            return Json(msg);
        }
        [HttpPost]
        public IActionResult DeclineTransaction(string trans_id, string reason)
        {
            var msg = withdrawRemittanceManager.DeclineTransaction(trans_id, reason, Session);
            return Json(msg);
        }
        [HttpPost]
        public IActionResult AuthorizeRemittance(string trans_id, string agent_id, string outlet_id)
        {
            var msg = withdrawRemittanceManager.AuthorizeRemittance(trans_id,agent_id,outlet_id, Session);
            return Json(msg);
        }

        #endregion
    }
}