using EasyAssetManagerCore.BusinessLogic.Operation;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EasyAssetManager.Controllers
{
    public class LimitPackageController : BaseController
    {
        private readonly ILimitManager limitService;
        private IHostingEnvironment environment;
        private readonly ICommonManager commonManager;
        public LimitPackageController(ILimitManager limitService, IHostingEnvironment environment, ICommonManager commonManager)
        {
            this.limitService = limitService;
            this.environment = environment;
            this.commonManager = commonManager;
        }
        #region LimitPackageSetup
        [ScreenPermission("40121")]
        public IActionResult Index(string limitId = null)
        {
            var limit = new LimitPackage();
            if (limitId != null)
                limit = limitService.GetLimitDetails(limitId, Session.User.user_id);

            return View(limit);
        }

        [HttpPost]
        public IActionResult SaveLimitPackage(LimitPackage limitPackage)
        {
            var message = limitService.SetLimitPackage(limitPackage, Session);
            return Json(message);
        }

        #endregion LimitPackageSetup
        #region LimitPackageView
         [ScreenPermission("40121")]
        public IActionResult LimitPackageView(string limitId)
        {
            var limit = limitService.GetLimitDetails(limitId, Session.User.user_id);
            if (limit == null)
                limit = new LimitPackage();
            return View(limit);
        }
        #endregion LimitPackageView
        #region LimitPackageList
        [ScreenPermission("41001")]
        public IActionResult LimitPackageList()
        {
            return View();
        }
        public IActionResult LimitPackageListPartial()
        {
            var limits = limitService.GetLimitPackages("", Session.User.user_id);
            return PartialView("_LimitPackageListPartial", limits);
        }
        [HttpPost]
        public IActionResult DeletLimitPackage(string limitId)
        {
            var message = limitService.DeleteLimitPackage(limitId, Session);
            return Json(message);
        }

        #endregion LimitPackageList
        #region TransactionLimitAllocation
        [ScreenPermission("40131")]
        public IActionResult TransactionLimitAllocation()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SubmitTransactionLimitPackage(LimitPackage limitPackage)
        {
            var newLimitPackage = limitService.SubmitTransactionLimitPackage(limitPackage, Session);
            return Json(newLimitPackage);
        }
        [HttpPost]
        public IActionResult SaveTransactionLimitPackage(LimitPackage limitPackage)
        {
            var message = limitService.SetTransLimitPackage(limitPackage, Session);
            return Json(message);
        }
        #endregion TransactionLimitAllocation
        #region LimitPackageAuthorization
         [ScreenPermission("40123")]
        public IActionResult LimitPackageAuthorization()
        {
            return View();
        }
        public IActionResult LimitPackageAuthorizationList()
        {
            var unAuthLimitPackage = limitService.GetUnauthLimitPackages(Session.User.user_id);
            return PartialView("_LimitPackageAuthorizationList", unAuthLimitPackage);
        }
        [HttpPost]
        public IActionResult AuthorizationLimitPackage(List<string> limitIds)
        {
            var message = limitService.AuthorizeLimitPackage(limitIds, Session);
            return Json(message);
        }
        #endregion LimitPackageAuthorization
        #region TransactionLimitAuthorization
         [ScreenPermission("40133")]
        public IActionResult TransactionLimitAuthorization()
        {
            return View();
        }
        public IActionResult TransactionLimitAuthorizationList()
        {
            var unAuthTransactionLimit = limitService.GetUnauthTransLimit(Session.User.user_id);
            return PartialView("_TransactionLimitAuthorizationList", unAuthTransactionLimit);
        }
        [HttpPost]
        public IActionResult DeleteTransactionLimit(string limit_slno)
        {
            var message = limitService.DeleteTransactionLimit(limit_slno, Session);
            return Json(message);
        }
        [HttpPost]
        public IActionResult AuthorizationTransactionLimit(List<string> limitSlNos)
        {
            var message = limitService.AuthorizeTransLimit(limitSlNos, Session);
            return Json(message);
        }
        #endregion TransactionLimitAuthorization
    }
}