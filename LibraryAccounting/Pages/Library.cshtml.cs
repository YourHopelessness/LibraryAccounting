using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAccounting.Services;

namespace LibraryAccounting.Pages
{
    public class LibraryModel : PageModel
    {
        public ICollection<DTO.LibraryDTO> Library { get; set; }
        private readonly IBooksVisable _library;

        public LibraryModel(IBooksVisable library)
        {
            _library = library;
        }
        public void OnGet()
        {
            Library = _library.GetBooks().GetAwaiter().GetResult();
        }
    }
}