using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LibraryAccounting.BL.Services;
using Microsoft.AspNetCore.Http;
using LibraryAccounting.Models;
using LibraryAccounting.BL.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace LibraryAccounting.Pages.BookActions
{
    /// <summary>
    /// Добавление книги
    /// </summary>
    [Authorize(Roles = "admin")]
    public class AddModel : PageModel
    {
        private readonly ILibraryCurrentable _stateService;
        private readonly IMapper _configMapper;

        [BindProperty(SupportsGet = true)]
        public BookListModel NewBook { get; set; }

        public AddModel(ILibraryCurrentable stateService, IMapper mapperConfig)
        {
            _stateService = stateService;
            _configMapper = mapperConfig;
        }
        public void OnGet()
        {

        }

        public IActionResult OnPostCancel()
        {
            return Redirect("/1/Title%20ASC");
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            if(NewBook.PublishedDate < DateTime.Now &&
               NewBook.Title.DefaultIfEmpty().Count() > 0 &&
               NewBook.Title.DefaultIfEmpty().Count() < 121 &
               NewBook.Author.DefaultIfEmpty().Count() > 0 &&
               NewBook.Author.DefaultIfEmpty().Count() < 121 &&
               NewBook.PublishedBy.DefaultIfEmpty().Count() > 0 &&
               NewBook.PublishedBy.DefaultIfEmpty().Count() < 121)
            {
                BooksDto book = _configMapper.Map<BooksDto>(NewBook);
                await _stateService.AddBook(book);
                return RedirectToPage("/1/Title%20ASC");
            }
            return Page();
        }
    }
}
