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
        public IActionResult GetBranchList(string arearId="")
        {
            var userTypes = rMAssetManager.GetBranchList(arearId,Session);
            if (userTypes != null && userTypes.Any())
            {
                var selectList = userTypes.Select(x => new SelectListItem() { Text = x.BRANCH_NAME, Value = x.BRANCH_CODE });
                return Json(selectList);
            }
            return Json(null);
        }
        [HttpGet]
        public IActionResult GetAreaList()
        {
            var userTypes = rMAssetManager.GetAreaList(Session);
            if (userTypes != null && userTypes.Any())
            {
                var selectList = userTypes.Select(x => new SelectListItem() { Text = x.AREA_NAME, Value = x.AREA_CODE });
                return Json(selectList);
            }
            return Json(null);
        }

        [HttpGet]
        public IActionResult GetRmList(string branchCode="")
        {
            branchCode = string.IsNullOrEmpty(branchCode) ? Session.User.branch_code : branchCode;
            var userTypes = rMAssetManager.GetRmList(branchCode, Session);
            if (userTypes != null && userTypes.Any())
            {
                var selectList = userTypes.Select(x => new SelectListItem() { Text = x.emp_name, Value = x.rm_code });
                return Json(selectList);
            }
            return Json(null);
        }
        [HttpGet]
        public IActionResult GetLoanProductList(string loanType="")
        {
            var userTypes = rMAssetManager.GetLoanProductList(loanType, Session);
            if (userTypes != null && userTypes.Any())
            {
                var selectList = userTypes.Select(x => new SelectListItem() { Text = x.PRODUCT_DESC, Value = x.PRODUCT_CODE });
                return Json(selectList);
            }
            return Json(null);
        }

        [HttpGet]
        public IActionResult GetDesignationList()
        {
            var userTypes = rMAssetManager.GetDesignationList(Session);
            if (userTypes != null && userTypes.Any())
            {
                var selectList = userTypes.Select(x => new SelectListItem() { Text = x.desig_name, Value = x.desig_code });
                return Json(selectList);
            }
            return Json(null);
        }

        [HttpGet]
        public IActionResult GetGradeList()
        {
            var userTypes = rMAssetManager.GetGradeList(Session);
            if (userTypes != null && userTypes.Any())
            {
                var selectList = userTypes.Select(x => new SelectListItem() { Text = x.grade_name, Value = x.grade_code });
                return Json(selectList);
            }
            return Json(null);
        }
        [HttpGet]
        public IActionResult GetDepartmentList()
        {
            var userTypes = rMAssetManager.GetDepartmentList(Session);
            if (userTypes != null && userTypes.Any())
            {
                var selectList = userTypes.Select(x => new SelectListItem() { Text = x.dept_name, Value = x.dept_code });
                return Json(selectList);
            }
            return Json(null);
        }
        [HttpGet]
        public IActionResult GetCategoryList()
        {
            var userTypes = rMAssetManager.GetCategoryList(Session);
            if (userTypes != null && userTypes.Any())
            {
                var selectList = userTypes.Select(x => new SelectListItem() { Text = x.cat_desc, Value = x.cat_id });
                return Json(selectList);
            }
            return Json(null);
        }
        [HttpGet]
        public IActionResult GetRMStatusList()
        {
            var userTypes = rMAssetManager.GetRMStatusList(Session);
            if (userTypes != null && userTypes.Any())
            {
                var selectList = userTypes.Select(x => new SelectListItem() { Text = x.status_desc, Value = x.status_code });
                return Json(selectList);
            }
            return Json(null);
        }


    }

}