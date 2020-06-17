using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Models.CommonModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyAssetManager.Controllers
{
    public class SecProcessStatusController : BaseController
    {
        private readonly ISettingsUsersService settingsUsers;
        private IHostingEnvironment environment;
        private readonly ICommonManager commonManager;
        private readonly IHttpContextAccessor contextAccessor;
        public SecProcessStatusController(ISettingsUsersService settingsUsers, IHostingEnvironment environment, ICommonManager commonManager, IHttpContextAccessor contextAccessor)
        {
            this.settingsUsers = settingsUsers;
            this.environment = environment;
            this.commonManager = commonManager;
            this.contextAccessor = contextAccessor;
        }
        [ScreenPermission("10051")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetProcessList(string run_id) 
        {
            var runnningProcess = settingsUsers.GetProcessRunStatus(run_id,Session);
            return PartialView("_RunningProcessStatus", runnningProcess);
        }
    }
}
