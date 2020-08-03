using EasyAssetManagerCore.BusinessLogic.Operation.Asset;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace EasyAssetManager.Controllers
{
    public class ReportController : BaseController
    {
        private readonly IRMAssetManager rmAssetManager;
        private IReportManager reportRepository;
        private IHostingEnvironment environment;
        public ReportController(IReportManager reportRepository, IRMAssetManager rmAssetManager, IHostingEnvironment environment)
        {
            this.rmAssetManager = rmAssetManager;
            this.reportRepository = reportRepository;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AssetAtGlance(string loanType, string rmCode, string areaCode, string branchCode, string todate)
        {
            if (string.IsNullOrEmpty(rmCode)) rmCode = ""; if (string.IsNullOrEmpty(areaCode)) areaCode = ""; if (string.IsNullOrEmpty(branchCode)) branchCode = "";
            var data = reportRepository.AssetAtGlance(loanType, rmCode, areaCode, branchCode, todate, Session);
            return PartialView("_AssetAtGlance", data);
        }
        public IActionResult AreawiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate)
        {
            if (string.IsNullOrEmpty(rmCode)) rmCode = ""; if (string.IsNullOrEmpty(areaCode)) areaCode = ""; if (string.IsNullOrEmpty(branchCode)) branchCode = "";
            var data = reportRepository.AreawiseReport(loanType, rmCode, areaCode, branchCode, todate, Session);
            return PartialView("_AreawiseReport", data);
        }
        public IActionResult BranchwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate)
        {
            if (string.IsNullOrEmpty(rmCode)) rmCode = ""; if (string.IsNullOrEmpty(areaCode)) areaCode = ""; if (string.IsNullOrEmpty(branchCode)) branchCode = "";
            var data = reportRepository.BranchwiseReport(loanType, rmCode, areaCode, branchCode, todate, Session);
            return PartialView("_BranchwiseReport", data);
        }
        public IActionResult RmwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate)
        {
            if (string.IsNullOrEmpty(rmCode)) rmCode = ""; if (string.IsNullOrEmpty(areaCode)) areaCode = ""; if (string.IsNullOrEmpty(branchCode)) branchCode = "";
            var data = reportRepository.RmwiseReport(loanType, rmCode, areaCode, branchCode, todate, Session);
            return PartialView("_RmwiseReport", data);
        }
        public IActionResult BstwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate)
        {
            if (string.IsNullOrEmpty(rmCode)) rmCode = ""; if (string.IsNullOrEmpty(areaCode)) areaCode = ""; if (string.IsNullOrEmpty(branchCode)) branchCode = "";
            var data = reportRepository.BstwiseReport(loanType, rmCode, areaCode, branchCode, todate, Session);
            return PartialView("_BstwiseReport", data);
        }
        public IActionResult ProductwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate)
        {
            if (string.IsNullOrEmpty(rmCode)) rmCode = ""; if (string.IsNullOrEmpty(areaCode)) areaCode = ""; if (string.IsNullOrEmpty(branchCode)) branchCode = "";
            var data = reportRepository.ProductwiseReport(loanType, rmCode, areaCode, branchCode, todate, Session);
            return PartialView("_ProductwiseReport", data);
        }
        public IActionResult YearwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate)
        {
            if (string.IsNullOrEmpty(rmCode)) rmCode = ""; if (string.IsNullOrEmpty(areaCode)) areaCode = ""; if (string.IsNullOrEmpty(branchCode)) branchCode = "";
            var data = reportRepository.YearwiseReport(loanType, rmCode, areaCode, branchCode, todate, Session);
            return PartialView("_YearwiseReport", data);
        }
        public IActionResult ClientwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate)
        {
            if (string.IsNullOrEmpty(rmCode)) rmCode = ""; if (string.IsNullOrEmpty(areaCode)) areaCode = ""; if (string.IsNullOrEmpty(branchCode)) branchCode = "";
            var data = reportRepository.ClientwiseReport(loanType, rmCode, areaCode, branchCode, todate, Session);
            return PartialView("_ClientwiseReport", data);
        }

        public IActionResult GraphReport()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetYearlyReportData(string area_code, string branch_code, string rm_code, string loantype)
        {
            var halfYearlyData = rmAssetManager.GetYearlyReportData(area_code, branch_code, rm_code, loantype, Session);
            return Json(halfYearlyData);
        }
    }
}
