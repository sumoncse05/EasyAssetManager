using EasyAssetManagerCore.BusinessLogic.Operation.Asset;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace EasyAssetManager.Controllers
{
    public class ReportController : BaseController
    {
        private IReportManager reportRepository;
        private IHostingEnvironment environment;
        public ReportController(IReportManager reportRepository, IHostingEnvironment environment)
        {
            this.reportRepository = reportRepository;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AssetAtGlance(string loanType, string rmCode, string areaCode, string branchCode, string todate)
        {
            var data = reportRepository.AssetAtGlance(loanType, rmCode, areaCode, branchCode, todate, Session);
            return PartialView("_AssetAtGlance", data);
        }
        public IActionResult AreawiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate)
        {
            var data = reportRepository.AreawiseReport(loanType, rmCode, areaCode, branchCode, todate, Session);
            return PartialView("_AreawiseReport", data);
        }
        public IActionResult BranchwiseReport(string loanType,string rmCode,string areaCode,string branchCode,string todate)
        {
            var data = reportRepository.BranchwiseReport(loanType, rmCode, areaCode, branchCode, todate, Session);
            return PartialView("_BranchwiseReport", data);
        }
        public IActionResult RmwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate)
        {
            var data = reportRepository.RmwiseReport(loanType, rmCode, areaCode, branchCode, todate, Session);
            return PartialView("_RmwiseReport", data);
        }
        public IActionResult BstwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate)
        {
            var data = reportRepository.BstwiseReport(loanType, rmCode, areaCode, branchCode, todate, Session);
            return PartialView("_BstwiseReport", data);
        }
        public IActionResult ProductwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate)
        {
            var data = reportRepository.ProductwiseReport(loanType, rmCode, areaCode, branchCode, todate, Session);
            return PartialView("_ProductwiseReport", data);
        }
        public IActionResult YearwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate)
        {
            var data = reportRepository.YearwiseReport(loanType, rmCode, areaCode, branchCode, todate, Session);
            return PartialView("_YearwiseReport", data);
        }
        public IActionResult ClientwiseReport(string loanType, string rmCode, string areaCode, string branchCode, string todate)
        {
            var data = reportRepository.ClientwiseReport(loanType, rmCode, areaCode, branchCode, todate, Session);
            return PartialView("_ClientwiseReport", data);
        }

    }
}
