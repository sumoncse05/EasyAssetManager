using EasyAssetManagerCore.BusinessLogic.Operation;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EasyAssetManager.Controllers
{
    public class RemittanceCompanyController : BaseController
    {
        private readonly IRemmittanceManager remmittanceManager;
        private IHostingEnvironment environment;
        private readonly ICommonManager commonManager;
        private readonly IHttpContextAccessor contextAccessor;
        public RemittanceCompanyController(IRemmittanceManager remmittanceManager, IHostingEnvironment environment, ICommonManager commonManager, IHttpContextAccessor contextAccessor)
        {
            this.remmittanceManager = remmittanceManager;
            this.environment = environment;
            this.commonManager = commonManager;
            this.contextAccessor = contextAccessor;
        }
        [ScreenPermission("50145")]
        public IActionResult Index(string remit_id = null)
        {
            Remittance request = new Remittance();
            if(remit_id!=null) 
                request = remmittanceManager.GetRemitCompanyDetails(remit_id, Session).FirstOrDefault();
            ViewBag.RemitCompany = request;
            return View();
        }
        //public IActionResult GetRemitCompanyDetails(string remit_id)
        //{
        //    Remittance request = new Remittance();
        //    request = remmittanceManager.GetRemitCompanyDetails(remit_id, Session).FirstOrDefault();
        //    ViewBag.RemitCompany = request;
        //    return View("Index");
        //}
        [HttpPost]
        public IActionResult SetRemitCompanyDetails(Remittance remittance)
        {
            var request = remmittanceManager.SetRemitCompanyDetails(remittance, Session);
            return Json(request);
        }

        [HttpPost]
        public IActionResult DelRemitCompanyDetails(string remit_id)
        {
            var request = remmittanceManager.DelRemitCompanyDetails(remit_id, Session);
            return Json(request);
        }
        [ScreenPermission("50147")]
        public IActionResult RemitCompanyAuthorizeList()
        {
            return View();
        }
        public IActionResult GetUnauthRemitCompanyDetails()
        {
            var request = remmittanceManager.GetUnauthoRemitCompanyDetails(Session);
            return PartialView("_UnauthRemitCompanyList", request);
        }
        [HttpPost]
        public IActionResult SetRemitCompanyAuthorized(string remitList)
        {
            var remits = remitList.Split(',').ToList();
            var request = remmittanceManager.SetRemitCompanyAuthorized(remits, Session);
            return Json(request);
        }
       // [ScreenPermission("51027")]
        public IActionResult SearchRemitCompany()
        {
            return View();
        }
        public IActionResult GetRemitCompanyDetailsSearch(string sc_remit_id=null, string sc_remit_name=null)
        {
            var request = remmittanceManager.GetRemitCompanyDetails(sc_remit_id, sc_remit_name, Session);
            return PartialView("_RemitCompanyList", request);
        }
    }
}
