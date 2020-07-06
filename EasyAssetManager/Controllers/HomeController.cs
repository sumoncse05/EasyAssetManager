using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EasyAssetManager.Models;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.BusinessLogic.Operation;

namespace EasyAssetManager.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ISettingsUsersService settingsUsersService;
        private IHostingEnvironment environment;
        private readonly IHttpContextAccessor contextAccessor;
        public HomeController(ISettingsUsersService settingsUsersService, IHostingEnvironment environment, IHttpContextAccessor contextAccessor)
        {
            this.settingsUsersService = settingsUsersService;
            this.environment = environment;
            this.contextAccessor = contextAccessor;
        }
        public IActionResult Index()
        {
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
        [ScreenPermission("10019")]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(SettingsUsers user)
        {
            var msg = settingsUsersService.SetUserPassword(user, Session, contextAccessor);
            return Json(msg);
        }

        public IActionResult Profile()
        {
            return View();
        }



    }
}
