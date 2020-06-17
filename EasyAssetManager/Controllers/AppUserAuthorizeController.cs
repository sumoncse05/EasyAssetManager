using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Models.CommonModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EasyAssetManager.Controllers
{
    public class AppUserAuthorizeController : BaseController
    {
        private readonly ISettingsUsersService settingsUsers;
        private IHostingEnvironment environment;
        private readonly ICommonManager commonManager;
        private readonly IHttpContextAccessor contextAccessor;
        public AppUserAuthorizeController(ISettingsUsersService settingsUsers, IHostingEnvironment environment, ICommonManager commonManager, IHttpContextAccessor contextAccessor)
        {
            this.settingsUsers = settingsUsers;
            this.environment = environment;
            this.commonManager = commonManager;
            this.contextAccessor = contextAccessor;
        }
        [ScreenPermission("10013")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetUnAuthorizeUserList()
        {
            var request = settingsUsers.GetUnAuthorizeUserList(Session);
            return PartialView("_UnAuthorizeUser", request);
        }
        [HttpPost]
        public IActionResult AuthUsers(string listUser)
        {
            var Users = listUser.Split(',').ToList();
            var request = settingsUsers.AuthUsers(Users, Session);            
            return Json(request);
        }
    }
}