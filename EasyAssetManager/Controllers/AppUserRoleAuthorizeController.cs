using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Models.CommonModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EasyAssetManager.Controllers
{
    public class AppUserRoleAuthorizeController : BaseController
    {
        private readonly ISettingsUsersService settingsUsers;
        private IHostingEnvironment environment;
        private readonly ICommonManager commonManager;
        private readonly IHttpContextAccessor contextAccessor;
        public AppUserRoleAuthorizeController(ISettingsUsersService settingsUsers, IHostingEnvironment environment, ICommonManager commonManager, IHttpContextAccessor contextAccessor)
        {
            this.settingsUsers = settingsUsers;
            this.environment = environment;
            this.commonManager = commonManager;
            this.contextAccessor = contextAccessor;
        }
        [ScreenPermission("10018")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetUnAuthorizeUserRoleList()
        {
            var request = settingsUsers.GetUnAuthorizeUserRoleList(Session);
            return PartialView("_UnAuthorizeRole", request);
        }
        [HttpPost]
        public IActionResult AuthUserRoles(string listUser)
        {
            var Users = listUser.Split(',').ToList();
            var request = settingsUsers.AuthUsersRole(Users, Session);
            return Json(request);
        }
    }
}