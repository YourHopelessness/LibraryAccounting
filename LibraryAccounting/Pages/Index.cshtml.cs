using System;
using System.Collections.Generic;
using LibraryAccounting.BL.Dto;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace LibraryAccounting.Pages
{
    /// <summary>
    /// Класс - контроллер главной страницы
    /// </summary>
    [Authorize]
    public class IndexModel : PageModel
    {
        /// <summary>Список отображаемых на станице книг</summary>
        public IEnumerable<BookListModel> BooksList { get; set; }

        private readonly BL.Services.ILibraryCurrentable _library;
        private readonly IMapper _config;

        ///<summary>Текущая страница</summary>
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; }

        ///<summary>Поле сортировки</summary>
        [BindProperty(SupportsGet = true)]
        public string OrderBy { get; set; }

        ///<summary>Поле сортировки</summary>
        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; }
        ///<summary>Количетсво книг</summary>
        public int Count { get; set; }

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
        ///<inheritdoc></inheritdoc>
        public IndexModel(BL.Services.ILibraryCurrentable library, IMapper mapper)
        {
            _library = library;
            _config = mapper;
        }

        /// <inheritdoc></inheritdoc>
        public async Task OnGetAsync()
        {
            var books = await _library.GetBooks();
            BooksList = _config.Map<IEnumerable<BookListModel>>(books);
            Count = await _library.GetCount();
        }

        /// <summary> Пост запрос на добавление книги </summary>
        public async Task<IActionResult> OnPostAddBookAsync()
        {
            throw new System.Exception();
            return RedirectToPage();
        }

        /// <summary> Пост запрос на бронирование книги </summary>
        public async Task<IActionResult> OnPostTakeBookAsync()
        {
            throw new System.Exception();
            return RedirectToPage();
        }

        /// <summary> Пост запрос на фильтрацию книг </summary>
        public async Task<IActionResult> OnPostFilterByAsync()
        {
            throw new System.Exception();
            return RedirectToPage();
        }
    }

}


