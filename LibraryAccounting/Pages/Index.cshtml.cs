using System;
using System.Collections.Generic;
using LibraryAccounting.BL.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using LibraryAccounting.BL.Services;
using LibraryAccounting.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Net;

namespace LibraryAccounting.Pages
{
    /// <summary>
    /// Класс - контроллер главной страницы
    /// </summary>
    [Authorize]
    public class IndexModel : PageModel
    {
        /// <summary>Список отображаемых на станице книг</summary>
        public ObservableCollection<BookListModel> BooksList { get; set; }

        private readonly ILibraryCurrentable _library;
        private readonly IReservable _reservations;
        private readonly IChangeble _changes;
        private readonly IMapper _config;


        ///<summary>Количетсво книг</summary>
        public int Count { get; set; }

        #region pagination
        ///<summary>Текущая страница</summary>
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; }

        ///<summary>Размер одной страницы</summary>
        public int PageSize { get; set; } = 10;

        ///<summary>Обзее количетсво страниц</summary>
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));

        ///<summary>Флаг возможности перехода на предыдуoую страницу</summary>
        public bool ShowPrevious => CurrentPage > 1;

        ///<summary>Флаг возможности перехода на следующую страницу</summary>
        public bool ShowNext => CurrentPage < TotalPages;

        ///<summary>Флаг возможности перехода на первую страницу</summary>
        public bool ShowFirst => CurrentPage != 1;

        ///<summary>Флаг возможности перехода на последнюю страницу</summary>
        public bool ShowLast => CurrentPage != TotalPages;
        #endregion

        #region sorting
        ///<summary>Поле сортировки</summary>
        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; }

        /// <summary>
        /// Порядок сортировки
        /// </summary>
        public string OrderBy { get; set; }
        #endregion
        /// <summary>Поиск по isbn коду</summary>
        [BindProperty(SupportsGet = true)]
        public SearchModel Search { get; set; }

        ///<inheritdoc></inheritdoc>
        public IndexModel(ILibraryCurrentable library,
                          IMapper mapper,
                          IChangeble changes,
                          IReservable reservations
            )
        {
            _library = library;
            _config = mapper;
            _changes = changes;
            _reservations = reservations;
            
        }

        /// <inheritdoc></inheritdoc>
        public async Task OnGetAsync()
        {
            var sortby = new String(SortBy.TakeWhile(s => s != ' ').ToArray());
            if (typeof(BookListModel).GetProperty(sortby.ToString()) == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                Response.Redirect("/Error/?error=404");
                return;
            }
            var f = Request.Headers["Referer"].ToString();
            var books = await _library.GetBooks();
            var reservations = await _reservations.GetReservations();
            var changes = await _changes.GetChanges();
            var bookList = _config.Map<List<BookListModel>>(books);
            reservations.ForEach(r => _config.Map(r, bookList.Find(b => b.BookId == r.BookId)));
            changes.ForEach(c => _config.Map(c, bookList.Find(b => b.BookId == c.BookId)));

            Count = await _library.GetCount();

            #region searching
            Search.Statuses = new SelectList(_library.GetStatuses().Result.Values);
            var list = bookList.Select(b => b);
            if (!string.IsNullOrEmpty(Search.SearchString))
            {
                list = list.Where(s => s.ISBN.Contains(Search.SearchString));
            }
            if (!string.IsNullOrEmpty(Search.Status))
            {
                list = list.Where(x => x.Status == Search.Status);
            }
            Search.ISBNList = new SelectList(bookList.Select(b => b.ISBN).Distinct());
            #endregion

            #region sorting

            list = list.AsQueryable().OrderBy(SortBy + OrderBy);
            BooksList = new ObservableCollection<BookListModel>(list.Skip((CurrentPage - 1) * PageSize).Take(PageSize));

            #endregion
        }
    }

}


