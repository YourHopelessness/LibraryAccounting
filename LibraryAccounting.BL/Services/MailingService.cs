using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAccounting.BL.Services
{
    /// <summary>
    /// Интерфейс сервиса рассылки писем
    /// </summary>
    public interface IMailerSendable
    {
        /// <summary>
        /// Ассинхронная отправка почты
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public Task Send(Guid employeeId, Guid bookId);
    }
    /// <summary>
    /// Сервис рассылки писем
    /// </summary>
    public class MailingService : IMailerSendable
    {
        /// <summary>
        /// отправка писем
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public async Task Send(Guid employeeId, Guid bookId)
        {
            //TODO ассинхронная отправка почты
        }
    }
}
