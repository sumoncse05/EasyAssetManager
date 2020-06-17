using EasyAssetManagerCore.BusinessLogic.Operation;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EasyAssetManager.Controllers
{
    public class AccountOpeningController : BaseController
    {
        private readonly IAccountOpeningManager accountOpeningManager;
        private IHostingEnvironment environment;
        private readonly ICommonManager commonManager;
        private readonly IHttpContextAccessor contextAccessor;

        public AccountOpeningController(IAccountOpeningManager accountOpeningManager, IHostingEnvironment environment, ICommonManager commonManager, IHttpContextAccessor contextAccessor)
        {
            this.accountOpeningManager = accountOpeningManager;
            this.environment = environment;
            this.commonManager = commonManager;
            this.contextAccessor = contextAccessor;
        }
        [ScreenPermission("50129")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NewAccountOpening(AccountOpening accountOpening) 
        {

            var message = accountOpeningManager.NewAccountOpening(accountOpening, Session, contextAccessor);
            var data = new
            {
                message = message,
                transactionID = Session.TransactionSession.TransactionID
            };
            return Json(data);
        }
        [HttpPost]
        public IActionResult ConfirmAccountOpen(AccountOpening accountOpening)
        {
            return Json(accountOpening);
        }
        [HttpPost]
        public IActionResult ClearRegistrationSession()
        {
            Session.TransactionSession = new TransactionSession();
            HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, Session);
            return Json(1);
        }

        [ScreenPermission("50133")]
        public IActionResult AccountOpeningAuthorize()
        {
            return View();
        }

        public IActionResult GetAccountOpeningAuthorize(string branchId=null, string requestId=null)
        {
            var unAuthorizeAccounts = accountOpeningManager.GetAccountOpeningRequest(branchId, requestId, Session.User.user_id);
            return PartialView("_AllAccountOpeningAuthorize", unAuthorizeAccounts);
        }
        [HttpGet]
        public IActionResult AccountAuthorizePreview(string ac_reg_slno)
        {
            var unAuthorizeAccount = accountOpeningManager.GetAccountOpeningRequest("", ac_reg_slno, Session.User.user_id).FirstOrDefault();
            return View("AccountAuthorizePreview", unAuthorizeAccount);
        }
        [HttpGet]
        public IActionResult GetAccountDtlCbs(string cust_ac_no)
        {
            var account= accountOpeningManager.GeAccountDetails(cust_ac_no, Session.User.user_id).FirstOrDefault();
            return Json(account);
        }
        [HttpPost]
        public IActionResult AuthorizeAccount(string ac_reg_slno, string cust_ac_no, string ac_desc, string ac_open_date)
        {
            var message = accountOpeningManager.AuthorizeAccountOpeningRequest(ac_reg_slno,cust_ac_no, ac_desc, ac_open_date, Session);
            return Json(message);
        }
    }
}