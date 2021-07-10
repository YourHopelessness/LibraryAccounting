using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LibraryAccounting.BL.Dto;
using LibraryAccounting.DAL.DB;
using LibraryAccounting.DAL.Repositories;

namespace LibraryAccounting.BL.Services
{
    /// <summary>
    /// Перечисление списка вовзращаемоых бронирований
    /// </summary>
    public enum ReservationsPeriod
    {
        /// <summary>текущие</summary>
        currently,
        /// <summary>за все время</summary>
        allTime,
        /// <summary>конкретный период</summary>
        concretePeriod
    }

    /// <summary>
    /// Интерфес для получения и регитсрации бронирования
    /// </summary>
    public interface IReservable
    {
        /// <summary>
        /// Событие регитстрации
        /// </summary>
        public event FixChanges BookChanged;
        /// <summary>
        /// Резервация книги
        /// </summary>
        /// <param name="book">книга</param>
        /// <param name="employee">работник</param>
        /// <param name="reservationDate">дата брони</param>
        /// <param name="returningDate">дата возвращения</param>
        /// <returns></returns>
        public Task<bool> ReserveBook(Guid book, Guid employee, DateTime reservationDate, DateTime returningDate);

        /// <summary>
        /// получить книги в использовании или за весь период
        /// </summary>
        /// <param name="bookId">книга</param>
        /// <param name="tagTime">Параметр получнеия списка</param>
        /// /// <param name="period">запрашиваемый период, учитывается только при типе conretePeriod</param>
        /// <returns></returns>
        public Task<List<BookInReservationsDto>> GetReservations(Guid? bookId = null,
                                                                 ReservationsPeriod tagTime = ReservationsPeriod.currently,
                                                                 Tuple<DateTime, DateTime> period = nul);
    }

    public class ReservationManagerService : IReservable
    {
        private readonly LibraryUOW _libraryUOW;
        private readonly IMapper _mapper;

        /// <summary>
        /// Событие создаваемое при изменении статуса книги
        /// </summary>
        public event FixChanges BookChanged;

        /// <inheritdoc></inheritdoc>
        public ReservationManagerService(BaseLibraryContext libraryContext, IMapper mapper)
        {
            _libraryUOW = new LibraryUOW(libraryContext);
            _mapper = mapper;
        }
        /// <inheritdoc></inheritdoc>
        public Task<List<BookInReservationsDto>> GetReservations(Guid? bookId = null,
                                                                 ReservationsPeriod tagTime = ReservationsPeriod.currently,
                                                                 Tuple<DateTime, DateTime> period = null)
        {
            var resevedBooks = new List<BookInReservationsDto>();
            switch(tagTime)
            {
                case ReservationsPeriod.currently:
                    resevedBooks = _mapper.Map(_libraryUOW.Reservations.Get(filter: b => b.ReturnDate < DateTime.Now && b.ReturnDate != null).Result, resevedBooks);
                    break;
                case ReservationsPeriod.allTime:
                    resevedBooks = _mapper.Map(_libraryUOW.Reservations.Get().Result, resevedBooks);
                    break;
                case ReservationsPeriod.concretePeriod:
                    //TODO резервация за конкретный период
                    break;
                default:
                    break;
            }
            return Task.FromResult(resevedBooks);
        }
        /// <inheritdoc></inheritdoc>
        public Task<bool> ReserveBook(Guid book, Guid employee, DateTime reservationDate, DateTime returningDate)
        {
            /* TODO
             * Нвходим книгу для бронирования и человека в базе
             * Записываем в таблицу бронирования
             * посылаем сигнал о смене статуса книги на выдана
             * если что-то пошло не так выбрасываем исключение (или true)
             */
            return Task.FromResult(false);
        }
        
    }
}
