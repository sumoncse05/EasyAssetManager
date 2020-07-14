using EasyAssetManagerCore.BusinessLogic.Operation.Asset;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EasyAssetManager.Controllers
{
    public class LoanClController : BaseController
    {
        private IFileProcessManager fileProcessManager;
        private IHostingEnvironment environment;
        private readonly IHttpContextAccessor contextAccessor;
        public LoanClController(IHostingEnvironment environment, IHttpContextAccessor contextAccessor, IFileProcessManager fileProcessManager)
        {
            this.environment = environment;
            this.contextAccessor = contextAccessor;
            this.fileProcessManager = fileProcessManager;
        }
        public IActionResult Index(string loan_number = null)
        {
            var loancl = new AST_LOAN_CL_TMP();
            if (loan_number != null)
                loancl = fileProcessManager.Getloancl(loan_number, "", "", Session).FirstOrDefault();
            ViewBag.LoanCl = loancl;
            return View();
        }
        public IActionResult ClSearch()
        {
            return View();
        }
        public IActionResult GetClLoan(string loan_number, string cl_status, string eff_date)
        {
            var loans = fileProcessManager.Getloancl(loan_number, cl_status, eff_date, Session);
            return PartialView("_GetClLoan", loans);
        }
        [HttpPost]
        public IActionResult SubmitLoanCl(AST_LOAN_CL_TMP wo)
        {
            var message = fileProcessManager.Set_LOAN_CL(wo, Session);
            return Json(message);
        }
    }
}