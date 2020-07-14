using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EasyAssetManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.BusinessLogic.Operation.Asset;

namespace EasyAssetManager.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IRMAssetManager rmAssetManager;
        private readonly ISettingsUsersService settingsUsersService;
        private IHostingEnvironment environment;
        private readonly IHttpContextAccessor contextAccessor;
        public HomeController(ISettingsUsersService settingsUsersService, IRMAssetManager rmAssetManager, IHostingEnvironment environment, IHttpContextAccessor contextAccessor)
        {
            this.settingsUsersService = settingsUsersService;
            this.rmAssetManager = rmAssetManager;
            this.environment = environment;
            this.contextAccessor = contextAccessor;
        }
        public IActionResult Index()
        {
            var data = rmAssetManager.GetDashBoardDetails(Session);
            ViewBag.DashbordData = data;
            return View(); 
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string errorCode)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove(ApplicationConstant.GlobalSessionSession);
            return RedirectToAction("Index", "Login");
        }
        [HttpGet]
        public IActionResult GetHalfYearlyDashBoard()
        {
            var halfYearlyData = rmAssetManager.GetHalfYearlyDashBoard(Session);
            return Json(halfYearlyData);
        }

    }
}
