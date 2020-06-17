using EasyAssetManagerCore.BusinessLogic.Operation;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Models.CommonModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyAssetManager.Controllers
{
    public class SearchAccountWorkFlowReqController : BaseController
    {
        private readonly ISearchAccountWorkFlowReqManager searchAccountWorkFlowReqManager;
        private IHostingEnvironment environment;
        private readonly ICommonManager commonManager;
        private readonly IHttpContextAccessor contextAccessor;

        public SearchAccountWorkFlowReqController(ISearchAccountWorkFlowReqManager searchAccountWorkFlowReqManager, IHostingEnvironment environment, ICommonManager commonManager, IHttpContextAccessor contextAccessor)
        {
            this.searchAccountWorkFlowReqManager = searchAccountWorkFlowReqManager;
            this.environment = environment;
            this.commonManager = commonManager;
            this.contextAccessor = contextAccessor;
        }
        [ScreenPermission("51017")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAccountWorkFowRequest(string ac_reg_slno, string ac_name) 
        {
            var request = searchAccountWorkFlowReqManager.GetAccountWorkFowRequest(ac_reg_slno, ac_name, Session);
            return PartialView("_AccountWorkFlowReq", request);
        }
    }
}