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
    public class BillerDetailsController : BaseController
    {
        private readonly IBillerManager billerManager;
        private IHostingEnvironment environment;
        private readonly ICommonManager commonManager;
        private readonly IHttpContextAccessor contextAccessor;
        public BillerDetailsController(IBillerManager billerManager, IHostingEnvironment environment, ICommonManager commonManager, IHttpContextAccessor contextAccessor)
        {
            this.billerManager = billerManager;
            this.environment = environment;
            this.commonManager = commonManager;
            this.contextAccessor = contextAccessor;
        }
        [ScreenPermission("50141")]
        public IActionResult Index(string biller_id)
        {
            Biller request = new Biller();
            if(biller_id!=null)
                request = billerManager.GetBillerDetails(biller_id, Session).FirstOrDefault();
            ViewBag.BillerDetail = request;
            return View();
        }
        //public IActionResult GetBillerDetails(string biller_id)
        //{
        //    Biller request = new Biller();
        //    request = billerManager.GetBillerDetails(biller_id,Session).FirstOrDefault();
        //    ViewBag.BillerDetail = request;
        //    return View("Index");
        //}
        [HttpPost]
        public IActionResult SetBillerDetails(Biller biller)
        {
            var request = billerManager.SetBillerDetails(biller, Session);
            return Json(request);
        }

        [HttpPost]
        public IActionResult DelBillerDetails(string biller_id)
        {
            var request = billerManager.DelBillerDetails(biller_id, Session);
            return Json(request);
        }
        //[HttpPost]
        //public IActionResult SetBillerAuthorized(string biller_id)
        //{
        //    var request = billerManager.SetBillerAuthorized(biller_id, Session);
        //    return Json(request);
        //}
        [ScreenPermission("50143")]
        public IActionResult BillerAuthorizeList()
        {
            return View();
        }
        public IActionResult GetUnauthoBillerDetails()
        {
            var request = billerManager.GetUnauthoBillerDetails( Session);
            return PartialView("_UnathorizeBillerList", request);
        }
        [HttpPost]
        public IActionResult SetBillerAuthorized(string billerList)
        {
            var billers = billerList.Split(',').ToList();
            var request = billerManager.SetBillerAuthorized(billers, Session);
            return Json(request);
        }
        [ScreenPermission("51025")]
        public IActionResult SearchBiller() 
        {
            return View();
        }
        public IActionResult GetBillerList(string biller_id,string biller_name)
        {
            var request = billerManager.GetBillerDetails(biller_id, biller_name, Session);
            return PartialView("_BillerList", request);
        }
    }
}
