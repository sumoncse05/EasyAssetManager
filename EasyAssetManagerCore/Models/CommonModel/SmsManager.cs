using EasyAssetManagerCore.Model.CommonModel;
using SmsService;

namespace EasyAssetManagerCore.Models.CommonModel
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
  //  [System.Web.Services.WebServiceBindingAttribute(Name = "SmsManagerSoap", XNamespace = "http://tempuri.org/")]
    public partial class SmsManager //: System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        //  private System.Threading.SendOrPostCallback SendSmsOperationCompleted;

        //  private System.Threading.SendOrPostCallback SendBulkSmsOperationCompleted;

        //  private System.Threading.SendOrPostCallback GetSmsStatusOperationCompleted;

        //  /// <remarks/>
        //  public SmsManager()
        //  {
        //      this.Url = "http://192.168.5.77/EasySMS/webservices/SmsManager.asmx";
        //      ConfigurationReader configReader = new ConfigurationReader("");
        //      if (configReader.Value("AppSettings", "AppMode") == "Live")
        //      {
        //          this.Url = configReader.Value("WebServicesLive", "SmsManager");
        //      }
        //      else
        //      {
        //          this.Url = configReader.Value("WebServicesTest", "SmsManager");
        //      }
        //  }

        //  /// <remarks/>
        //  public event SendSmsCompletedEventHandler SendSmsCompleted;

        //  /// <remarks/>
        //  public event SendBulkSmsCompletedEventHandler SendBulkSmsCompleted;

        //  /// <remarks/>
        //  public event GetSmsStatusCompletedEventHandler GetSmsStatusCompleted;

        //  /// <remarks/>
        ////  [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SendSms", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string[] SendSms(string sms_tmpl_id, string mobile_no, string sms)
        {
            if (!ApplicationConstant.ApplicationMode)
            {
                mobile_no = ApplicationConstant.MobileNumber;
            }
            SmsManagerSoapClient client = new SmsManagerSoapClient(SmsManagerSoapClient.EndpointConfiguration.SmsManagerSoap);
            var result =client.SendSmsAsync(ApplicationConstant.SmsUserId, ApplicationConstant.SmsPassword, sms_tmpl_id,mobile_no,sms);
            return result.Result.Body.SendSmsResult.ToArray();
        }

        //  /// <remarks/>
        //  public System.IAsyncResult BeginSendSms(string user_id, string password, string sms_tmpl_id, string mobile_no, string sms, System.AsyncCallback callback, object asyncState)
        //  {
        //      return this.BeginInvoke("SendSms", new object[] {
        //              user_id,
        //              password,
        //              sms_tmpl_id,
        //              mobile_no,
        //              sms}, callback, asyncState);
        //  }

        //  /// <remarks/>
        //  public string[] EndSendSms(System.IAsyncResult asyncResult)
        //  {
        //      object[] results = this.EndInvoke(asyncResult);
        //      return ((string[])(results[0]));
        //  }

        //  /// <remarks/>
        //  public void SendSmsAsync(string user_id, string password, string sms_tmpl_id, string mobile_no, string sms)
        //  {
        //      this.SendSmsAsync(user_id, password, sms_tmpl_id, mobile_no, sms, null);
        //  }

        //  /// <remarks/>
        //  public void SendSmsAsync(string user_id, string password, string sms_tmpl_id, string mobile_no, string sms, object userState)
        //  {
        //      if ((this.SendSmsOperationCompleted == null))
        //      {
        //          this.SendSmsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendSmsOperationCompleted);
        //      }
        //      this.InvokeAsync("SendSms", new object[] {
        //              user_id,
        //              password,
        //              sms_tmpl_id,
        //              mobile_no,
        //              sms}, this.SendSmsOperationCompleted, userState);
        //  }

        //  private void OnSendSmsOperationCompleted(object arg)
        //  {
        //      if ((this.SendSmsCompleted != null))
        //      {
        //          System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
        //          this.SendSmsCompleted(this, new SendSmsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        //      }
        //  }

        //  /// <remarks/>
        //  [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SendBulkSms", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        //  public string[] SendBulkSms(string user_id, string password, string sms_tmpl_id, string mobile_no, string sms)
        //  {
        //      object[] results = this.Invoke("SendBulkSms", new object[] {
        //              user_id,
        //              password,
        //              sms_tmpl_id,
        //              mobile_no,
        //              sms});
        //      return ((string[])(results[0]));
        //  }

        //  /// <remarks/>
        //  public System.IAsyncResult BeginSendBulkSms(string user_id, string password, string sms_tmpl_id, string mobile_no, string sms, System.AsyncCallback callback, object asyncState)
        //  {
        //      return this.BeginInvoke("SendBulkSms", new object[] {
        //              user_id,
        //              password,
        //              sms_tmpl_id,
        //              mobile_no,
        //              sms}, callback, asyncState);
        //  }

        //  /// <remarks/>
        //  public string[] EndSendBulkSms(System.IAsyncResult asyncResult)
        //  {
        //      object[] results = this.EndInvoke(asyncResult);
        //      return ((string[])(results[0]));
        //  }

        //  /// <remarks/>
        //  public void SendBulkSmsAsync(string user_id, string password, string sms_tmpl_id, string mobile_no, string sms)
        //  {
        //      this.SendBulkSmsAsync(user_id, password, sms_tmpl_id, mobile_no, sms, null);
        //  }

        //  /// <remarks/>
        //  public void SendBulkSmsAsync(string user_id, string password, string sms_tmpl_id, string mobile_no, string sms, object userState)
        //  {
        //      if ((this.SendBulkSmsOperationCompleted == null))
        //      {
        //          this.SendBulkSmsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendBulkSmsOperationCompleted);
        //      }
        //      this.InvokeAsync("SendBulkSms", new object[] {
        //              user_id,
        //              password,
        //              sms_tmpl_id,
        //              mobile_no,
        //              sms}, this.SendBulkSmsOperationCompleted, userState);
        //  }

        //  private void OnSendBulkSmsOperationCompleted(object arg)
        //  {
        //      if ((this.SendBulkSmsCompleted != null))
        //      {
        //          System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
        //          this.SendBulkSmsCompleted(this, new SendBulkSmsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        //      }
        //  }

        //  /// <remarks/>
        //  [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetSmsStatus", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        //  public string[] GetSmsStatus(string user_id, string password, string sms_type, string sms_id)
        //  {
        //      object[] results = this.Invoke("GetSmsStatus", new object[] {
        //              user_id,
        //              password,
        //              sms_type,
        //              sms_id});
        //      return ((string[])(results[0]));
        //  }

        //  /// <remarks/>
        //  public System.IAsyncResult BeginGetSmsStatus(string user_id, string password, string sms_type, string sms_id, System.AsyncCallback callback, object asyncState)
        //  {
        //      return this.BeginInvoke("GetSmsStatus", new object[] {
        //              user_id,
        //              password,
        //              sms_type,
        //              sms_id}, callback, asyncState);
        //  }

        //  /// <remarks/>
        //  public string[] EndGetSmsStatus(System.IAsyncResult asyncResult)
        //  {
        //      object[] results = this.EndInvoke(asyncResult);
        //      return ((string[])(results[0]));
        //  }

        //  /// <remarks/>
        //  public void GetSmsStatusAsync(string user_id, string password, string sms_type, string sms_id)
        //  {
        //      this.GetSmsStatusAsync(user_id, password, sms_type, sms_id, null);
        //  }

        //  /// <remarks/>
        //  public void GetSmsStatusAsync(string user_id, string password, string sms_type, string sms_id, object userState)
        //  {
        //      if ((this.GetSmsStatusOperationCompleted == null))
        //      {
        //          this.GetSmsStatusOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetSmsStatusOperationCompleted);
        //      }
        //      this.InvokeAsync("GetSmsStatus", new object[] {
        //              user_id,
        //              password,
        //              sms_type,
        //              sms_id}, this.GetSmsStatusOperationCompleted, userState);
        //  }

        //  private void OnGetSmsStatusOperationCompleted(object arg)
        //  {
        //      if ((this.GetSmsStatusCompleted != null))
        //      {
        //          System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
        //          this.GetSmsStatusCompleted(this, new GetSmsStatusCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        //      }
        //  }

        //  /// <remarks/>
        //  public new void CancelAsync(object userState)
        //  {
        //      base.CancelAsync(userState);
        //  }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void SendSmsCompletedEventHandler(object sender, SendSmsCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendSmsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SendSmsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string[] Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void SendBulkSmsCompletedEventHandler(object sender, SendBulkSmsCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendBulkSmsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SendBulkSmsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string[] Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void GetSmsStatusCompletedEventHandler(object sender, GetSmsStatusCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetSmsStatusCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetSmsStatusCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string[] Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }
}
