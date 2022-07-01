using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VueMvc.Filter;
using VueMvc.Models;

namespace VueMvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            // 設定ファイル読込
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddJsonFile("disettings.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            // DIコンテナに対象クラスを登録
            RegisterDiContainer(services);

            services.AddMvc(options => {
                options.EnableEndpointRouting = false;
                options.Filters.Add(typeof(LoggingFilter));
                options.Filters.Add(typeof(TransactionFilter));
            });

            services.AddDbContext<MvcMovieContext>(options =>
                  options.UseSqlite(Configuration.GetConnectionString("MovieDatabase")));
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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// 設定ファイルからDIコンテナにクラスを登録します。
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        private void RegisterDiContainer(IServiceCollection services) {
            foreach (IConfigurationSection section in Configuration.GetSection("disettings").GetChildren().ToList()) {
                var _scope = section.GetValue<string>("scope")?? "Request";
                var _interface = section.GetValue<string>("interface");
                var _extend = section.GetValue<string>("extend");

                switch(_scope.ToLower()) {
                    case "instance" : {
                        services.AddTransient(Type.GetType(_interface), Type.GetType(_extend));
                        break;
                    }
                    case "request" : {
                        services.AddScoped(Type.GetType(_interface), Type.GetType(_extend));
                        break;
                    }
                    case "singleton" : {
                        services.AddSingleton(Type.GetType(_interface), Type.GetType(_extend));
                        break;
                    }
                }
            }
        }
    }
}
