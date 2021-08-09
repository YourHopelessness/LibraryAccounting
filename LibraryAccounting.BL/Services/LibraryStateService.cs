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
    /// Интерфейс для сервиса показа книг
    /// </summary>
    public interface ILibraryCurrentable
    {
        /// <summary>
        /// Событие, происходящее при добавлении книги
        /// </summary>
        public event FixChanges BookAddedOrEdited;
        /// <summary>
        /// Событие, происходящее при бронировании или смене статуса книги
        /// </summary>
        public event FixChanges BookChangesStatus;
        /// <summary>
        /// Получение списка имеющихся книг
        /// </summary>
        /// <returns>Список книг</returns>
        Task<List<BooksDto>> GetBooks();
        /// <summary>
        /// Получение количетсва книг в библиотеке
        /// </summary>
        /// <returns></returns>
        Task<int> GetCount();
        /// <summary>
        /// Получение статусов книг
        /// </summary>
        /// <returns>Список статусов книг</returns>
        Dictionary<string, string> GetStatuses();
        /// <summary>
        /// Добавление книги
        /// </summary>
        /// <param name="book">книга</param>
        /// <param name="comment">комментарий</param>
        /// <returns></returns>
        Task AddEditBook(BooksDto book, string comment = null);
        /// <summary>
        /// Получение текущего состояния книги по идентификатору
        /// </summary>
        /// <param name="id">идентификатор книги</param>
        /// <returns></returns>
        Task<BooksDto> GetBookById(Guid id);
        /// <summary>
        /// Сменить статус книги на утеряна
        /// </summary>
        /// <param name="id">книга</param>
        /// <returns></returns>
        Task LostBook(Guid id);
        /// <summary>
        /// Удалить книгу
        /// </summary>
        /// <param name="id">книга</param>
        /// <returns></returns>
        Task DeleteBook(Guid id);
    }

    /// <summary>Сервис показа списка книг и работы с ним</summary>
    public class LibraryStateService : ILibraryCurrentable
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly LibraryUOW _libraryUoW;
        private readonly IAuthenticable _auth;

        /// <inheritdoc></inheritdoc>
        public event FixChanges BookAddedOrEdited;
        /// <inheritdoc></inheritdoc>
        public event FixChanges BookChangesStatus;

        /// <summary>Конструктор со внедрением зависимости контекста базы данных </summary>
        /// <param name="context">Контекст базы данных</param>
        /// <param name="mapper">Карты для маппера</param>
        /// <param name="httpAccessor">провайдер http контекста</param>
        /// <param name="auth">сотрудники</param>
        /// <param name="reservation"></param>
        /// <param name="change"></param>
        public LibraryStateService(BaseLibraryContext context,
            IMapper mapper,
            IHttpContextAccessor httpAccessor,
            IAuthenticable auth,
            IReservable reservation,
            IChangeble change)
        {
            _libraryUoW = new LibraryUOW(context);
            _mapper = mapper;
            _httpAccessor = httpAccessor;
            _auth = auth;
            reservation.BookReservation += ChangeBookStatus;
            BookChangesStatus += change.FixChanges;
            BookAddedOrEdited += change.FixChanges;
        }

        /// <inheritdoc></inheritdoc>
        public async Task<int> GetCount() => (await GetBooks()).Count; // Количество книг

        /// <inheritdoc></inheritdoc>
        public async Task<BooksDto> GetBookById(Guid id)
        {
            var books = await _libraryUoW.Books.Get(filter: b => b.Id == id);
            return _mapper.Map<BooksDto>(books.FirstOrDefault());
        }

        /// <inheritdoc></inheritdoc>
        public Dictionary<string, string> GetStatuses()
        {
            //Словарь статусов перевода из бд на русский
            var bookStatuses = new Dictionary<string, string>{
                { "In library" , "В библиотеке" },
                { "Issued" , "Выдана"},
                { "Losted", "Утеряна"}};

            if (_httpAccessor.HttpContext.User.IsInRole("admin"))
            {
                return bookStatuses; //админимтратору доступны все книги для просмотра
            }
            //пользователю не должны показываться книги в статусе утеряна
            return bookStatuses.Where(b => b.Value != "Утеряна").ToDictionary(dict => dict.Key, dict => dict.Value);
        }


        /// <inheritdoc></inheritdoc>
        public async Task<List<BooksDto>> GetBooks()
        {
            List<BooksDto> library = new();
            if (_httpAccessor.HttpContext.User.IsInRole("admin"))
            {
                var books = await _libraryUoW.Books.Get();
                library = _mapper.Map<List<Books>, List<BooksDto>>(books.ToList());
            }
            else
            {
                var books = await _libraryUoW.Books.Get(b => b.StatusId != 3);
                library = _mapper.Map<List<Books>, List<BooksDto>>(books.ToList());
            }
            library.ForEach(async l =>
            {
                var status = await Task.FromResult(_libraryUoW.BooksStatuses.Get(filter: e => e.Id ==
                                       Convert.ToInt32(l.Status)));
                l.Status = GetStatuses()[status.Result.First().Status];
            });
            return library;
        }

        /// <inheritdoc></inheritdoc>
        public async Task AddEditBook(BooksDto book, string comment = null)
        {
            if (_httpAccessor.HttpContext.User.IsInRole("admin"))
            {
                if (book.BookId != Guid.Empty && GetBookById(book.BookId) != null) // редактирование книги
                {
                    if (comment == String.Empty)
                        _httpAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest; // для редактирования комментарий обязателен
                    Books nBook = _mapper.Map<Books>(book);
                    _libraryUoW.Books.Update(nBook);
                    await BookAddedOrEdited?.Invoke(nBook.Id,
                                            _auth.GetUserId(),
                                            DateTime.Now,
                                            ChangeType.Edited, comment);
                }
                else
                {
                    Books nBook = _mapper.Map<Books>(book);
                    nBook.StatusId = 2; // статус в библиотеке
                    _libraryUoW.Books.Insert(nBook);
                    await BookAddedOrEdited?.Invoke(nBook.Id, _auth.GetUserId(), DateTime.Now, ChangeType.Added);
                }
                _libraryUoW.Save();
                Guid changemakerId = Guid.Parse(_httpAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value);
                await BookAddedOrEdited(book.BookId, changemakerId, DateTime.Now, ChangeType.Added);
            }
            else
            {
                _httpAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden; // запретить добавление книги
            }
        }

        /// <inheritdoc></inheritdoc>
        public async Task LostBook(Guid id)
        {
            var book = await _libraryUoW.Reservations.Get(filter: b =>
                    b.BookId == id &&
                    b.ReservationDate < DateTime.Now &&
                    !b.ReturningFlag.Value);
            book.FirstOrDefault().ReturnDate = DateTime.Now;
            _libraryUoW.Reservations.Update(book.FirstOrDefault());
            await ChangeBookStatus(id, "losted");
        }

        /// <inheritdoc></inheritdoc>
        public async Task DeleteBook(Guid id)
        {
            var res = await _libraryUoW.Reservations.Get(filter: b => b.BookId == id);
            _libraryUoW.Reservations.Delete(res.FirstOrDefault()); 
            var change = await _libraryUoW.Changes.Get(filter: b => b.BookId == id);
            _libraryUoW.Changes.Delete(change.FirstOrDefault());
            var book = await _libraryUoW.Books.Get(filter: b => b.Id == id);
            _libraryUoW.Books.Delete(book.FirstOrDefault());
            _libraryUoW.Save();
        }

        /// <summary>
        /// Смена статуса книги
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="status"></param>
        /// <returns></returns>s
        private async Task ChangeBookStatus(Guid bookId, string status)
        {
            var bookDto = await GetBookById(bookId);

            int newStatus = bookDto == null ? 0 :
                            status == "reserved" ? (int) ChangeType.Reservation :
                            status == "returned" ? (int)ChangeType.Returning :
                            status == "losted" ? (int)ChangeType.Lost : 0;
            if (newStatus == 0)
            {
                _httpAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }
            var book = _libraryUoW.Books.Get(b => b.Id == bookId).Result.First();
            book.StatusId = newStatus - 1; // Статусы в ChangeType не соответсвуют статусам в справочнике в базе данных
            _libraryUoW.Books.Update(book);
            _libraryUoW.Save();
            await BookChangesStatus?.Invoke(bookId, _auth.GetUserId(), DateTime.Now, (ChangeType) newStatus);
        }
    }
}
