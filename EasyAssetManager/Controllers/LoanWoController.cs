using EasyAssetManagerCore.BusinessLogic.Operation.Asset;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EasyAssetManager.Controllers
{
    public class LoanWoController : BaseController
    {
        private IFileProcessManager fileProcessManager;
        private IHostingEnvironment environment;
        private readonly IHttpContextAccessor contextAccessor;
        public LoanWoController(IHostingEnvironment environment, IHttpContextAccessor contextAccessor, IFileProcessManager fileProcessManager)
        {
            this.environment = environment;
            this.contextAccessor = contextAccessor;
            this.fileProcessManager = fileProcessManager;
        }
        public IActionResult Index(string loanNumber = null)
        {
            var loanwo = new AST_LOAN_WO_STATUS_TEMP();
            if (loanNumber != null )
                loanwo = fileProcessManager.Getloanwo(loanNumber, "", "","", Session.User.user_id).FirstOrDefault();
            ViewBag.LoanWo = loanwo;
            return View();
        }
        public IActionResult WoSearch()
        {
            return View();
        }
        public IActionResult GetWoLoan(string loanNumber="",string area_code="",string branch_code="",string wo_date="")
        {
            var loans = fileProcessManager.Getloanwo(loanNumber,area_code, branch_code, wo_date, Session.User.user_id);
            return PartialView("_GetWoLoan", loans);
        }
        [HttpPost]
        public IActionResult SubmitLoanWo(AST_LOAN_WO_STATUS_TEMP wo)
        {
            var message = fileProcessManager.Set_LOAN_WO(wo, Session);
            return Json(message);
        }


    }
}