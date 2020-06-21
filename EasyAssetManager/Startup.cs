using System;
using EasyAssetManagerCore.BusinessLogic.Operation;
using EasyAssetManagerCore.BusinessLogic.Operation.Asset;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EasyAssetManager
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                  .SetBasePath(env.ContentRootPath)
                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                  .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                  .AddEnvironmentVariables();
            Configuration = builder.Build();
            //var x = new Encription().Encrypt("ermp");
            ApplicationConstant.ApplicationMode = Convert.ToBoolean(Configuration["Data:ApplicationModeIsLive"]);
            string dataSource, userId, password;
            if (ApplicationConstant.ApplicationMode)
            {
                dataSource = Configuration["Data:LiveConnection:EbankConnectionString:DataSource"];
                userId = Configuration["Data:LiveConnection:EbankConnectionString:UserID"];
                password = Configuration["Data:LiveConnection:EbankConnectionString:Password"];
                ApplicationConstant.ConnectionString = string.Format("Data Source={0};Password={1};User ID={2};", dataSource, new Encription().Decrypt(password), new Encription().Decrypt(userId));
                ApplicationConstant.EbankConnectionString = string.Format("Data Source={0};Password={1};User ID={2};", dataSource, new Encription().Decrypt(password), new Encription().Decrypt(userId));

                dataSource = Configuration["Data:LiveConnection:EsecConnectionString:DataSource"];
                userId = Configuration["Data:LiveConnection:EsecConnectionString:UserID"];
                password = Configuration["Data:LiveConnection:EsecConnectionString:Password"];
                ApplicationConstant.EsecConnectionString = string.Format("Data Source={0};Password={1};User ID={2};", dataSource, new Encription().Decrypt(password), new Encription().Decrypt(userId));

                dataSource = Configuration["Data:LiveConnection:CbsConnectionString:DataSource"];
                userId = Configuration["Data:LiveConnection:CbsConnectionString:UserID"];
                password = Configuration["Data:LiveConnection:CbsConnectionString:Password"];
                ApplicationConstant.CbsConnectionString = string.Format("Data Source={0};Password={1};User ID={2};", dataSource, new Encription().Decrypt(password), new Encription().Decrypt(userId));

                dataSource = Configuration["Data:LiveConnection:RemitConnectionString:DataSource"];
                userId = Configuration["Data:LiveConnection:RemitConnectionString:UserID"];
                password = Configuration["Data:LiveConnection:RemitConnectionString:Password"];
                ApplicationConstant.RemitConnectionString = string.Format("Data Source={0};Password={1};User ID={2};", dataSource, new Encription().Decrypt(password), new Encription().Decrypt(userId));

                TransactionXmlConstant.SOURCE = Configuration["Data:LiveTransactionXmlConstant:SOURCE"];
                TransactionXmlConstant.UBSCOMP = Configuration["Data:LiveTransactionXmlConstant:UBSCOMP"];
                TransactionXmlConstant.USERID = Configuration["Data:LiveTransactionXmlConstant:USERID"];
                TransactionXmlConstant.SERVICE = Configuration["Data:LiveTransactionXmlConstant:SERVICE"];
                TransactionXmlConstant.OPERATION = Configuration["Data:LiveTransactionXmlConstant:OPERATION"];
                TransactionXmlConstant.BATCHNO = Configuration["Data:LiveTransactionXmlConstant:BATCHNO"];
                TransactionXmlConstant.CCY = Configuration["Data:LiveTransactionXmlConstant:CCY"];
                TransactionXmlConstant.AUTHSTAT = Configuration["Data:LiveTransactionXmlConstant:AUTHSTAT"];
                TransactionXmlConstant.SLNO = Configuration["Data:LiveTransactionXmlConstant:SLNO"];
                TransactionXmlConstant.BATCH_NO = Configuration["Data:LiveTransactionXmlConstant:BATCH_NO"];
                TransactionXmlConstant.DESCRIPTION = Configuration["Data:LiveTransactionXmlConstant:DESCRIPTION"];
                TransactionXmlConstant.BALANCING = Configuration["Data:LiveTransactionXmlConstant:BALANCING"];

                ApplicationConstant.CardServiceUserId = Configuration["Data:CardServiceLiveSettings:UserId"];
                ApplicationConstant.CardServiceUserPassword = new Encription().Decrypt(Configuration["Data:CardServiceLiveSettings:Password"]);

                ApplicationConstant.UbsService = Configuration["Data:LiveServiceUrl:UbsService"];
                ApplicationConstant.SmsService = Configuration["Data:LiveServiceUrl:SmsService"];
                ApplicationConstant.FcubService = Configuration["Data:LiveServiceUrl:FcubService"];
                ApplicationConstant.CardService = Configuration["Data:LiveServiceUrl:CardService"];

                ApplicationConstant.SmsUserId = Configuration["Data:LiveSmsSetting:SmsUserId"];
                ApplicationConstant.SmsPassword = new Encription().Decrypt(Configuration["Data:LiveSmsSetting:SmsPassword"]);
                ApplicationConstant.MobileNumber = Configuration["Data:LiveSmsSetting:MobileNumber"];
            }
            else
            {
                dataSource = Configuration["Data:TestConnection:EbankConnectionString:DataSource"];
                userId = Configuration["Data:TestConnection:EbankConnectionString:UserID"];
                password = Configuration["Data:TestConnection:EbankConnectionString:Password"];
                ApplicationConstant.ConnectionString = string.Format("Data Source={0};Password={1};User ID={2};", dataSource, new Encription().Decrypt(password), new Encription().Decrypt(userId));
                ApplicationConstant.EbankConnectionString = string.Format("Data Source={0};Password={1};User ID={2};", dataSource, new Encription().Decrypt(password), new Encription().Decrypt(userId));

                dataSource = Configuration["Data:TestConnection:EsecConnectionString:DataSource"];
                userId = Configuration["Data:TestConnection:EsecConnectionString:UserID"];
                password = Configuration["Data:TestConnection:EsecConnectionString:Password"];
                ApplicationConstant.EsecConnectionString = string.Format("Data Source={0};Password={1};User ID={2};", dataSource, new Encription().Decrypt(password), new Encription().Decrypt(userId));

                dataSource = Configuration["Data:TestConnection:CbsConnectionString:DataSource"];
                userId = Configuration["Data:TestConnection:CbsConnectionString:UserID"];
                password = Configuration["Data:TestConnection:CbsConnectionString:Password"];
                ApplicationConstant.CbsConnectionString = string.Format("Data Source={0};Password={1};User ID={2};", dataSource, new Encription().Decrypt(password), new Encription().Decrypt(userId));

                dataSource = Configuration["Data:TestConnection:RemitConnectionString:DataSource"];
                userId = Configuration["Data:TestConnection:RemitConnectionString:UserID"];
                password = Configuration["Data:TestConnection:RemitConnectionString:Password"];
                ApplicationConstant.RemitConnectionString = string.Format("Data Source={0};Password={1};User ID={2};", dataSource, new Encription().Decrypt(password), new Encription().Decrypt(userId));

                TransactionXmlConstant.SOURCE = Configuration["Data:TestTransactionXmlConstant:SOURCE"];
                TransactionXmlConstant.UBSCOMP = Configuration["Data:TestTransactionXmlConstant:UBSCOMP"];
                TransactionXmlConstant.USERID = Configuration["Data:TestTransactionXmlConstant:USERID"];
                TransactionXmlConstant.SERVICE = Configuration["Data:TestTransactionXmlConstant:SERVICE"];
                TransactionXmlConstant.OPERATION = Configuration["Data:TestTransactionXmlConstant:OPERATION"];
                TransactionXmlConstant.BATCHNO = Configuration["Data:TestTransactionXmlConstant:BATCHNO"];
                TransactionXmlConstant.CCY = Configuration["Data:TestTransactionXmlConstant:CCY"];
                TransactionXmlConstant.AUTHSTAT = Configuration["Data:TestTransactionXmlConstant:AUTHSTAT"];
                TransactionXmlConstant.SLNO = Configuration["Data:TestTransactionXmlConstant:SLNO"];
                TransactionXmlConstant.BATCH_NO = Configuration["Data:TestTransactionXmlConstant:BATCH_NO"];
                TransactionXmlConstant.DESCRIPTION = Configuration["Data:TestTransactionXmlConstant:DESCRIPTION"];
                TransactionXmlConstant.BALANCING = Configuration["Data:TestTransactionXmlConstant:BALANCING"];

                ApplicationConstant.CardServiceUserId = Configuration["Data:CardServiceTestSettings:UserId"];
                ApplicationConstant.CardServiceUserPassword = new Encription().Decrypt(Configuration["Data:CardServiceTestSettings:Password"]);

                ApplicationConstant.UbsService = Configuration["Data:TestServiceUrl:UbsService"];
                ApplicationConstant.SmsService = Configuration["Data:TestServiceUrl:SmsService"];
                ApplicationConstant.FcubService = Configuration["Data:TestServiceUrl:FcubService"];
                ApplicationConstant.CardService = Configuration["Data:TestServiceUrl:CardService"];

                ApplicationConstant.SmsUserId = Configuration["Data:TestSmsSetting:SmsUserId"];
                ApplicationConstant.SmsPassword = new Encription().Decrypt(Configuration["Data:TestSmsSetting:SmsPassword"]);
                ApplicationConstant.MobileNumber = Configuration["Data:TestSmsSetting:MobileNumber"];
            }


        }

        public IConfiguration Configuration
        {
            get;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(1);
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ISettingsUsersService, SettingsUsersService>();
            services.AddScoped<ICommonManager, CommonManager>();
            services.AddScoped<IFileProcessManager, FileProcessManager>();
            services.AddScoped<ICustomerManager, CustomerManager>();
            services.AddScoped<IDepositManager, DepositManager>();
            services.AddScoped<IWithdrawCashManager, WithdrawCashManager>();
            services.AddScoped<IWithdrawRemittanceManager, WithdrawRemittanceManager>();
            services.AddScoped<IFundTransferManager, FundTransferManager>();
            services.AddScoped<IBillPayCashManager, BillPayCashManager>();
            services.AddScoped<IBillPayCashPalliBuddyutManager, BillPayCashPalliBuddyutManager>();
            services.AddScoped<IAccountOpeningManager, AccountOpeningManager>();
            services.AddScoped<IServiceRequestManager, ServiceRequestManager>();
            services.AddScoped<ISearchAgentTransactionManager, SearchAgentTransactionManager>();
            services.AddScoped<IBalanceEnquiryManager, BalanceEnquiryManager>(); 
            services.AddScoped<ISearchAccountWorkFlowReqManager, SearchAccountWorkFlowReqManager>();
            services.AddScoped<IAppUserSetupManager, AppUserSetupManager>();
            services.AddScoped<ILimitManager, LimitManager>();
            services.AddScoped<IBillerManager, BillerManager>();
            services.AddScoped<IAgentManager, AgentManager>();
            services.AddScoped<IRemmittanceManager, RemmittanceManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            env.EnvironmentName = EnvironmentName.Production;
            app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {

                //                app.UseExceptionHandler(options =>
                //                {
                //                    options.Run(async context =>
                //                    {
                //                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //                        context.Response.ContentType = "text/html";
                //                        var ex = context.Features.Get<IExceptionHandlerFeature>();
                //                        if (ex != null)
                //                        {
                //                            var message = ex.Error.Message;
                //                            var source = ex.Error.StackTrace.Split(new string[] { "MoveNext()" }, StringSplitOptions.None).FirstOrDefault();
                //                            //var log = new Log
                //                            //{
                //                            //    Message = message,
                //                            //    Source = source,
                //                            //    StackTrace = ex.Error.StackTrace,
                //                            //    Date = DateTime.UtcNow
                //                            //};
                //                            //new LogService().Insert(log);

                //                            var html = @"
                //<html xmlns=""http://www.w3.org/1999/xhtml"">
                //<head>
                //    <title>ERP System</title>
                //</head>
                //<body>
                //    <div style=""text-align:center;margin-top:100px;"">
                //        <h2>Sorry, Exception occur</h2>        
                //    </div>
                //</body>
                //</html>";

                //                            await context.Response.WriteAsync(html).ConfigureAwait(false);
                //                        }
                //                    });
                //                });

                //                //app.UseExceptionHandler("/Home/Error");
                app.UseStatusCodePagesWithRedirects("/Error/{0}");
            }

            app.UseStaticFiles();

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Login}/{action=Index}/{id?}");
            });

        }
    }
}
