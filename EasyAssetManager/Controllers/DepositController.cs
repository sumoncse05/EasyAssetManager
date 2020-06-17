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
    public class DepositController : BaseController
    {
        private readonly IDepositManager depositManager;
        private IHostingEnvironment environment;
        private readonly ICommonManager commonManager;
        private readonly IHttpContextAccessor contextAccessor;
        public DepositController(IDepositManager depositManager, IHostingEnvironment environment, ICommonManager commonManager, IHttpContextAccessor contextAccessor)
        {
            this.depositManager = depositManager;
            this.environment = environment;
            this.commonManager = commonManager;
            this.contextAccessor = contextAccessor;
        }
        [ScreenPermission("20121")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ComfirmTransaction(Deposit deposit)
        {
            var msg = depositManager.ComfirmTransaction(deposit,Session, contextAccessor);
            return Json(msg);
        }
        [HttpPost]
        public IActionResult InitiateTransaction(Deposit deposit)
        {
            var msg = depositManager.InitiateTransaction(deposit, Session, contextAccessor);
            return Json(msg);
        }
        [HttpPost]
        public IActionResult GenerateFingerRequest(Deposit deposit)
        {
            var msg = depositManager.GenerateFingerRequest(deposit, Session, contextAccessor);
            return Json(msg);
        }


        [HttpPost]
        public IActionResult CompleteTransaction(string bearerType=null)
        {
            var message = new Message();
            if (!string.IsNullOrEmpty(bearerType))
            {
                if (bearerType == "01" && Session.TransactionSession.FingerVerifyStatus != "S")
                {
                    MessageHelper.Error(message, "Finger not yet verified or Verification Failed.");
                }
                else
                {
                    message = depositManager.CompleteTransaction(Session,contextAccessor);
                }
            }
            else
            {
                message = depositManager.CompleteTransaction(Session,contextAccessor);
            }
            var data = new
            {
                message = message,
                TransactionID = Session.TransactionSession.UbsTransactionRefNo
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

        [HttpPost]
        public IActionResult GetFingerVerifyStatus()
        {
            var msg = depositManager.GetFingerVerifyStatus(Session,contextAccessor);
            return Json(msg);
        }

        [HttpPost]
        public IActionResult ReGenerateFingerRequest()
        {
            var message = depositManager.ReGenerateFingerRequest(Session, contextAccessor);
            return Json(message);
        }
    }

    public class Transaction
    {
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public string Branch { get; set; }
        public string Phone { get; set; }

        public decimal Balance { get; set; }
        public string OtpNumber { get; set; }
        public string Biometric { get; set; }
        public string ToAccountNo { get; set; }
        public decimal TrAmount { get; set; }
    }
}