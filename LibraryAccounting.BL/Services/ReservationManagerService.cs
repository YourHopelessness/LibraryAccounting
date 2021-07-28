using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LibraryAccounting.BL.Dto;
using LibraryAccounting.DAL.DB;
using LibraryAccounting.DAL.Entities;
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
        /// <param name="id">книга</param>
        /// <param name="returningDate">дата возвращения</param>
        /// <returns></returns>
        public Task<bool> ReserveBook(Guid id, DateTime returningDate);

        /// <summary>
        /// получить книги в использовании или за весь период
        /// </summary>
        /// <param name="bookId">книга</param>
        /// <param name="tagTime">Параметр получнеия списка</param>
        /// /// <param name="period">запрашиваемый период, учитывается только при типе conretePeriod</param>
        /// <returns></returns>
        public Task<List<BookInReservationsDto>> GetReservations(Guid? bookId = null,
                                                                 ReservationsPeriod tagTime = ReservationsPeriod.currently,
                                                                 Tuple<DateTime, DateTime> period = null);
    }

    public class ReservationManagerService : IReservable
    {
        private readonly LibraryUOW _libraryUOW;
        private readonly IMapper _mapper;
        private readonly IEmployeesStatable _employees;

        /// <summary>
        /// Событие создаваемое при изменении статуса книги
        /// </summary>
        public event FixChanges BookChanged;

        /// <inheritdoc></inheritdoc>
        public ReservationManagerService(BaseLibraryContext libraryContext, 
            IMapper mapper, 
            IEmployeesStatable employees)
        {
            _libraryUOW = new LibraryUOW(libraryContext);
            _mapper = mapper;
            _employees = employees;
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
                    resevedBooks = _mapper.Map(_libraryUOW.Reservations.Get(filter: b => b.ReturnDate > DateTime.Now && 
                                                                            b.ReturningFlag == false && 
                                                                            (bookId == null || b.BookId == bookId)).Result, resevedBooks);
                    resevedBooks.ForEach(r =>
                                r.ReaderName = _libraryUOW.Employees.Get(filter: e => e.Id == r.ReaderId).Result.FirstOrDefault().FirstName + " " +
                                               _libraryUOW.Employees.Get(filter: e => e.Id == r.ReaderId).Result.FirstOrDefault().LastName);
                    break;
                case ReservationsPeriod.allTime:
                    resevedBooks = _mapper.Map(_libraryUOW.Reservations.Get(filter: b => (bookId == null || 
                                                                                b.BookId == bookId)).Result, resevedBooks);
                    resevedBooks.ForEach(r =>
                                r.ReaderName = _libraryUOW.Employees.Get(filter: e => e.Id == r.ReaderId).Result.FirstOrDefault().FirstName + " " +
                                               _libraryUOW.Employees.Get(filter: e => e.Id == r.ReaderId).Result.FirstOrDefault().LastName);
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
        public async Task<bool> ReserveBook(Guid id, DateTime returningDate)
        {
            var reader = await _employees.GetReader();
            BookInReservationsDto reservBook = new();
            reservBook.ReaderId = reader.ReaderId;
            reservBook.BookId = id;
            reservBook.ReservationDate = DateTime.Now;
            reservBook.ReturningDate = returningDate;
            await Task.Run(() =>  _libraryUOW.Reservations.Insert(_mapper.Map<Reservations>(reservBook)));
            _libraryUOW.Save();
            //TODO делать запись изменения
            return true;
        }
        
    }
}
