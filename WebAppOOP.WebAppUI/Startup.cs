using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppOOP.BusinessLogic.Authentication.CartAuthentication;
using WebAppOOP.BusinessLogic.Authentication.CartAuthentication.Interfaces;
using WebAppOOP.BusinessLogic.Authentication.UserAuthentication;
using WebAppOOP.BusinessLogic.Authentication.UserAuthentication.Interfaces;
using WebAppOOP.BusinessLogic.Hashing;
using WebAppOOP.BusinessLogic.Hashing.Interface;
using WebbAppOOP.Data.DataAccess;
using WebbAppOOP.Data.DataAccess.Interfaces;

namespace WebAppOOP.WebAppUI
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
            services.AddAuthentication("CookieAuth").AddCookie("CookieAuth", options =>
            {
                options.Cookie.Name = "CookieAuth";
                options.AccessDeniedPath = "/Index";
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                {
                    policy.RequireClaim("Admin");
                });
                options.AddPolicy("User", policy =>
                {
                    policy.RequireClaim("User");
                });

            });
            services.AddSession();
            services.AddMemoryCache();
            services.AddScoped<IHash, HashKey>();
            services.AddScoped<IAuthentication, CookieAuthentication>();
            services.AddScoped<ICartAuthentication, CartAuthentication>();
            services.AddScoped<IProductDataAccess, ProductDataAccess_JSON>();
            services.AddScoped(typeof(IDataAccess<>), typeof(EntityDataAccess_JSON<>));
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
