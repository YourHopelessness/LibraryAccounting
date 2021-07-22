using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using LibraryAccounting.DAL.DB;
using LibraryAccounting.BL.Services;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace LibraryAccounting
{
    /// <summary>
    /// Основной класс приложения
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Точка запуска приложения
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// Свойство конфигурации
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Конфигурация сервисов
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServerSideBlazor();
            services.AddHttpContextAccessor();
            services.AddDbContext<BaseLibraryContext, LibraryDbContext>(); // Добавляем контекст бд
            services.AddScoped<IAuthenticable, AuthService>();
            services.AddScoped<ILibraryCurrentable, LibraryStateService>();
            services.AddScoped<IReservable, ReservationManagerService>();
            services.AddScoped<IChangeble, ChangesManagerService>();
            services.AddScoped<IMailerSendable, MailingService>();
            services.AddScoped<IEmployeesStatable, EmployeeService>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = new PathString("/Account/Login");
                        options.AccessDeniedPath = new PathString("/Account/AccessDenied");
                    });
            services.AddAutoMapper(typeof(MapperConfigurateMap));
            services.AddRouting();
            services.AddRazorPages();
            services.AddMemoryCache();
        }

        /// <summary>
        /// Конфигруация приложения
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithRedirects("/Error?error={0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            var cookiePolicyOptions = new CookiePolicyOptions
            {
                CheckConsentNeeded = context => true,
                MinimumSameSitePolicy = SameSiteMode.Strict,
            };
            app.UseCookiePolicy(cookiePolicyOptions);
            app.UseRouting();

            app.UseAuthentication();    // аутентификация
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
