using EasyAssetManagerCore.BusinessLogic.Operation;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EasyAssetManager.Controllers
{
    public class SMEController : BaseController
    {
        private IHostingEnvironment environment;
        public SMEController(IHostingEnvironment environment)
        {
            this.environment = environment;
        }
        public IActionResult BranchWiseRetailReport()
        {
            return View();
        }
    }
}
