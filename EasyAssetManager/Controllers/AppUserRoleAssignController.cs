using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Models.CommonModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyAssetManager.Controllers
{
    public class AppUserRoleAssignController : BaseController
    {
        private readonly ISettingsUsersService settingsUsersService;
        private readonly ISettingsUsersService userService;
        private IHostingEnvironment environment;
        private readonly ICommonManager commonManager;
        private readonly IHttpContextAccessor contextAccessor;

        public AppUserRoleAssignController(ISettingsUsersService settingsUsersService, ISettingsUsersService userService, IHostingEnvironment environment, ICommonManager commonManager, IHttpContextAccessor contextAccessor)
        {
            this.settingsUsersService = settingsUsersService;
            this.userService = userService;
            this.environment = environment;
            this.commonManager = commonManager;
            this.contextAccessor = contextAccessor;
        }

        [ScreenPermission("10017")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult FreeRoleList(string user_id)
        {
            var freeRole = settingsUsersService.GetFreeRole(user_id, Session);
            return PartialView("_FreeRoleList", freeRole);
        }
        public IActionResult AssignedRoleList(string user_id)
        {
            var assignRole = settingsUsersService.GetAssignedRole(user_id, Session);
            return PartialView("_AssignedRoleList", assignRole);
        }
        [HttpPost]
        public IActionResult AddRemoveRole(string user_id, string role_id, string trans_type)
        {
            var setUserRole = settingsUsersService.SetUserRole(user_id, role_id, trans_type, Session);
            return Json(setUserRole);
        }
    }
}