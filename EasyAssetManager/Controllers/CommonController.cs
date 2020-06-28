using EasyAssetManagerCore.BusinessLogic.Operation.Asset;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace EasyAssetManager.Controllers
{
    public class CommonController : BaseController
    {
        private readonly IRMAssetManager rMAssetManager;

        public CommonController(IRMAssetManager accountOpeningManager)
        {
            this.rMAssetManager = accountOpeningManager;
        }
       
       
        [HttpGet]
        public IActionResult GetBranchList()
        {
            var userTypes = rMAssetManager.GetBranchList(Session.User.user_id);
            if (userTypes != null && userTypes.Any())
            {
                var selectList = userTypes.Select(x => new SelectListItem() { Text = x.BRANCH_NAME, Value = x.BRANCH_CODE });
                return Json(selectList);
            }
            return Json(null);
        }

 
        [HttpGet]
        public IActionResult GetRmList()
        {
            var userTypes = rMAssetManager.GetRmList(Session.User.branch_code, Session.User.user_id);
            if (userTypes != null && userTypes.Any())
            {
                var selectList = userTypes.Select(x => new SelectListItem() { Text = x.EMP_NAME, Value = x.RM_CODE });
                return Json(selectList);
            }
            return Json(null);
        }
        [HttpGet]
        public IActionResult GetLoanProductList(string loanType="")
        {
            var userTypes = rMAssetManager.GetLoanProductList(loanType,Session.User.user_id);
            if (userTypes != null && userTypes.Any())
            {
                var selectList = userTypes.Select(x => new SelectListItem() { Text = x.PRODUCT_DESC, Value = x.PRODUCT_CODE });
                return Json(selectList);
            }
            return Json(null);
        }




    }

}