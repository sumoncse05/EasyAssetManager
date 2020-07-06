using EasyAssetManagerCore.BusinessLogic.Operation.Asset;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EasyAssetManager.Controllers
{
    public class RMAssetController :BaseController
    {
        private readonly IRMAssetManager rmAssetManager;
        private IHostingEnvironment environment;
        public RMAssetController(IRMAssetManager rmAssetManager, IHostingEnvironment environment)
        {
            this.rmAssetManager = rmAssetManager;
            this.environment = environment;
        }
        //[ScreenPermission("50111")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAvailableLoan(string loanType,string loanProduct,string rmCode)
        {
            var availableLoans = rmAssetManager.GetAvailableLoan(loanType, loanProduct, rmCode,Session);
            return PartialView("_GetAvailableLoan", availableLoans);
        }
        [HttpPost]
        public IActionResult SetAssignRM(string loanList,string newRmCode,string effectDate)
        {
            var loans = loanList.Split(',').ToList();
            var message = rmAssetManager.SetAssignRM(loans, newRmCode, effectDate, Session);
            return Json(message);
        }

        //[ScreenPermission("50111")]
        public IActionResult RMAuthorize()
        {
            return View();
        }
        public IActionResult GetUnAuthorizeRM(string loanType, string branchCode, string rmCode)
        {
            var unauthorizeRm = rmAssetManager.GetUnAuthorizeRM(loanType, branchCode, rmCode,Session);
            return PartialView("_UnAuthorizeRM", unauthorizeRm);
        }
        [HttpPost]
        public IActionResult SetAuthorizeRM(string loanList)
        {
            var loans = loanList.Split(',').ToList();
            var message = rmAssetManager.SetAuthorizeRM(loans, Session);
            return Json(message);
        }
    }
}