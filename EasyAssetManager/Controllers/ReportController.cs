using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace EasyAssetManager.Controllers
{
    public class ReportController : BaseController
    {
        private IHostingEnvironment environment;
        public ReportController(IHostingEnvironment environment)
        {
            this.environment = environment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AssetAtGlance(string loanType, string rmCode, string areaCode, string branchCode, string todate)
        {
            return PartialView("_AssetAtGlance", null);
        }
        public IActionResult AreawiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate)
        {
            return PartialView("_AreawiseReport", null);
        }
        public IActionResult BranchwiseReport(string loanType,string rmCode,string areaCode,string branchCode,string todate)
        {
            return PartialView("_BranchwiseReport", null);
        }
        public IActionResult RmwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate)
        {
            return PartialView("_RmwiseReport", null);
        }
        public IActionResult BstwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate)
        {
            return PartialView("_BstwiseReport", null);
        }
        public IActionResult ProductwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate)
        {
            return PartialView("_ProductwiseReport", null);
        }
        public IActionResult YearwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate)
        {
            return PartialView("_YearwiseReport", null);
        }
        public IActionResult ClientwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate)
        {
            return PartialView("_ClientwiseReport", null);
        }

    }
}
