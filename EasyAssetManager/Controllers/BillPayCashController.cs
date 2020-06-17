using EasyAssetManagerCore.BusinessLogic.Operation;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyAssetManager.Controllers
{
    public class BillPayCashController : BaseController
    {
        private readonly IBillPayCashManager billPayCashManager;
        private IHostingEnvironment environment;
        private readonly IHttpContextAccessor contextAccessor;
        public BillPayCashController(IBillPayCashManager billPayCashManager, IHostingEnvironment environment, IHttpContextAccessor contextAccessor)
        {
            this.billPayCashManager = billPayCashManager;
            this.environment = environment;
            this.contextAccessor = contextAccessor;
        }
        [ScreenPermission("60121")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InitiateTransaction(BillPayCash billPayCash)
        {
            var msg = billPayCashManager.InitiateTransaction(billPayCash, Session, contextAccessor);
            return Json(msg);
        }
        [HttpPost]
        public IActionResult CompleteTransaction()
        {
            var msg = billPayCashManager.CompleteTransaction(Session, contextAccessor);
            var data = new
            {
                message = msg,
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