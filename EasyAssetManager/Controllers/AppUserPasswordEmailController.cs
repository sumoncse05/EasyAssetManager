using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Models.CommonModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyAssetManager.Controllers
{
    public class AppUserPasswordEmailController : BaseController
    {
        private readonly ISettingsUsersService settingsUsers;
        private IHostingEnvironment environment;
        private readonly ICommonManager commonManager;
        private readonly IHttpContextAccessor contextAccessor;
        public AppUserPasswordEmailController(ISettingsUsersService settingsUsers, IHostingEnvironment environment, ICommonManager commonManager, IHttpContextAccessor contextAccessor)
        {
            this.settingsUsers = settingsUsers;
            this.environment = environment;
            this.commonManager = commonManager;
            this.contextAccessor = contextAccessor;
        }
        [ScreenPermission("10025")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetUserPassworEmailList()
        {
            var request = settingsUsers.GetUserPassworEmailList(Session);
            return PartialView("_UserPasswordEmailList", request);
        }
        [HttpPost]
        public IActionResult EmailResetPassword(string rst_slno, string user_id)
        {
            var request = settingsUsers.EmailResetPassword(rst_slno, user_id, Session);
            return Json(request);
        }
    }
}