using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAccounting.DAL.Repositories;
using LibraryAccounting.DAL.Entities;
using LibraryAccounting.DAL.DB;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using LibraryAccounting.BL.Dto;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace LibraryAccounting.BL.Services
{
    /// <summary>
    /// Делегат фиксации изменений статусов книг
    /// </summary>
    /// <param name="bookId"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public delegate Task FixBookStatus(Guid bookId, string status);

    /// <summary>
    /// Интерфейс для сервиса показа книг
    /// </summary>
    public interface ILibraryCurrentable
    {
        /// <summary>
        /// Получение списка имеющихся книг
        /// </summary>
        /// <returns>Список книг</returns>
        public Task<List<BooksDto>> GetBooks();
        /// <summary>
        /// Получение количетсва книг в библиотеке
        /// </summary>
        /// <returns></returns>
        public Task<int> GetCount();
    }

    /// <summary>Сервис показа списка книг и работы с ним</summary>
    public class LibraryStateService : ILibraryCurrentable
    {
        private IMapper _mapper;
        private LibraryUOW _libraryUoW;
        private Dictionary<string, string> bookStatuses = new Dictionary<string, string>{ 
            { "In library" , "В библиотеке" },
            { "Issued" , "Выдана"},
            { "Losted", "Утреяна"}};

        /// <summary>Конструктор со внедрением зависимости контекста базы данных </summary>
        /// <param name="context">Контекст базы данных</param>
        /// <param name="mapper">Карты для маппера</param>
        /// <param name="changes">Измененеия</param>
        ///  <param name="reservations">Бронирование</param>
        public LibraryStateService(BaseLibraryContext context, IMapper mapper, IChangeble changes)
        {
            _libraryUoW = new LibraryUOW(context);
            _mapper = mapper;
            changes.StatusChanged += ChangeBookStatus; // будут ли они обрабатываться ?
        }

        /// <inheritdoc></inheritdoc>
        public Task<int> GetCount() => Task.FromResult(GetBooks().Result.Count); // Количество книг

        /// <inheritdoc></inheritdoc>
        public Task<List<BooksDto>> GetBooks()
        {
            List<BooksDto> library = _mapper.Map<List<Books>, List<BooksDto>>(_libraryUoW.Books.Get().Result.ToList());
            library.ForEach(l => l.Status = bookStatuses[_libraryUoW.BooksStatuses.Get(filter: e => e.Id == Convert.ToInt32(l.Status)).Result.FirstOrDefault().Status]);
            return Task.FromResult(library);
        }

        /// <summary>
        /// Смена статуса книги
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        private async Task ChangeBookStatus(Guid bookId, string status)
        {
            /* TODO
             * проставить в таблице книг статус нужной книге
             */
        }
    }
}
