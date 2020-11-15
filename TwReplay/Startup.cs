using System;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TwReplay.Areas.Identity;
using TwReplay.Data;
using TwReplay.Services;
using TwReplay.Services.Download;
using TwReplay.Storage.Abstraction;
using TwReplay.Storage.Videobin;
using TwReplay.Twitch;
using TwReplay.Twitch.Abstraction;

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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services
                .AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>
                >();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddHttpClient();

            services.AddScoped(provider =>
            {
                var factory = provider.GetRequiredService<IHttpClientFactory>();
                return factory.CreateClient();
            });

            services.AddScoped(_ => Configuration.GetValue<TwitchApiConfig>("TwitchApi"));

            services.AddMemoryCache();

            services.AddScoped<IUploadService>(provider =>
            {
                var httpClient = provider.GetRequiredService<HttpClient>();
                var config = new VideobinUploadServiceConfig();
                return new VideobinUploadService(httpClient, config);
            });

            services.AddScoped<TwitchApi>();
            services.AddScoped<TwitchClipDownloader>();
            services.AddScoped<TwitchClipService>();
            services.AddScoped<ReuploadService>();

            services.AddScoped<ReuploadClipService>();

            services.AddScoped<ITwitchApiClipsService, TwitchApiApiClipsService>();
            services.AddScoped<IDownloadService, HttpDownloadService>();

            services.AddHostedService<ReuploadBackgroundService>();

            services.AddLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext context)
        {
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