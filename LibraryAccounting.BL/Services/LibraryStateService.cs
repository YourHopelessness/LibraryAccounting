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
    /// Интерфейс для сервиса показа книг
    /// </summary>
    public interface ILibraryCurrentable
    {
        /// <summary>
        /// Получение списка имеющихся книг
        /// </summary>
        /// <param name="currentPage">Текущая отображаемая страница</param>
        /// <param name="pageSize"></param>
        /// <param name="field"></param>
        /// <returns>Список книг</returns>
        public Task<List<BooksDto>> GetBooks();
        /// <summary>
        /// Получение длины массива книг
        /// </summary>
        /// <returns></returns>
        public Task<int> GetCount();
    }

    /// <summary>Сервис показа списка книг и работы с ним</summary>
    public class LibraryStateService : ILibraryCurrentable
    {
        private IMapper _mapper;
        private LibraryUOW _libraryUoW;

        /// <summary>Конструктор со внедрением зависимости контекста базы данных </summary>
        /// <param name="context">Контекст базы данных</param>
        /// <param name="mapper">Карты для маппера</param>
        public LibraryStateService(BaseLibraryContext context, IMapper mapper)
        {
            _libraryUoW = new LibraryUOW(context);
            _mapper = mapper;
        }

        /// <inheritdoc></inheritdoc>
        public Task<int> GetCount() => Task.FromResult(GetBooks().Result.Count);

        /// <inheritdoc></inheritdoc>
        public Task<List<BooksDto>> GetBooks()
        {
            List<BooksDto> library = _mapper.Map<List<Books>, List<BooksDto>>(_libraryUoW.Books.Get().ToList());
            library.ForEach(l => l.Status = _libraryUoW.BooksStatuses.Get(filter: e => e.Id == Convert.ToInt32(l.Status)).FirstOrDefault().Status);
            return Task.FromResult(library);
        }
    }
}
