using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LibraryAccounting.Models;
using LibraryAccounting.BL.Dto;
using LibraryAccounting.BL.Services;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace LibraryAccounting.Pages.BookActions
{
    /// <summary>
    /// Класс для редактирования книги
    /// </summary>
    [Authorize(Roles = "admin")]
    public class EditModel : PageModel
    {
        private readonly ILibraryCurrentable _stateService;
        private readonly IMapper _configMapper;
        /// <summary>
        /// Редктируемая книга
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public BookListModel Edit { get; set; }

        public EditModel(ILibraryCurrentable stateService, IMapper mapperConfig)
        {
            _stateService = stateService;
            _configMapper = mapperConfig;
        }
        public IActionResult OnPostCancel()
        {
            return Redirect("/1/Title%20ASC");
        }

        public void OnGet([FromQuery] Guid id)
        {
            if (id == Guid.Empty)
            {
                return; //TODO выдавать BadRequest
            }
            Edit = new();
            Edit.BookId = id;
            Edit = _configMapper.Map<BookListModel>(_stateService.GetBookById(id).Result);
        }
    }
}
