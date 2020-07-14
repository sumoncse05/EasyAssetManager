using EasyAssetManagerCore.BusinessLogic.Operation.Asset;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace EasyAssetManager.Controllers
{
    public class LoanSearchController : BaseController
    {
        private readonly IRMAssetManager rmAssetManager;
        private IHostingEnvironment environment;
        public LoanSearchController(IRMAssetManager rmAssetManager, IHostingEnvironment environment)
        {
            this.rmAssetManager = rmAssetManager;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetLoanDetailList(string ac_area_code, string ac_branch_code, string ac_loan_no)
        {
            var data = rmAssetManager.GetLoanDetailList(ac_area_code, ac_branch_code, ac_loan_no, Session);
            return PartialView("_LoanList", data);
        }
        [HttpPost]
        public IActionResult UpdateMonitoringRM(string loan_ac_no, string moni_rm_code)
        {
            var message = rmAssetManager.UpdateMonitoringRM(loan_ac_no, moni_rm_code, Session);
            return Json(message);
        }
    }
}
