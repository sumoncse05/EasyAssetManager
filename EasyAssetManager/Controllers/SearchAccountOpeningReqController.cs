using EasyAssetManagerCore.BusinessLogic.Operation;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyAssetManager.Controllers
{
    public class SearchAccountOpeningReqController : BaseController
    {
        private readonly IAccountOpeningReqManager accountOpeningReqManager;
        private IHostingEnvironment environment;
        private readonly ICommonManager commonManager;
        private readonly IHttpContextAccessor contextAccessor;
        public SearchAccountOpeningReqController(IAccountOpeningReqManager accountOpeningReqManager, IHostingEnvironment environment, ICommonManager commonManager, IHttpContextAccessor contextAccessor)
        {
            this.accountOpeningReqManager = accountOpeningReqManager;
            this.environment = environment;
            this.commonManager = commonManager;
            this.contextAccessor = contextAccessor;
        }
        [ScreenPermission("51015")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAccountOpenRequest(string ac_reg_slno, string ac_name) 
        {
            var request = accountOpeningReqManager.GetAccountOpenRequest(ac_reg_slno, ac_name, Session);
            return PartialView("_AccountOpeningRequest", request);
        }
        
       [HttpPost]
        public IActionResult DelAccountOpeningRequest(string ac_reg_slno, string reason)
        {
            var request = accountOpeningReqManager.DelAccountOpeningRequest(ac_reg_slno, reason, Session, contextAccessor);
                return Json(request);
        }
        [HttpPost]
        public IActionResult UpdateAccountOpening(AccountOpening  account)
        {
            var request = accountOpeningReqManager.UpdateAccountOpening(account, Session, contextAccessor);
            return Json(request);
        }
    }
}