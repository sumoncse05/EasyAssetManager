using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace EasyAssetManager.Controllers
{
    public class UserRoleController : BaseController
    {
        private readonly ISettingsUsersService userService;
        private IHostingEnvironment environment;
        private readonly ICommonManager commonManager;
        public UserRoleController(ISettingsUsersService userService, IHostingEnvironment environment, ICommonManager commonManager)
        {
            this.userService = userService;
            this.environment = environment;
            this.commonManager = commonManager;
        }
        [ScreenPermission("10015")]
        public IActionResult Index()
        {
            var userRole = new UserRole();
            return View(userRole);
        }
        public IActionResult Rolelist()
        {
            var userRoles = userService.GetRoleList(Session.User.user_id);
            return PartialView("_RoleList", userRoles);
        }
        [HttpGet]
        public IActionResult UserScreenAccessPermission()
        {
            var userRoles = userService.UserScreenAccessPermission(Session.User.user_id);
            return Json(userRoles);
        }
        [HttpGet]
        public IActionResult RoleScreenAccessPermission(string roleId)
        {
            var userRoles = userService.RoleScreenAccessPermission(roleId,"",Session.User.user_id);
            return Json(userRoles);
        }
        [HttpPost]
        public IActionResult SaveRole(UserRole userRole)
        {
            var message = userService.SaveRole(userRole,Session);
            return Json(message);
        }

        [HttpPost]
        public IActionResult DeleteRole(string roleId)
        {
            var message = userService.DeleteRole(roleId, Session);
            return Json(message);
        }

    }
}