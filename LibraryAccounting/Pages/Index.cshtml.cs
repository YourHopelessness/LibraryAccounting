using System.Collections.Generic;
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
        public ICollection<DTO.LibraryDTO> Library { get; set; }
        private readonly Services.IBooksVisable _library;

        public IndexModel(Services.IBooksVisable library)
        {
            _library = library;
        }

        [BindProperty]
        public Message Message { get; set; }

        public IReadOnlyList<Message> Messages { get; private set; }

        [TempData]
        public string Result { get; set; }

        public async Task OnGetAsync()
        {
            Library = await _library.GetBooks();
        }

        public async Task<IActionResult> OnPostAddMessageAsync()
        {
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostTakeBookAsync()
        {
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostFilterByAsync()
        {
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAllMessagesAsync()
        {
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteMessageAsync(int id)
        {
            

            return RedirectToPage();
        }
    }

}


