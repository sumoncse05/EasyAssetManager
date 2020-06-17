using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EasyAssetManager.Controllers
{
    public class BaseController : Controller
    {

        public AppSession Session { get; set; }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool isAjaxRequest = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            Session = HttpContext.Session.Get<AppSession>(ApplicationConstant.GlobalSessionSession);

            if (Session == null)
            {
                // Fake implementation of session for development
                // FakeImplementationOfSession();

                //if (isAjaxRequest)
                //{
                //    context.Result = StatusCode(401);
                //}
                //else
                //{
                string redirectTo = string.Format("/Login/Index"); // Redirect to login page
                context.Result = new RedirectResult(redirectTo);
                //}
            }


            //if (!isAjaxRequest && Session.User.NeedToChangePassword)
            //{
            //    var actionName = context.RouteData.Values["Action"].ToString().ToLower();
            //    var controllerName = context.RouteData.Values["Controller"].ToString().ToLower();
            //    if (!(actionName.Equals("index") && controllerName.Equals("home")))
            //    {
            //        context.Result = new RedirectResult("/Home");
            //    }
            //}

            ViewBag.Session = Session;

            base.OnActionExecuting(context);
        }
        private void FakeImplementationOfSession()
        {
            var appSession = new AppSession
            {
                User = new SettingsUsers
                {
                    user_id = "ishtiaque01",
                    branch_name = "Test User",
                    roleid = "1",
                    user_name = "Sheikh Istiaque Ahmed",
                    address = "Rajshahi Branch",
                    agent_name = "PRAN AGRO LIMITED",
                    outlet_name = "AKCM HOSPITAL",
                },
                TransactionSession = new EasyAssetManagerCore.Models.CommonModel.TransactionSession
                {
                    UbsTransactionRefNo = "RDF975956",
                    TransactionType = "05",
                    TransactionID = "SDG86584375",
                    CustomerName = "SDG86584375",
                    BillRef = "SDG86584375",
                    TransactionAmount = "500",
                    TransactionVatAmount = "45",
                    TransactionDate = "20-04-2020",
                    TransactionCharge = "0",
                    TransactionAccountDesc = "SDG86584375"
                }
            };
            HttpContext.Session.Set(ApplicationConstant.GlobalSessionSession, appSession);
            Session = HttpContext.Session.Get<AppSession>(ApplicationConstant.GlobalSessionSession);
        }


    }
}