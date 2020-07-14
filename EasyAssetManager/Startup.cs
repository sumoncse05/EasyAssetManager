using System;
using System.ComponentModel;
using EasyAssetManagerCore.BusinessLogic.Operation;
using EasyAssetManagerCore.BusinessLogic.Operation.Asset;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Model.CommonModel;
using EasyAssetManagerCore.Models.CommonModel;
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
                ApplicationConstant.CardService = Configuration["Data:LiveServiceUrl:CardService"];
            }
            else
            {
                dataSource = Configuration["Data:TestConnection:EbankConnectionString:DataSource"];
                userId = Configuration["Data:TestConnection:EbankConnectionString:UserID"];
                password = Configuration["Data:TestConnection:EbankConnectionString:Password"];
                ApplicationConstant.ConnectionString = string.Format("Data Source={0};Password={1};User ID={2};", dataSource, new Encription().Decrypt(password), new Encription().Decrypt(userId));
                ApplicationConstant.EbankConnectionString = string.Format("Data Source={0};Password={1};User ID={2};", dataSource, new Encription().Decrypt(password), new Encription().Decrypt(userId));
                ApplicationConstant.CardService = Configuration["Data:TestServiceUrl:CardService"];
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
            TypeDescriptor.AddAttributes(typeof(DateTime), new TypeConverterAttribute(typeof(DeDateTimeConverter)));
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(1);
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ISettingsUsersService, SettingsUsersService>();
           

            services.AddScoped<IReportManager, ReportManager>();
            services.AddScoped<IRMAssetManager, RMAssetManager>();

            services.AddScoped<IFileProcessManager, FileProcessManager>(); 
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

               // app.UseExceptionHandler("/Login/Index");
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
