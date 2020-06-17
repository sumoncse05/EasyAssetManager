using EasyAssetManagerCore.BusinessLogic.Operation;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyAssetManager.Controllers
{
    public class WithdrawCashController : BaseController
    {
        private readonly IWithdrawCashManager withdrawCashManager;
        private IHostingEnvironment environment;
        private readonly IHttpContextAccessor contextAccessor;
        public WithdrawCashController(IWithdrawCashManager withdrawCashManager, IHostingEnvironment environment, IHttpContextAccessor contextAccessor)
        {
            this.withdrawCashManager = withdrawCashManager;
            this.environment = environment;
            this.contextAccessor = contextAccessor;
        }
        [ScreenPermission("30121")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InitiateTransaction(WithdrawCash withdrawCash)
        {
            var msg = withdrawCashManager.InitiateTransaction(withdrawCash, Session, contextAccessor);
            return Json(msg);
        }

        [HttpPost]
        public IActionResult CustomerVerification()
        {
             return  Json(Session.TransactionSession.CustomerValidated.ToString() + " Customer(s) verified out of " + Session.TransactionSession.AccountOperatingMode.ToString());
        }
        [HttpPost]
        public IActionResult GenerateFingerRequest(WithdrawCash withdrawCash)
        {
            var msg = withdrawCashManager.GenerateFingerRequest(withdrawCash, Session, contextAccessor);
            return Json(msg);
        }
        [HttpGet]
        public IActionResult GetFingerVerifyStatus()
        {
            var msg = withdrawCashManager.GetFingerVerifyStatus(Session, contextAccessor);
            return Json(msg);
        }

        [HttpPost]
        public IActionResult ReGenerateFingerRequest()
        {
            var message = withdrawCashManager.ReGenerateFingerRequest(Session, contextAccessor);
            return Json(message);
        }
        [HttpPost]
        public IActionResult ResendOtp()
        {
            var message = withdrawCashManager.ReRequestSmsOtp(Session, contextAccessor);
            return Json(message);
        }
        [HttpPost]
        public IActionResult RequestOtp()
        {
            var message = withdrawCashManager.RequestSmsOtp(Session, contextAccessor);
            return Json(message);
        }
        [HttpPost]
        public IActionResult VerifyOtp(string otp)
        {
            var message = withdrawCashManager.VerifyOtp(otp, Session, contextAccessor);
            var data = new
            {
                message = message,
                status = Session.TransactionSession.CustomerValidated.ToString() + " Customer(s) verified out of " + Session.TransactionSession.AccountOperatingMode.ToString(),
                customerValidated = Session.TransactionSession.CustomerValidated,
                accountOperatingMode = Session.TransactionSession.AccountOperatingMode
            };
            return Json(data);
        }
        [HttpPost]
        public IActionResult CompleteTransaction()
        {
            var message = withdrawCashManager.CompleteTransaction(Session, contextAccessor);
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
            Session.TransactionSession=new TransactionSession();
            HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, Session);
            return Json(1);
        }
    }
}