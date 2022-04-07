using Authentication.Data;
using Authentication.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.ML.OnnxRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication
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
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options =>
                options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection =
                        Configuration.GetSection("Authentication:Google");

                    options.ClientId = "1079177695201-84d6jbs47qv6pg4pi2fs210ph5j6hvg9.apps.googleusercontent.com";
                    options.ClientSecret = "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";
                });

            //I HOPE THIS WORKS...
            services.AddDbContext<CrashDbContext>(options =>

                options.UseMySql(Configuration.GetConnectionString("CrashDataDbConnection"))
            );


            //services.AddIdentity<IdentityUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(Configuration["ConnectionStrings: ApplicationDataDbConnection"]));
                
            //services.AddDbContext<ApplicationDbContext>(options =>

            //    options.UseMySql(Configuration.GetConnectionString("ApplicationDataDbConnection"))
            //);

            services.AddControllersWithViews();
            //services.AddRazorPages();

            //TESTING THIS OUT JK THIS ACTUALLY WORKS, DON'T DELETE
            services.AddRazorPages(options =>
            {
                //options.Conventions.AuthorizePage("/Contact");
                options.Conventions.AuthorizeFolder("/Admin");
                //options.Conventions.AllowAnonymousToPage("/Private/PublicPage");
                options.Conventions.AllowAnonymousToFolder("/Views/");
            });


            services.AddScoped<ICrashRepository, EFCrashRepository>();
            services.AddServerSideBlazor();

            services.AddSingleton<InferenceSession>(
                new InferenceSession("wwwroot/utah_crash_severity.onnx")
            );
        }
        // NEW STUFF (I HOPE THIS WORKS)
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "Paging",
                    "Page{pageNum}",
                    new { Controller = "Home", action = "Temp", pageNum = 1 });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "Calculator",
                    pattern: "{controller=Inference}/{action=Calculator}");

                endpoints.MapDefaultControllerRoute();

                endpoints.MapRazorPages();

                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/admin/{*catchall}", "/admin/Index");
            });

        }
    }
}



// OLD STUFF
// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//        {
//            if (env.IsDevelopment())
//            {
//                app.UseDeveloperExceptionPage();
//                app.UseDatabaseErrorPage();
//            }
//            else
//            {
//                app.UseExceptionHandler("/Home/Error");
//                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//                app.UseHsts();
//            }
//            app.UseHttpsRedirection();
//            app.UseStaticFiles();

//            app.UseRouting();

//            app.UseAuthentication();
//            app.UseAuthorization();

//            app.UseEndpoints(endpoints =>
//            {
//                endpoints.MapControllerRoute(
//                    name: "default",
//                    pattern: "{controller=Home}/{action=Index}/{id?}");
//                endpoints.MapRazorPages();
//            });
//        }
//    }
//}