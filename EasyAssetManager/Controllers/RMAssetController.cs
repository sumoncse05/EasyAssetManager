using EasyAssetManagerCore.BusinessLogic.Operation.Asset;
using EasyAssetManagerCore.BusinessLogic.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace EasyAssetManager.Controllers
{
    public class RMAssetController :BaseController
    {
        private readonly IRMAssetManager agentService;
        private IHostingEnvironment environment;
        private readonly ICommonManager commonManager;
        public RMAssetController(IRMAssetManager rmAssetManager, IHostingEnvironment environment, ICommonManager commonManager)
        {
            this.agentService = rmAssetManager;
            this.environment = environment;
            this.commonManager = commonManager;
        }
        //[ScreenPermission("50111")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAvailableLoan()
        {
            return PartialView("_GetAvailableLoan", null);
        }
    }
}