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
    /// Делегат для фиксации изменений статуса/параметров книг
    /// </summary>
    /// <param name="bookId"></param>
    /// <param name="changemakerId"></param>
    /// <param name="changeDate"></param>
    /// <param name="type"></param>
    ///<param name="comment"></param>
    /// <returns></returns>
    public delegate Task FixChanges(Guid bookId, Guid changemakerId, DateTime changeDate, ChangeType type, string comment = null);

    /// <summary>Теги для получения изменений</summary>
    public enum ChangesPeriod
    {
        /// <summary>последние</summary>
        latest,
        /// <summary>за все время</summary>
        allTime,
        /// <summary>конкретный период</summary>
        concretePeriod
    }

    /// <summary>Тип вносимых изменений по книге</summary>
    public enum ChangeType
    {
        /// <summary>бронирование книги</summary>
        Reservation,
        /// <summary>возвращение в библиотеку</summary>
        Returning,
        /// <summary>смена статуса книги на утеряна</summary>
        Lost,
        /// <summary>дургие изменения</summary>
        Other
    }

    /// <summary>Интейрейс отвечающий за регистрацию и показ изменений</summary>
    public interface IChangeble
    {
        /// <inheritdoc></inheritdoc>
        /// <summary>
        /// Событие смены статуса книги
        /// </summary>
        public event FixBookStatus StatusChanged;
        /// <summary>
        /// Получение изменений
        /// </summary>
        /// <param name="bookId">книга</param>
        /// <param name="tagTime">Параметр получнеия списка</param>
        /// /// <param name="period">запрашиваемый период, учитывается только при типе conretePeriod</param>
        /// <returns></returns>
        public Task<List<ChangesDto>> GetChanges(Guid? bookId = null,
                                                 ChangesPeriod tagTime = ChangesPeriod.latest,
                                                 Tuple<DateTime, DateTime> period = null);
    }

    /// <summary>Класс, релизующий интерфеск изменений</summary>
    public class ChangesManagerService : IChangeble
    {
        /// <summary>
        /// Событие смены статуса книги
        /// </summary>
        public event FixBookStatus StatusChanged;

        private readonly LibraryUOW _libraryUOW;
        private readonly IMapper _mapper;

        /// <summary>Констурктор с DI</summary>
        public ChangesManagerService(BaseLibraryContext libraryContext, IMapper mapper, IReservable reservations)
        {
            _libraryUOW = new LibraryUOW(libraryContext);
            _mapper = mapper;
            reservations.BookChanged += FixChanges;
        }

        /// <inheritdoc></inheritdoc>
        public Task<List<ChangesDto>> GetChanges(Guid? bookId = null,
                                                 ChangesPeriod tagTime = ChangesPeriod.latest,
                                                 Tuple<DateTime, DateTime> period = null)
        {
            var changesBooks = new List<ChangesDto>();
            switch (tagTime)
            {
                case ChangesPeriod.latest:
                    changesBooks = _mapper.Map(_libraryUOW.Changes.Get().Result, changesBooks);
                    changesBooks = changesBooks.OrderByDescending(c => c.ChangeDate)
                                               .GroupBy(c => c.ChangemakerId)
                                               .Select(c => c.Last()).ToList();
                    changesBooks.ForEach(c => c.ChangemakerFullName = 
                                    _mapper.Map(_libraryUOW.Employees.Get(filter: e => e.Id == 
                                                                           c.ChangemakerId)
                                                                      .Result.FirstOrDefault(), c)
                                                .ChangemakerFullName);
                    break;
                case ChangesPeriod.allTime:
                    changesBooks = _mapper.Map(_libraryUOW.Reservations.Get().Result, changesBooks);
                    changesBooks.ForEach(c => c.ChangemakerFullName =
                                    _mapper.Map(_libraryUOW.Employees.Get(filter: e => e.Id ==
                                                                           c.ChangemakerId)
                                                                      .Result.FirstOrDefault(), c)
                                                .ChangemakerFullName);
                    break;
                case ChangesPeriod.concretePeriod:
                    //TODO резервация за конкретный период
                    break;
                default:
                    break;
            }
            return Task.FromResult(changesBooks);
        }

        /// <summary>
        /// Фиксация изменений
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="changemakerId"></param>
        /// <param name="changeTime"></param>
        /// <param name="type"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        private async Task FixChanges(Guid bookId, Guid changemakerId, DateTime changeTime, ChangeType type, string comment = null)
        {

        }
    }
}
