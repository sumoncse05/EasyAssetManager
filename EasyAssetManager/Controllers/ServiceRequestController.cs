using EasyAssetManagerCore.BusinessLogic.Operation;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyAssetManager.Controllers
{
    public class ServiceRequestController : BaseController
    {
        private readonly IServiceRequestManager serviceRequestManager;
        private readonly IAccountManager accountManager;
        private IHostingEnvironment environment;
        private readonly ICommonManager commonManager;
        private readonly IHttpContextAccessor contextAccessor;

        public ServiceRequestController(IServiceRequestManager serviceRequestManager, IAccountManager accountManager, IHostingEnvironment environment, ICommonManager commonManager, IHttpContextAccessor contextAccessor)
        {
            this.serviceRequestManager = serviceRequestManager;
            this.accountManager = accountManager;
            this.environment = environment;
            this.commonManager = commonManager;
            this.contextAccessor = contextAccessor;
        }
        [ScreenPermission("50201")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetAccountDetails(string cust_ac_no, string wf_type, string remarks)
        {
            var accounts = accountManager.GeAccountDetailsWithSession(cust_ac_no, Session.User.user_id, Session, contextAccessor);            
            return Json(accounts);
        }
        [HttpPost]
        public IActionResult CustomerVerification(string customer_no, string wf_type, string remarks) 
        {
            var msg = serviceRequestManager.CustomerVerification(customer_no, wf_type, remarks, Session, contextAccessor);
            return Json(msg);
        }
        [HttpPost]
        public IActionResult CustomerFingerVerification(string wf_desc)
        {
            var msg = serviceRequestManager.CustomerFingerVerification(wf_desc, Session, contextAccessor); 
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
            var msg = serviceRequestManager.GetFingerVerifyStatus(Session, contextAccessor);
            return Json(msg);
        }
        [HttpPost]
        public IActionResult ReGenerateFingerRequest()
        {
            var message = serviceRequestManager.ReGenerateFingerRequest(Session, contextAccessor);
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
        [ScreenPermission("50203")]
        public IActionResult WorkFlowServiceRequestAuthorize() 
        {
            return View();
        }
        public IActionResult GetUnauthWorkflowRequest(string wfslno)
        {
            var serviceRequest = serviceRequestManager.GetUnauthWorkflowRequest(wfslno, "01", Session.User.user_id);
            return PartialView("_UnauthWorkFlowList", serviceRequest);
        }
        [HttpPost]
        public IActionResult GetServiceReferenceDtl(string wfslno,string wf_ref_no,string wf_type)
        {
            var serviceRequest = serviceRequestManager.GetServiceReferenceDtl(wfslno, wf_ref_no, wf_type, Session.User.user_id);
            return Json(serviceRequest);
        }
        [HttpPost]
        public IActionResult AuthorizeServiceRequest(string wfslno, string wf_ref_no,string wf_type,string wf_resp_dtl)
        {
            var message = serviceRequestManager.AuthorizeServiceRequest(wfslno, wf_ref_no, wf_type, wf_resp_dtl, Session.User.user_id);
            return Json(message);
        }
    }
}