using EasyAssetManagerCore.BusinessLogic.Operation;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyAssetManager.Controllers
{
    public class FundTransferController : BaseController
    {
        private readonly IFundTransferManager fundTransferManager;
        private IHostingEnvironment environment;
        private readonly ICommonManager commonManager;
        private readonly IHttpContextAccessor contextAccessor;
        public FundTransferController(IFundTransferManager fundTransferManager, IHostingEnvironment environment, ICommonManager commonManager, IHttpContextAccessor contextAccessor)
        {
            this.fundTransferManager = fundTransferManager;
            this.environment = environment;
            this.commonManager = commonManager;
            this.contextAccessor = contextAccessor;
        }
        [ScreenPermission("70121")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InitiateTransaction(FundTransfer fundTransfer)
        {
            var receiveFundTransfer = fundTransferManager.InitiateTransaction(fundTransfer, Session, contextAccessor);
            return Json(receiveFundTransfer);
        }

        [HttpPost]
        public IActionResult CustomerVerification()
        {
            return Json(Session.TransactionSession.CustomerValidated.ToString() + " Customer(s) verified out of " + Session.TransactionSession.AccountOperatingMode.ToString());
        }
        [HttpPost]
        public IActionResult GenerateFingerRequest(FundTransfer fundTransfer)
        {
            var msg = fundTransferManager.GenerateFingerRequest(fundTransfer, Session, contextAccessor);
            return Json(msg);
        }
        [HttpPost]
        public IActionResult GetFingerVerifyStatus()
        {
            var msg = fundTransferManager.GetFingerVerifyStatus(Session, contextAccessor);
            return Json(msg);
        }

        [HttpPost]
        public IActionResult ReGenerateFingerRequest()
        {
            var message = fundTransferManager.ReGenerateFingerRequest(Session, contextAccessor);
            return Json(message);
        }
        [HttpPost]
        public IActionResult ResendOtp()
        {
            var message = fundTransferManager.ReRequestSmsOtp(Session, contextAccessor);
            return Json(message);
        }
        [HttpPost]
        public IActionResult RequestOtp()
        {
            var message = fundTransferManager.RequestSmsOtp(Session, contextAccessor);
            return Json(message);
        }
        [HttpPost]
        public IActionResult VerifyOtp(string otp)
        {
            var message = fundTransferManager.VerifyOtp(otp,Session, contextAccessor);
            var data = new
            {
                message = message,
                status= Session.TransactionSession.CustomerValidated.ToString() + " Customer(s) verified out of " + Session.TransactionSession.AccountOperatingMode.ToString(),
                customerValidated= Session.TransactionSession.CustomerValidated,
                accountOperatingMode= Session.TransactionSession.AccountOperatingMode
            };
            return Json(data);
        }
        [HttpPost]
        public IActionResult CompleteTransaction()
        {
            var message = fundTransferManager.CompleteTransaction(Session, contextAccessor);
            var data = new
            {
                message = message,
                transactionID = Session.TransactionSession.UbsTransactionRefNo
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
    }
}