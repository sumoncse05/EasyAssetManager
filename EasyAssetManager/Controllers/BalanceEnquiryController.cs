using EasyAssetManagerCore.BusinessLogic.Operation;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace EasyAssetManager.Controllers
{
    public class BalanceEnquiryController : BaseController
    {

        private readonly IBalanceEnquiryManager balanceEnquiryManager;
        private IHostingEnvironment environment;
        private readonly IAccountManager accountManager;
        private readonly ICommonManager commonManager;
        private readonly IHttpContextAccessor contextAccessor;

        public BalanceEnquiryController(IBalanceEnquiryManager balanceEnquiryManager,IAccountManager accountManager,IHostingEnvironment environment,ICommonManager commonManager,IHttpContextAccessor contextAccessor)
        {
            this.balanceEnquiryManager = balanceEnquiryManager;
            this.accountManager = accountManager;
            this.environment = environment;
            this.commonManager = commonManager;
            this.contextAccessor = contextAccessor;
        }
        [ScreenPermission("31005")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetAccountDetails(string cust_ac_no) 
        {
            var accounts = accountManager.GeAccountDetailsWithSession(cust_ac_no,Session.User.user_id,Session,contextAccessor);
            return Json(accounts);
        }
        [HttpPost]
        public IActionResult CustomerVerificatin(string customer_no) 
        {
            var msg = balanceEnquiryManager.CustomerVerification(customer_no, Session,contextAccessor);
            return Json(msg);
        }
        [HttpPost]
        public IActionResult SendAccountBalance(string customer_no)
        {
            var msg = balanceEnquiryManager.SendAccountBalance(Session, contextAccessor);
            var data = new
            {
                message = msg,
                transactionID = Session.TransactionSession.TransactionID
            };
            return Json(data);
        }
        [HttpPost]
        public IActionResult GetFingerVerifyStatus()
        {
            var msg = balanceEnquiryManager.GetFingerVerifyStatus(Session, contextAccessor);
            return Json(msg);
        }
        [HttpPost]
        public IActionResult ReGenerateFingerRequest()
        {
            var message = balanceEnquiryManager.ReGenerateFingerRequest(Session, contextAccessor);
            return Json(message);
        }
        [HttpPost]
        public IActionResult ConfirmServiceRequest(ServiceRequestManager serviceRequestManager)
        {
            return Json(serviceRequestManager);
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