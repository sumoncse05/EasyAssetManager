using EasyAssetManagerCore.BusinessLogic.Operation.Asset;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EasyAssetManager.Controllers
{
    public class RMController : BaseController
    {
        private readonly IRMAssetManager rmAssetManager;
        private IHostingEnvironment environment;
        public RMController(IRMAssetManager rmAssetManager, IHostingEnvironment environment)
        {
            this.rmAssetManager = rmAssetManager;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SaveRM(RM rm)
        {
            var message = rmAssetManager.SetRM(rm, Session);
            return Json(message);
        }
        public IActionResult SearchRMDetails()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UpdateRMStatus(RM rm)
        {
            var message = rmAssetManager.UpdateRMStatus(rm, Session);
            return Json(message);
        }
        public IActionResult GetRMDetails(string emp_id)
        {
            var message = rmAssetManager.GetRMDetails(emp_id, Session);
            return Json(message);
        }

        public IActionResult GetRMDetail(string emp_id)
        {
            var message = rmAssetManager.GetRMDetails(emp_id, Session);
            ViewBag.RMDetails = message;
            return View("Index");
        }
        public IActionResult GetRMDetailList(string emp_id,string rm_name)
        {
            var data = rmAssetManager.GetRMDetailList(emp_id, rm_name, Session);
            return PartialView("_RMList", data); 
        }
    }
}
