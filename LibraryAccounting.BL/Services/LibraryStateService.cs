﻿using System;
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
        /// <summary>
        /// Получение статусов книг
        /// </summary>
        /// <returns>Список статусов книг</returns>
        public Task<Dictionary<string, string>> GetStatuses();
    }

    /// <summary>Сервис показа списка книг и работы с ним</summary>
    public class LibraryStateService : ILibraryCurrentable
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly LibraryUOW _libraryUoW;

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
        }

        /// <inheritdoc></inheritdoc>
        public async Task<int> GetCount() => await Task.Run(() => GetBooks().Result.Count); // Количество книг

        /// <inheritdoc></inheritdoc>
        public async Task<Dictionary<string, string>> GetStatuses()
        {
            var bookStatuses = new Dictionary<string, string>{
                { "In library" , "В библиотеке" },
                { "Issued" , "Выдана"},
                { "Losted", "Утеряна"}};
            if (_httpAccessor.HttpContext.User.IsInRole("admin"))
            {
                return await Task.Run(() => bookStatuses);
            }
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
