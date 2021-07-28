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
using System.Net;
using Newtonsoft.Json;

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

        ///<summary>
        /// Флаг редактировалась ли форма ранее
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public bool? UntouchedForm { get; set; } = true;

        /// <summary>
        /// Id редактируемой книги
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public Guid? Id { get; set; }

        /// <summary>
        /// Контсруктор модели страницы
        /// </summary>
        /// <param name="stateService"></param>
        /// <param name="mapperConfig"></param>
        public EditModel(ILibraryCurrentable stateService, IMapper mapperConfig)
        {
            _stateService = stateService;
            _configMapper = mapperConfig;
        }

        /// <summary>
        /// Нажата отмена
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostCancel()
        {
            return RedirectToPage("../Index");
        }

        /// <summary>
        /// Гет запрос на отображение редактируемой книги
        /// </summary>
        public void OnGet()
        {
            BooksDto book = _stateService.GetBookById(Id.GetValueOrDefault()).Result;
            if (book == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                Response.Redirect($"/Error/?error={Response.StatusCode}");
                return; 
            }
            Edit = new();
            Edit.BookId = Id.GetValueOrDefault();
            Edit = _configMapper.Map<BookListModel>(book);
            TempData["Edit"] = JsonConvert.SerializeObject(Edit);
        }

        /// <summary>
        /// Изменение книги
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostEditAsync()
        {
            ModelState.ClearValidationState(nameof(Edit));
            if (!TryValidateModel(Edit, nameof(Edit)))
            {
                var resultsGroupedByMembers =
                    Edit.Validate(new System.ComponentModel.DataAnnotations.ValidationContext(Edit));
                foreach (var member in resultsGroupedByMembers)
                {
                    ModelState.AddModelError(
                        member.MemberNames.First(),
                        member.ErrorMessage);
                }
                TempData["Edit"] = JsonConvert.SerializeObject(Edit);
                return RedirectToPage(new { id = Id, untouchedform = false });
            }
            BooksDto book = _configMapper.Map<BooksDto>(Edit);
            await _stateService.AddEditBook(book);
            return RedirectToPage("../Index");
        }
    }
}
