using EasyAssetManagerCore.BusinessLogic.Operation;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EasyAssetManager.Controllers
{
    public class AppUserSetupController : BaseController
    {
        private readonly IAppUserSetupManager appUserSetupManager;
        private readonly ISettingsUsersService userService;
        private IHostingEnvironment environment;
        private readonly ICommonManager commonManager;
        private readonly IHttpContextAccessor contextAccessor;

        public AppUserSetupController(IAppUserSetupManager appUserSetupManager, ISettingsUsersService userService, IHostingEnvironment environment, ICommonManager commonManager, IHttpContextAccessor contextAccessor)
        {
            this.appUserSetupManager = appUserSetupManager;
            this.userService = userService;
            this.environment = environment;
            this.commonManager = commonManager;
            this.contextAccessor = contextAccessor;
        }

        [ScreenPermission("10011")]
        public IActionResult Index(string user_id=null)
        {
            var userInfo = new User();
            if(user_id!=null)
                userInfo = userService.GetUserById(user_id, Session);
            ViewBag.UserInfo = userInfo;

            return View();
        }
        [HttpPost]
        public IActionResult CreateUser(User user) 
        {
            var message = appUserSetupManager.CreateUser(user, Session, contextAccessor);            
            return Json(message);
        }
        [HttpPost]
        public IActionResult CheckExistingUser(string user_id)
        {
            var message = appUserSetupManager.CheckExistingUserName(user_id, Session, contextAccessor);            
            return Json(message);
        }

        [ScreenPermission("10011")]
        public IActionResult AppUserPasswordReset()
        {
            return View();
        }

        public IActionResult GetActiveUsers(string user_id,string user_name,string user_type)
        {
            var activeUsers = userService.GetActiveUsers(user_id, user_name, user_type, Session.User.user_id);
            return PartialView("_ActiveUserList", activeUsers);
        }

        [HttpPost]
        public IActionResult ResetPassword(string userId, string reason)
        {
            var message = userService.ResetPassword(userId, reason, Session);
            return Json(message);
        }

         [ScreenPermission("10023")]
        public IActionResult AuthorizePasswordResetRequest()
        {
            return View();
        }

        public IActionResult AuthorizePasswordResetRequestList()
        {
            var authorizeRequests = userService.GetAuthorizePasswordResetRequest(Session.User.user_id);
            return PartialView("_AuthorizePasswordResetRequestList", authorizeRequests);
        }

        [HttpPost]
        public IActionResult AuthorizePasswordResetRequest(IEnumerable<SettingsUsers> users)
        {
            var message = userService.SaveAuthorizePasswordResetRequest(users, Session);
            return Json(message);
        }
        public IActionResult GetUserById(string user_id)
        {
            var userInfo = userService.GetUserById(user_id, Session);
            return View("Index",userInfo);
        }
    }
}