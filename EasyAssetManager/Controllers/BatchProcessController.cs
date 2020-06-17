using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EasyAssetManager.Controllers
{
    public class BatchProcessController : BaseController
    {
        private IHostingEnvironment environment;
        private readonly ICommonManager commonManager;
        public BatchProcessController(IHostingEnvironment environment, ICommonManager commonManager)
        {
            this.environment = environment;
            this.commonManager = commonManager;
        }
        [ScreenPermission("50301")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RunProcessCommand(BatchProcess batchProcess)
        {
            batchProcess.batch_id = "HR_COMMGEN";
            batchProcess.process_id = "1001";
            var runId = "0";
            var message = commonManager.GetAccountStatus(batchProcess, Session,out runId);
            var data = new
            {
                message=message,
                runId=runId
            };
            return Json(data);
        }

        public IActionResult GetProcessStatus(string runId)
        {
            var batchProcesses = commonManager.GetProcessRunStatus(runId, Session.User.user_id);
            return PartialView("_AllProcessStatus", batchProcesses);
        }

        [HttpGet]
        public IActionResult GetErrorMessage(string runId)
        {
            var error = commonManager.GetProcessErrMsgCommand(runId).FirstOrDefault();
            return Json(error);
        }

    }
}