using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Models.CommonModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyAssetManager.Controllers
{
    public class ClearUserSessionController : BaseController
    {
        private readonly ISettingsUsersService settingsUsers;
        private IHostingEnvironment environment;
        private readonly ICommonManager commonManager;
        private readonly IHttpContextAccessor contextAccessor;
        public ClearUserSessionController(ISettingsUsersService settingsUsers, IHostingEnvironment environment, ICommonManager commonManager, IHttpContextAccessor contextAccessor)
        {
            this.settingsUsers = settingsUsers;
            this.environment = environment;
            this.commonManager = commonManager;
            this.contextAccessor = contextAccessor;
        }
        [ScreenPermission("10041")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetLoginUserList() 
        {
            var data = settingsUsers.LoogedinUsers(Session);
            return PartialView("_LoginUsers", data);
        }
        [HttpPost]
        public IActionResult ClearSession(string user_id, string station_ip) 
        {
            var data = settingsUsers.ClearLoginSession(user_id, station_ip, Session);
            return Json(data);
        }
    }
}
