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

        /// <summary>
        /// Добавляемая книга
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public BookListModel NewBook { get; set; }

        /// <summary>
        /// Конутсруктор модели
        /// </summary>
        /// <param name="stateService"></param>
        /// <param name="mapperConfig"></param>
        public AddModel(ILibraryCurrentable stateService, IMapper mapperConfig)
        {
            _stateService = stateService;
            _configMapper = mapperConfig;
        }
        /// <summary>
        /// гет запрос
        /// </summary>
        public void OnGet() { }

        /// <summary>
        /// Отмена
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostCancel()
        {
            return RedirectToPage("../Index");
        }

        /// <summary>
        /// Добавление книги
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAddAsync()
        {
            ModelState.ClearValidationState(nameof(NewBook));
            if (!TryValidateModel(NewBook, nameof(NewBook)))
            {
                var resultsGroupedByMembers =
                    NewBook.Validate(new System.ComponentModel.DataAnnotations.ValidationContext(NewBook));
                foreach (var member in resultsGroupedByMembers)
                {
                    ModelState.AddModelError(
                        member.MemberNames.First(),
                        member.ErrorMessage);
                }
                return Page();
            }
            BooksDto book = _configMapper.Map<BooksDto>(NewBook);
            await _stateService.AddEditBook(book);
            return RedirectToPage("../Index");
        }
    }
}
