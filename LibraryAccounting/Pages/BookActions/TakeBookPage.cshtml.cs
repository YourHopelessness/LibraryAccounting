using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using LibraryAccounting.BL.Dto;
using LibraryAccounting.BL.Services;
using LibraryAccounting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LibraryAccounting.Pages.BookActions
{
    /// <summary>
    /// Модель для бронирования книги
    /// </summary>
    public class TakeBookModelPage : PageModel
    {
        private readonly ILibraryCurrentable _stateService;
        private readonly IReservable _reservationManager;
        private readonly IMapper _configMapper;

        /// <summary>
        /// Книга для выдачи
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public TakeBookModel Book {get; set;}

        ///<summary>
        /// Флаг редактировалась ли форма ранее
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public bool? UntouchedForm { get; set; } = true;

        /// <summary>
        /// Id бронируемой книги
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public Guid? Id { get; set; }

        /// <inheritdoc></inheritdoc>
        public TakeBookModelPage(IReservable reservationManager,
                             IMapper configMapper,
                             ILibraryCurrentable stateService)
        {
            _reservationManager = reservationManager;
            _configMapper = configMapper;
            _stateService = stateService;
        }
        /// <summary>
        /// Гет запрос на отображение атрибутов книг на странице
        /// </summary>
        /// <returns></returns>
        public async Task OnGetAsync()
        {
            BooksDto book = await _stateService.GetBookById(Id.GetValueOrDefault());
            if (book == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                Response.Redirect($"/Error/?error={Response.StatusCode}");
                return;
            }
            Book = _configMapper.Map<TakeBookModel>(book);
            TempData["Book"] = JsonConvert.SerializeObject(Book);
        }

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
        public async Task<IActionResult> OnPostTakeAsync()
        {
            ModelState.ClearValidationState(nameof(Book));
            if (!TryValidateModel(Book, nameof(Book)))
            {
                var resultsGroupedByMembers =
                    Book.Validate(new System.ComponentModel.DataAnnotations.ValidationContext(Book));
                foreach (var member in resultsGroupedByMembers)
                {
                    ModelState.AddModelError(
                        member.MemberNames.First(),
                        member.ErrorMessage);
                }
                TempData["Book"] = JsonConvert.SerializeObject(Book);
                return RedirectToPage(new { id = Id, untouchedform = false});
            }
            else
            {
                await _reservationManager.ReserveBook(Id.GetValueOrDefault(), Book.ReturningTime);
                return RedirectToPage("../Index");
            }
        }
    }
}

