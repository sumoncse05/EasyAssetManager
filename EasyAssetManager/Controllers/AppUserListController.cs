using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Models.CommonModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyAssetManager.Controllers
{
    public class AppUserListController : BaseController
    {
        private readonly ISettingsUsersService settingsUsers;
        private IHostingEnvironment environment;
        private readonly ICommonManager commonManager;
        private readonly IHttpContextAccessor contextAccessor;
        public AppUserListController(ISettingsUsersService settingsUsers, IHostingEnvironment environment, ICommonManager commonManager, IHttpContextAccessor contextAccessor)
        {
            this.settingsUsers = settingsUsers;
            this.environment = environment;
            this.commonManager = commonManager;
            this.contextAccessor = contextAccessor;
        }
        [ScreenPermission("11011")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetUserList(string sc_user_type,string sc_user_idno,string sc_user_name)
        {
            var data = settingsUsers.GetUserList(sc_user_type, sc_user_idno, sc_user_name, Session);
            return PartialView("_UserList", data);
        }
        [HttpPost]
        public IActionResult UpdateUserStatus(string user_id, string user_status, string update_reason)
        {
            var request = settingsUsers.SetUserInactive(user_id, user_status, update_reason, Session);
            return Json(request);
        }
        [HttpPost]
        public IActionResult DeleteUser(string user_id)
        {
            var request = settingsUsers.DeleteUser(user_id, Session);
            return Json(request);
        }
        [HttpPost]
        public IActionResult EmailUserDetails(string user_id)
        {
            var request = settingsUsers.EmailUserDetails(user_id, Session);
            return Json(request);
        }
    }
}
