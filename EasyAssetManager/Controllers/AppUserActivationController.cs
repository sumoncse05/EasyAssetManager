using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyAssetManager.Controllers
{
    public class AppUserActivationController : BaseController
    {
        private readonly ISettingsUsersService settingsUsers;
        private IHostingEnvironment environment;
        private readonly ICommonManager commonManager;
        private readonly IHttpContextAccessor contextAccessor;
        public AppUserActivationController(ISettingsUsersService settingsUsers, IHostingEnvironment environment, ICommonManager commonManager, IHttpContextAccessor contextAccessor)
        {
            this.settingsUsers = settingsUsers;
            this.environment = environment;
            this.commonManager = commonManager;
            this.contextAccessor = contextAccessor;
        }
        [ScreenPermission("10039")]
        public IActionResult Index() 
        {
            return View();
        }
        public IActionResult GetLockedUserList() 
        {
            var data = settingsUsers.GetInactiveUser(Session);
            return PartialView("_LockedUserList",data);
        }
        [HttpPost]
        public IActionResult UpdateUserStatus(User user)
        {
            var request = settingsUsers.UpdateUserStatus(user.ACTV_SLNO, user.USER_ID, user.INACTIVE_REASON, Session);
            return Json(request);
        }
    }
}