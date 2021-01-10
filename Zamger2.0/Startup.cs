using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zamger2._0.Data;

namespace Zamger2._0
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySQL(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews()
            .AddRazorPagesOptions(options =>
            {
                options.Conventions.AddAreaFolderRouteModelConvention("Identity", "/Account", model =>
                {
                    var selectorCount = model.Selectors.Count;

                    var whitelistedIdentityEndpoints = new List<string>
                    { 
                        "Identity/Account/Manage", "Identity/Account/Manage/ChangePassword", "Identity/Account/Login", "Identity/Account/AccessDenied", "Identity/Account/Logout"
                    };

                    for (var i = selectorCount - 1; i >= 0; i--)
                    {
                        var selectorTemplate = model.Selectors[i].AttributeRouteModel.Template;

                        if (!whitelistedIdentityEndpoints.Contains(selectorTemplate))
                            model.Selectors.RemoveAt(i);
                    }
                });
            }); ;

            Seed.Initialize(services.BuildServiceProvider(), "Test123!");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //https in prod handled by nginx
                app.UseHttpsRedirection();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStatusCodePagesWithReExecute("/Home/Error");
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}")
                .RequireAuthorization();

                //endpoints.Map("/Identity/Account/Manage");
                //endpoints.Map("/Identity/Account/Manage/ChangePassword");
                //endpoints.Map("/Account/Login");
                endpoints.MapRazorPages();
            });
        }
    }
}
