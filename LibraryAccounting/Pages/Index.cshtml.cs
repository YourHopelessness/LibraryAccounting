using System;
using System.Collections.Generic;
using LibraryAccounting.BL.Dto;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LibraryAccounting.Pages
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }

    public class IndexModel : PageModel
    {
        public ICollection<LibraryDTO> Library { get; set; }
        private readonly BL.Services.IBooksVisable _library;
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; }
        [BindProperty(SupportsGet = true)]
        public string OrderBy { get; set; }
        public int Count { get; set; }
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public bool ShowPrevious => CurrentPage > 1;
        public bool ShowNext => CurrentPage < TotalPages;
        public bool ShowFirst => CurrentPage != 1;
        public bool ShowLast => CurrentPage != TotalPages;

        [TempData]
        public string Result { get; set; }

        public IndexModel(BL.Services.IBooksVisable library)
        {
            _library = library;
        }

        public async Task OnGetAsync()
        {
            Library = await _library.GetBooks(CurrentPage, PageSize, SortBy);
            Count = await _library.GetCount();
        }

        public async Task<IActionResult> OnPostAddBookAsync()
        {
            throw new System.Exception();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostTakeBookAsync()
        {
            throw new System.Exception();
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostFilterByAsync()
        {
            throw new System.Exception();
            return RedirectToPage();
        }
    }

}


