using Microsoft.Extensions.DependencyInjection;
using LibraryAccounting.BL.Services;
using LibraryAccounting.DAL.DB;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.NotificationMailing
{
    /// <summary>
    /// Класс запуска сервиса
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Точка входа сервиса
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        /// <inheritdoc></inheritdoc>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddDbContext<BaseLibraryContext, LibraryDbContext>();
                    services.AddScoped<IMailerSendable, MailingService>();
                });
    }
}
