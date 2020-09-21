using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsStore.Models;

namespace SportsStore {
    public class Startup {
        private IConfiguration Configuration { get; set; }
        // получаем внедрением ссылку на объект глобальной конфигурации
        public Startup (IConfiguration config) {
            Configuration = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices (IServiceCollection services) {
            // активация модель-вид-представление
            services.AddControllersWithViews ();
            // активация контекста БД (с созданием соединения)
            services.AddDbContext<StoreDbContext> (opts => {
                opts.UseSqlServer (
                    Configuration["ConnectionStrings:SportsStoreConnection"]);
            });
            services.AddScoped<IStoreRepository, EFStoreRepository> ();
            services.AddScoped<IOrderRepository, EFOrderRepository>();
            // активация дополнительного мини-каркаса работы с серверными страницами
            services.AddRazorPages();
            // активация работы с http-сеансами
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }
            /* добавление стандартных MiddleWare-звеньев
            в PipeLine приложения */
            app.UseStatusCodePages ();
            app.UseStaticFiles ();
            app.UseSession();
            app.UseRouting ();

            app.UseEndpoints (endpoints => {
                /* endpoints.MapControllerRoute("pagination",
                    "Products/{Page?}/{productPage:regex(^[1-9]\\d*$)=1}",
                    new { Controller = "Home", action = "Index" }); */

                endpoints.MapControllerRoute ("catpage",
                    "{category}/Page{productPage::regex(^[1-9]\\d*$)=1}",
                    new { Controller = "Home", action = "Index" });
                endpoints.MapControllerRoute ("page", "Page{productPage::regex(^[1-9]\\d*$)=1}",
                    new {
                        Controller = "Home", action = "Index"
                    });
                endpoints.MapControllerRoute ("category", "{category}",
                    new {
                        Controller = "Home", action = "Index", productPage =
                            1
                    });
                endpoints.MapControllerRoute ("pagination",
                    "Products/Page{productPage:regex(^[1-9]\\d*$)=1}",
                    new {
                        Controller = "Home", action = "Index"
                    });
                // добавление роута по умолчанию
                endpoints.MapDefaultControllerRoute ();
                // добавление роута серверных страниц
                endpoints.MapRazorPages();
                // The same:
                /* endpoints.MapControllerRoute("pagination",
                    "Products/{Page?}/{productPage:regex(^[1-9]\\d*$)}",
                    new { Controller = "Home", action = "Index", productPage = 1 });
                    endpoints.MapDefaultControllerRoute();
                }); */
                // endpoints.MapDefaultControllerRoute ();
            });

            SeedData.EnsurePopulated (app);
        }
    }
}