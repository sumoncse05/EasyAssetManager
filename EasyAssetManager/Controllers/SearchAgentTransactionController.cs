using EasyAssetManagerCore.BusinessLogic.Operation;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Models.CommonModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyAssetManager.Controllers
{
    public class SearchAgentTransactionController : BaseController
    {
        private readonly ISearchAgentTransactionManager searchAgentTransactionManager;
        private IHostingEnvironment environment;
        private readonly ICommonManager commonManager;
        private readonly IHttpContextAccessor contextAccessor;
        public SearchAgentTransactionController(ISearchAgentTransactionManager searchAgentTransactionManager, IHostingEnvironment environment, ICommonManager commonManager, IHttpContextAccessor contextAccessor)
        {
            this.searchAgentTransactionManager = searchAgentTransactionManager;
            this.environment = environment;
            this.commonManager = commonManager;
            this.contextAccessor = contextAccessor;
        }
        [ScreenPermission("31003")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAgentTransaction(string transaction_no, string customer_cif, string account_no)
        {
            var request = searchAgentTransactionManager.GetAccountOpenRequest(transaction_no, customer_cif, account_no, Session);
            return PartialView("_SearchAgentTransaction", request);
        }
        public IActionResult PrintTransactionSlip(string trans_id) 
        {
            var result = searchAgentTransactionManager.GetTransactionDetails(trans_id, Session,contextAccessor);
            return View(result);
        }
    }
}