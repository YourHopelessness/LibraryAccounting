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

namespace LibraryAccounting.Pages
{
    /// <summary>
    /// Класс - контроллер главной страницы
    /// </summary>
    [Authorize]
    public class IndexModel : PageModel
    {
        /// <summary>Список отображаемых на станице книг</summary>
        public List<BookListModel> BooksList { get; set; }

        private readonly ILibraryCurrentable _library;
        private readonly IReservable _reservations;
        private readonly IChangeble _changes;
        private readonly IMapper _config;
        private SortingModel<BookListModel> _sorting;


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

        public string OrderBy { get; set; }

        /// <summary>Поиск по isbn коду</summary>
        [BindProperty(SupportsGet = true)]
        public SearchModel Search { get; set; }
        #endregion


        ///<inheritdoc></inheritdoc>
        public IndexModel(ILibraryCurrentable library,
                          IMapper mapper,
                          IChangeble changes,
                          IReservable reservations)
        {
            _library = library;
            _config = mapper;
            _changes = changes;
            _reservations = reservations;
            _sorting = new SortingModel<BookListModel>();
            
        }

        /// <inheritdoc></inheritdoc>
        public async Task OnGetAsync()
        {
            var books = await _library.GetBooks();
            var reservations = await _reservations.GetReservations();
            var changes = await _changes.GetChanges();
            BooksList = _config.Map<List<BookListModel>>(books);
            reservations.ForEach(r => _config.Map(r, BooksList.Find(b => b.BookId == r.BookId)));
            changes.ForEach(c => _config.Map(c, BooksList.Find(b => b.BookId == c.BookId)));
            Count = await _library.GetCount();

            #region searching
            Search.Statuses = new SelectList(_library.GetStatuses().Result.Values);
            var list = BooksList.Select(b => b);
            if (!string.IsNullOrEmpty(Search.SearchString))
            {
                list = list.Where(s => s.ISBN.Contains(Search.SearchString));
            }
            if (!string.IsNullOrEmpty(Search.Status))
            {
                list = list.Where(x => x.Status == Search.Status);
            }
            Search.ISBNList = new SelectList(BooksList.Select(b => b.ISBN).Distinct());
            #endregion

            #region sorting

           /* var properties = new SortingModel<BookListModel>().SortedField;
            foreach (var f in _sorting.SortedField)
            {
                try
                {
                   ViewData[f] = (string)ViewData[f] == f || ViewData[f] == null ? f : (string)ViewData[f];
                }
                catch
                {
                    ViewData.Add(f, f);
                }
            }
            ViewData[SortBy] =  !properties.Contains(ViewData[SortBy]) ? 
                                (ViewData[SortBy] as string).SkipLast(5).ToString() : 
                                ViewData[SortBy] + " DESC";*/
             
           
           /* if (_sorting.Ascending)
            {
                list = list.AsQueryable().OrderBy(_sorting.SortedField + " ASC");
            }
            else
            {*/
                list = list.AsQueryable().OrderBy(SortBy + OrderBy);
            //}
            BooksList = list.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
            #endregion
        }

        public ActionResult TakeBook(Guid id)
        {
            TakeBookModel books = new TakeBookModel(BooksList);
            return Partial("_TakeBookPartial", books);
        }
    }

}


