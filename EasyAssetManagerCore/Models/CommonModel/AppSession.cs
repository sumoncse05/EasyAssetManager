using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EasyAssetManagerCore.Model.CommonModel
{
    public class AppSession
    {
        public AppSession()
        {
            Screens = new List<Screen>();
            TransactionSession = new TransactionSession();
        }
        public virtual SettingsUsers User { get; set; }
        public virtual IEnumerable<Screen> Screens { get; set; }
        public virtual IEnumerable<ScreenAccessPermission> ScreenAccessPermissions { get; set; }
 
        public TransactionSession TransactionSession { get; set; }
    }

    public static class SessionExtensions
    {
        public static void Set(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }

    //public class SessionUpdate 
    //{
    //    private readonly IHttpContextAccessor _contextAccessor;

    //    public SessionUpdate(IHttpContextAccessor contextAccessor)
    //    {
    //        _contextAccessor = contextAccessor;
    //    }

    //    public T GetDataFromSession<T>(string key)
    //    {
    //        var value = _contextAccessor.HttpContext.Session.GetString(key);
    //        return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
    //    }

    //    public void SetDataToSession(string key, object value)
    //    {
    //        _contextAccessor.HttpContext.Session.SetString(key, JsonConvert.SerializeObject(value));
    //    }
    //}

    public class ApplicationConstant
    {
        public static string GlobalSuccessMessage = "Operation Successful.";
        public static string GlobalErrorMessage = "Operation UnSuccessful.";
        public static string GlobalInfoMessage = "Information.";
        public static string GlobalSessionSession = "ApplicationSession";
        public static string CustomerSession = "CustomerSession";
        public static string TransactionSession = "TransactionSession";
        public static string ConnectionString { get; set; }

        public static string EbankConnectionString { get; set; }
        public static string CardServiceUserId { get; set; }
        public static string CardServiceUserPassword { get; set; }

        public static string UbsService { get; set; }
        public static string SmsService { get; set; }
        public static string FcubService { get; set; }
        public static string CardService { get; set; }

        public static string SmsUserId { get; set; }
        public static string SmsPassword { get; set; }
        public static string MobileNumber { get; set; }

        public static bool ApplicationMode { get; set; }

    }
}
