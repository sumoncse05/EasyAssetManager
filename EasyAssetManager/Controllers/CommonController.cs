using EasyAssetManagerCore.BusinessLogic.Operation;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace EasyAssetManager.Controllers
{
    public class CommonController : BaseController
    {
        private readonly IAccountOpeningManager accountOpeningManager;
        private readonly ISettingsUsersService settingsUsersService;
        private readonly IAccountManager accountManager;
        private readonly ICustomerManager customerManager;
        private readonly IAppUserSetupManager appUserSetupManager;
        private readonly IWithdrawRemittanceManager withdrawRemittanceManager;
        private readonly IBillPayCashManager billPayCashManager;
        //private readonly ISettingsUsersService settingsUsersService;
        private readonly ILimitManager limitManager;
        private readonly IAgentManager agentManager;
        public CommonController(IAccountOpeningManager accountOpeningManager, IAccountManager accountManager, ICustomerManager customerManager, IAppUserSetupManager appUserSetupManager, IWithdrawRemittanceManager withdrawRemittanceManager, IBillPayCashManager billPayCashManager, ISettingsUsersService settingsUsersService, ILimitManager limitManager, IAgentManager agentManager)
        {
            this.accountOpeningManager = accountOpeningManager;
            this.accountManager = accountManager;
            this.customerManager = customerManager;
            this.appUserSetupManager = appUserSetupManager;
            this.settingsUsersService = settingsUsersService;
            this.withdrawRemittanceManager = withdrawRemittanceManager;
            this.billPayCashManager = billPayCashManager;
            //this.settingsUsersService = settingsUsersService;
            this.limitManager = limitManager;
            this.agentManager = agentManager;
        }
        public IActionResult GetCustomers(string accountNo)
        {
            var customerAccounts = customerManager.GetCustomersByAccountNumber(accountNo, Session.User.user_id);
            return PartialView("_CustomerSearch", customerAccounts);
        }

        public IActionResult GetAccount(string accountNo)
        {
            var message = new Message();
            try
            {
                var accounts = accountManager.GeAccountDetails(accountNo, Session.User.user_id);

                if (accounts != null && accounts.Any())
                {
                    MessageHelper.Success(message, "Account Found");
                }
                else
                {
                    MessageHelper.Error(message, "Invalid account no. Please try again.");
                }
            }
            catch
            {
                MessageHelper.Error(message, "Invalid account no. Please try again.");
            }

            return Json(message);
        }

        [HttpGet]
        public IActionResult GetCustomerCombo()
        {
            var customers = customerManager.GetCustomersByAccountNumber(Session.TransactionSession.TransactionAccountNo, Session.User.user_id);
            Session.TransactionSession.Customers = customers;
            HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, Session);
            if (customers != null && customers.Any())
            {
                var selectList = customers.Select(x => new SelectListItem() { Text = x.customer_name, Value = x.customer_no });
                return Json(selectList);
            }
            return Json(null);
        }
        [HttpGet]
        public IActionResult GetWorkflowType()
        {
            var workFlow = accountOpeningManager.GetWorkflowType(Session.User.user_id);
            Session.TransactionSession.WorkFlowType = workFlow;
            HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, Session);
            if (workFlow != null && workFlow.Any())
            {
                var selectList = workFlow.Select(x => new SelectListItem() { Text = x.wf_desc, Value = x.wf_type });
                return Json(selectList);
            }
            return Json(null);
        }
        public IActionResult GetCustomerCombo(string accountNo)
        {
            var customers = customerManager.GetCustomersByAccountNumber(accountNo, Session.User.user_id);
            Session.TransactionSession.Customers = customers;
            HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, Session);
            if (customers != null && customers.Any())
            {
                var selectList = customers.Select(x => new SelectListItem() { Text = x.customer_name, Value = x.customer_no });
                return Json(selectList);
            }
            return Json(null);
        }

        public IActionResult GetRemittanceCompany(string remitenceCompanyId=null)
        {
            var remitenceCompanies = withdrawRemittanceManager.GetRemittanceCompany(remitenceCompanyId, Session.User.user_id);
            if (remitenceCompanies != null && remitenceCompanies.Any())
            {
                var selectList = remitenceCompanies.Select(x => new SelectListItem() { Text = x.remit_comp_name, Value = x.remit_comp_id });
                return Json(selectList);
            }
            return Json(null);
        }
        public IActionResult GetBillers(string pvc_billerid = null)
        {
            var billers = billPayCashManager.GetBillerDetail(pvc_billerid, Session.User.user_id);
            if (billers != null && billers.Any())
            {
                var selectList = billers.Select(x => new SelectListItem() { Text = x.biller_desc, Value = x.biller_id });
                return Json(selectList);
            }
            return Json(null);
        }

        public IActionResult PrintPalliBiddyutSlip()
        {
            return View();
        }
        public IActionResult PrintTransactionSlip()
        {
            return View();
        }
        public IActionResult GetCustomersByCIF(WorkflowDetail workflowDetail)
        {
            var customers = customerManager.GetCustomersByAccountNumber(workflowDetail.cust_ac_no , Session.User.user_id);
            var customer = customers.Where(x => x.customer_no == workflowDetail.customer_no).SingleOrDefault();
            return Json(customer);
        }

        [HttpGet]
        public IActionResult GetModuleListCombo()
        {
            var modules = appUserSetupManager.GetModiuleList( Session.User.user_id);
            if (modules != null && modules.Any())
            {
                var selectList = modules.Select(x => new SelectListItem() { Text = x.mod_desc, Value = x.mod_id });
                return Json(selectList);
            }
            return Json(null);
        }
        [HttpGet]
        public IActionResult GetUserTypeListCombo()
        {
            var userTypes = appUserSetupManager.GetUserTypeList(Session.User.user_id);
            if (userTypes != null && userTypes.Any())
            {
                var selectList = userTypes.Select(x => new SelectListItem() { Text = x.USER_DESC, Value = x.USER_TYPE });
                return Json(selectList);
            }
            return Json(null);
        }
        [HttpGet]
        public IActionResult GetBarnchListCombo()
        {
            var branches = appUserSetupManager.GetBranchList(Session.User.user_id);
            if (branches != null && branches.Any())
            {
                var selectList = branches.Select(x => new SelectListItem() { Text = x.BRANCH_NAME, Value = x.BRANCH_CODE });
                return Json(selectList);
            }
            return Json(null);
        }

        [HttpGet]
        public IActionResult GetUserBarnchListCombo()
        {
            var branches = accountOpeningManager.GetUserBranchList(Session.User.user_id);
            if (branches != null && branches.Any())
            {
                var selectList = branches.Select(x => new SelectListItem() { Text = x.BRANCH_NAME, Value = x.BRANCH_CODE });
                return Json(selectList);
            }
            return Json(null);
        }
        [HttpGet]
        public IActionResult GetDepartmentListCombo()
        {
            var departments = appUserSetupManager.GetDepartmentList(Session.User.user_id);
            if (departments != null && departments.Any())
            {
                var selectList = departments.Select(x => new SelectListItem() { Text = x.DEPT_NAME, Value = x.DEPT_CODE });
                return Json(selectList);
            }
            return Json(null);
        }
        [HttpGet]
        public IActionResult GetAgentListCombo()
        {
            var modules = appUserSetupManager.GetAgentList(Session.User.user_id);
            if (modules != null && modules.Any())
            {
                var selectList = modules.Select(x => new SelectListItem() { Text = x.AGENT_NAME, Value = x.AGENT_ID });
                return Json(selectList);
            }
            return Json(null);
        }
        [HttpGet]
        public IActionResult GetAgentOutletListCombo(string agent_id)
        {
            var customers = appUserSetupManager.GetAgentOutletList(agent_id, Session.User.user_id);
            if (customers != null && customers.Any())
            {
                var selectList = customers.Select(x => new SelectListItem() { Text = x.OUTLET_NAME, Value = x.OUTLET_ID });
                return Json(selectList);
            }
            return Json(null);
        }
        [HttpGet]
        public IActionResult GetUserListCombo()
        {
            var customers = settingsUsersService.CmdUserList(Session.User.user_id);
            if (customers != null && customers.Any())
            {
                var selectList = customers.Select(x => new SelectListItem() { Text = x.USER_ID, Value = x.USER_NAME });
                return Json(selectList);
            }
            return Json(null);
        }
        [HttpGet]
        public IActionResult GetLimitUserTypeList()
        {
            var customers = settingsUsersService.GetUserTypeList(Session.User.user_id);
            if (customers != null && customers.Any())
            {
                var selectList = customers.Select(x => new SelectListItem() { Text = x.USER_DESC, Value = x.USER_TYPE });
                return Json(selectList);
            }
            return Json(null);
        }
        [HttpGet]
        public IActionResult GetAgentAccountList(string agent_id)
        {
            var userTypes = agentManager.GetAgentAccountList(agent_id,Session.User.user_id);
            if (userTypes != null && userTypes.Any())
            {
                var selectList = userTypes.Select(x => new SelectListItem() { Text = x.ac_dtl, Value = x.cust_ac_no });
                return Json(selectList);
            }
            return Json(null);
        }

        [HttpGet]
        public IActionResult GetDivisionList()
        {
            var userTypes = accountManager.GetDivisionList(Session.User.user_id);
            if (userTypes != null && userTypes.Any())
            {
                var selectList = userTypes.Select(x => new SelectListItem() { Text = x.div_name, Value = x.div_code });
                return Json(selectList);
            }
            return Json(null);
        }
        [HttpGet]
        public IActionResult GetLimitPackageList()
        {
            var userTypes = limitManager.GetLimitPackageList(Session.User.user_id);
            if (userTypes != null && userTypes.Any())
            {
                var selectList = userTypes.Select(x => new SelectListItem() { Text = x.limit_desc, Value = x.limit_id });
                return Json(selectList);
            }
            return Json(null);
        }

        [HttpGet]
        public IActionResult GetDistrictList(string div_code)
        {
            var userTypes = accountManager.GetDistrictList(div_code, Session.User.user_id);
            if (userTypes != null && userTypes.Any())
            {
                var selectList = userTypes.Select(x => new SelectListItem() { Text = x.dist_name, Value = x.dist_code });
                return Json(selectList);
            }
            return Json(null);
        }

        [HttpGet]
        public IActionResult GetThanaList(string div_code,string dist_code)
        {
            var userTypes = accountManager.GetThanaList(div_code,dist_code, Session.User.user_id);
            if (userTypes != null && userTypes.Any())
            {
                var selectList = userTypes.Select(x => new SelectListItem() { Text = x.thana_name, Value = x.thana_code });
                return Json(selectList);
            }
            return Json(null);
        }

    }

}