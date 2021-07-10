using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryAccounting.DAL.DB;
using LibraryAccounting.BL.Services;
using LibraryAccounting.DAL.Entities;

namespace LibraryAccounting.NotificationMailing
{
    /// <summary>
    /// Сервис отправки напоминаний
    /// </summary>
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly BaseLibraryContext _libraryContext;
        private readonly IMailerSendable _mailer;

        /// <summary>
        /// Класс сервиса
        /// </summary>
        /// <param name="logger"></param>
        public Worker(ILogger<Worker> logger, BaseLibraryContext context, IMailerSendable mailer)
        {
            _logger = logger;
            _libraryContext = context;
            _mailer = mailer;
        }

        /// <summary>
        /// Фукция сервиса
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                /* TODO
                 * Получаем список адресатов
                 * отпраляем письма
                 * пишем в лог сколько кому отправили
                 * засыпаем на сутки
                 * время отправки задаем из конфига 
                 */
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
