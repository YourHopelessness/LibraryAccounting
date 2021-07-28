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
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Net;

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
        /// Событие, происходящее при добавлении книги
        /// </summary>
        public event FixChanges BookAdded;
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
        /// <summary>
        /// Получение статусов книг
        /// </summary>
        /// <returns>Список статусов книг</returns>
        public Task<Dictionary<string, string>> GetStatuses();
        /// <summary>
        /// Добавление книги
        /// </summary>
        /// <param name="book">книга</param>
        /// <returns></returns>
        public Task AddEditBook(BooksDto book);
        /// <summary>
        /// Получение текущего состояния книги по идентификатору
        /// </summary>
        /// <param name="id">идентификатор книги</param>
        /// <returns></returns>
        public Task<BooksDto> GetBookById(Guid id);
    }

    /// <summary>Сервис показа списка книг и работы с ним</summary>
    public class LibraryStateService : ILibraryCurrentable
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly LibraryUOW _libraryUoW;

        /// <inheritdoc></inheritdoc>
        public event FixChanges BookAdded;

        /// <summary>Конструктор со внедрением зависимости контекста базы данных </summary>
        /// <param name="context">Контекст базы данных</param>
        /// <param name="mapper">Карты для маппера</param>
        /// <param name="changes">Измененеия</param>
        /// <param name="httpAccessor">Измененеия</param>
        public LibraryStateService(BaseLibraryContext context, IMapper mapper, IChangeble changes, IHttpContextAccessor httpAccessor)
        {
            _libraryUoW = new LibraryUOW(context);
            _mapper = mapper;
            _httpAccessor = httpAccessor;
            changes.StatusChanged += ChangeBookStatus; // будут ли они обрабатываться ?
            BookAdded += changes.FixChanges;
        }

        /// <inheritdoc></inheritdoc>
        public async Task<int> GetCount() => await Task.Run(() => GetBooks().Result.Count); // Количество книг

        /// <inheritdoc></inheritdoc>
        public async Task<BooksDto> GetBookById(Guid id)
        {
            return _mapper.Map<BooksDto>((await _libraryUoW.Books.Get(filter: b => b.Id == id)).FirstOrDefault());
        }

        /// <inheritdoc></inheritdoc>
        public async Task<Dictionary<string, string>> GetStatuses()
        {
            //Словарь статусов перевода из бд на русский
            var bookStatuses = new Dictionary<string, string>{
                { "In library" , "В библиотеке" },
                { "Issued" , "Выдана"},
                { "Losted", "Утеряна"}};

            if (_httpAccessor.HttpContext.User.IsInRole("admin"))
            {
                return await Task.Run(() => bookStatuses);
            }
            //пользователю не должны показываться книги в статусе утеряна
            return await Task.Run(() => bookStatuses.Where(b => b.Value != "Утеряна").ToDictionary(dict => dict.Key, dict => dict.Value));
        }


        /// <inheritdoc></inheritdoc>
        public async Task<List<BooksDto>> GetBooks()
        {
            List<BooksDto> library = new List<BooksDto>();
            if (_httpAccessor.HttpContext.User.IsInRole("admin"))
            {
                library = await Task.Run(() => _mapper.Map<List<Books>, List<BooksDto>>(_libraryUoW.Books.Get().Result.ToList()));
            }
            else
            {
                library = await Task.Run(() => _mapper.Map<List<Books>, List<BooksDto>>(_libraryUoW.Books.Get(b => b.StatusId != 3).Result.ToList()));
            }
            await Task.Run(() => library.ForEach(l => l.Status = 
                                GetStatuses().Result[_libraryUoW.BooksStatuses.Get(filter: e => e.Id == 
                                           Convert.ToInt32(l.Status)).Result.FirstOrDefault().Status]));
            return library;
        }

        /// <inheritdoc></inheritdoc>
        public async Task AddEditBook(BooksDto book)
        {
            if (_httpAccessor.HttpContext.User.IsInRole("admin"))
            {
                if (book.BookId != Guid.Empty && GetBookById(book.BookId) != null)
                {
                    Books nBook = _mapper.Map<Books>(book);
                    await Task.Run(() => _libraryUoW.Books.Update(nBook));
                }
                else
                {
                    Books nBook = _mapper.Map<Books>(book);
                    nBook.StatusId = 2;
                    await Task.Run(() => _libraryUoW.Books.Insert(nBook));
                }
                _libraryUoW.Save();
                Guid changemakerId = Guid.Parse(_httpAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value);
                await BookAdded(book.BookId, changemakerId, DateTime.Now, ChangeType.Added);
            }
            else
            {
                _httpAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden; // запретить добавление книги
            }
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
