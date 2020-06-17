using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace EasyAssetManagerCore.Models.CommonModel
{
    public class ScreenPermission : ActionFilterAttribute
    {
        public string ScreenCode { get; private set; }
        public string IsScreenPermission { get; private set; }
        public bool CAN_UPDATE { get; set; }
        public bool CAN_DELETE { get; set; }
        public bool CAN_VIEW { get; set; }
        public ScreenPermission(string screenCode,string isScreenPermission="100")
        {
            ScreenCode = screenCode;
            IsScreenPermission = isScreenPermission;
            //CAN_VIEW = isScreenPermission.Substring(0, 1) == "1" ? true: false;
            //CAN_UPDATE = isScreenPermission.Substring(1, 1) == "1" ? true : false;
            //CAN_DELETE = isScreenPermission.Substring(2, 1) == "1" ? true : false;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var actionName = context.RouteData.Values["Action"].ToString().ToLower();
            var controllerName = context.RouteData.Values["Controller"].ToString().ToLower();
            var appSession = context.HttpContext.Session.Get<AppSession>(ApplicationConstant.GlobalSessionSession);

            if (appSession != null)
            {
                var screen = appSession.Screens.Where(o => o.scr_id==ScreenCode).FirstOrDefault();
                if (screen==null)
                {
                    context.Result = new RedirectResult("~/Error/{404}");
                }
                //else
                //{
                //    var permission = new ScreenAccessPermission();
                //    if (CAN_VIEW)
                //    {
                //        permission = appSession.ScreenAccessPermissions.Where(o => o.SCR_ID == ScreenCode && o.can_view == "Y").FirstOrDefault();
                //    }
                //    else if (CAN_UPDATE)
                //    {
                //        permission = appSession.ScreenAccessPermissions.Where(o => o.SCR_ID == ScreenCode && o.can_update == "Y").FirstOrDefault();
                //    }
                //    else if (CAN_DELETE)
                //    {
                //        permission = appSession.ScreenAccessPermissions.Where(o => o.SCR_ID == ScreenCode && o.can_delete == "Y").FirstOrDefault();
                //    }
                //    if (permission == null)
                //    {
                //        context.Result = new RedirectResult("~/Error/{404}");
                //    }
                //}
            }
            else
            {
                context.Result = new RedirectResult("~/");
            }


            base.OnActionExecuting(context);
        }
    }

    public class CheckSession : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var appSession = context.HttpContext.Session.Get<AppSession>(ApplicationConstant.GlobalSessionSession);

            if (appSession == null)
            {
                context.Result = new RedirectResult("~/");
            }
            base.OnActionExecuting(context);
        }
    }
}
