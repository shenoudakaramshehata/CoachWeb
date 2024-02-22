using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Coach.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;  
using System.Reflection;
using System.Threading.Tasks;
using NToastNotify;
using Newtonsoft.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Identity.UI.Services;
using Coach.Models;
using CorePush.Google;
using CorePush.Apple;
using Coach.Entities.Notification;
using Coach.Email;
using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Reporting;

namespace Coach
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)  
        {

            

            services.AddHttpClient<FcmSender>();    
            services.AddHttpClient<ApnSender>();

            // Configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("FcmNotification");
            services.Configure<FcmNotificationSetting>(appSettingsSection);
            services.AddTransient<INotificationService, NotificationService>();

          

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddDbContext<ApplicationDbContext>(options =>options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<CoachContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

            //
            services.AddDevExpressControls();
            services.AddMvc(options => options.EnableEndpointRouting = false)
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
            services.ConfigureReportingServices(configurator => {
                configurator.ConfigureWebDocumentViewer(viewerConfigurator => {
                    viewerConfigurator.UseCachedReportSourceBuilder();
                });
            });

            //
            services.AddDefaultIdentity<ApplicationUser>()
            .AddRoles<IdentityRole>() // <-- Add this line
            .AddEntityFrameworkStores<ApplicationDbContext>();


            services.AddRazorPages().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null).AddDataAnnotationsLocalization(
                options =>
                {
                    var type = typeof(SharedResource);
                    var assembblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
                    var factory = services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
                    var localizer = factory.Create("SharedResource", assembblyName.Name);
                    options.DataAnnotationLocalizerProvider = (t, f) => localizer;
                }
                );
               services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null).AddDataAnnotationsLocalization(
                options =>
                {
                    var type = typeof(SharedResource);
                    var assembblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
                    var factory = services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
                    var localizer = factory.Create("SharedResource", assembblyName.Name);
                    options.DataAnnotationLocalizerProvider = (t, f) => localizer;
                }
                );
         
            services.AddControllersWithViews().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null).AddDataAnnotationsLocalization(
                options =>
                {
                    var type = typeof(SharedResource);
                    var assembblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
                    var factory = services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
                    var localizer = factory.Create("SharedResource", assembblyName.Name);
                    options.DataAnnotationLocalizerProvider = (t, f) => localizer;
                }


                );
            services.Configure<IdentityOptions>(setupAction =>
            {

                setupAction.Password.RequireDigit = false;
                setupAction.Password.RequiredUniqueChars = 0;
                setupAction.Password.RequireLowercase = false;
                setupAction.Password.RequireNonAlphanumeric = false;
                setupAction.Password.RequireUppercase = false;
                setupAction.Password.RequiredLength = 6;
                setupAction.SignIn.RequireConfirmedEmail = false;
                setupAction.SignIn.RequireConfirmedPhoneNumber = false;
            });
            services.AddRazorPages().AddNToastNotifyToastr(new ToastrOptions()
            {
                ProgressBar = true,
                PreventDuplicates = true,
                CloseButton = true
            });
            services.AddControllersWithViews()
           .AddNewtonsoftJson(options => {
           options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
           options.SerializerSettings.ContractResolver = new DefaultContractResolver();
     }

 );
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.AddRazorPages()
           .AddNewtonsoftJson(options => {
           options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
           options.SerializerSettings.ContractResolver = new DefaultContractResolver();
     }

 );


            services.AddSwaggerGen();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Reporting
            app.UseDevExpressControls();
            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls12;
            //

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),

                new CultureInfo("ar-EG")

            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

                       

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
            app.UseStatusCodePages(async context =>
            {
                if (context.HttpContext.Response.StatusCode == 404)
                {
                    if (context.HttpContext.Request.Path.Value.ToLower().Contains("admin"))
                    {
                        context.HttpContext.Response.Redirect("/Admin/Error");
                    }
                    else {
                        context.HttpContext.Response.Redirect("/Error");
                    }
                    
                    
                }
                //else if (context.HttpContext.Response.StatusCode == 404)
                //{
                //    context.HttpContext.Response.Redirect("/Admin/Error");
                //}
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
        }
    }
}
