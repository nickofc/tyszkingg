using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sentry;
using TwitchLib.Api;
using TwitchLib.Api.V5;
using TwReplay.Areas.Identity;
using TwReplay.Data;
using TwReplay.Database;
using TwReplay.Services;

namespace TwReplay
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            /* Framework deps -- */

            services.AddDatabase<ApplicationDbContext>(o =>
            {
                var databaseEngineTypeFactory = new DatabaseEngineTypeFactory();

                o.EngineType = databaseEngineTypeFactory.Get(Configuration["Database:EngineType"]);
                o.ConnectionString = Configuration["Database:ConnectionString"];
            });

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

                options.SignIn.RequireConfirmedAccount = true;
                options.SignIn.RequireConfirmedEmail = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddScoped<AuthenticationStateProvider,
                RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddHttpContextAccessor();
            services.AddLogging();
            services.AddHttpClient();
            services.AddMemoryCache();

            /* -- Framework deps */

            services.AddTwitchApi(o =>
            {
                o.ClientId = Configuration["TwitchApi:ClientId"];
                o.AccessToken = Configuration["TwitchApi:ClientSecret"];
            });

            services.AddHttpDownloadService();
            services.AddVideobinUploadService();

            services.AddReuploadBackgroundService(o => { o.Delay = TimeSpan.FromMinutes(5); });


            // services.AddScoped<TwitchApi>();
            // services.AddScoped<TwitchClipDownloader>();
            // services.AddScoped<TwitchClipService>();
            // services.AddScoped<ReuploadService>();

            // services.AddScoped<ReuploadClipService>();

            // services.AddScoped<ITwitchApiClipsService, TwitchApiClipsService>();
            // services.AddScoped<IDownloadService, HttpDownloadService>();

            // services.AddHostedService<ReuploadBackgroundService>();
            // services.AddHostedService<EnsureFileIsAvailableBackgroundService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSentry(o =>
            {
                o.Debug = false;
                o.DiagnosticsLevel = SentryLevel.Debug;

                o.Dsn = Configuration["Sentry:DNS"];

                if (string.IsNullOrEmpty(o.Dsn))
                {
                    throw new InvalidOperationException($"Key (sentry) not found.");
                }

                o.MaxBreadcrumbs = 150;

                o.MinimumBreadcrumbLevel = LogLevel.Trace;
                o.MinimumEventLevel = LogLevel.Error;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}