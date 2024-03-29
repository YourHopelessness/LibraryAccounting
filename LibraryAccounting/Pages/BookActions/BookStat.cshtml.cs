using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LibraryAccounting.Models;
using LibraryAccounting.BL.Services;
using AutoMapper;
using LibraryAccounting.BL.Dto;
using System.Net;

namespace LibraryAccounting.Pages.BookActions
{
    /// <summary>
    /// ����� ��������� ���������� � �����
    /// </summary>
    [Authorize(Roles = "admin")]
    public class StatsModel : PageModel
    {
        private readonly ILibraryCurrentable _stateService;
        private readonly IReservable _reservationManager;
        private readonly IMapper _configMapper;

        /// <summary>
        /// ����������� ������
        /// </summary>
        public StatsModel(ILibraryCurrentable stateService, IReservable reservationManager, IMapper configMapper)
        {
            _stateService = stateService;
            _reservationManager = reservationManager;
            _configMapper = configMapper;
        }

        /// <summary>
        /// Id ������������ �����
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public Guid? Id { get; set; }
        
        /// <summary>
        /// �������� ���� �����
        /// </summary>
        public List<OwnedBooksModel> BookdInOwn { get; set; }

        /// <summary>
        /// ��� ������
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
            BookdInOwn = _configMapper.Map<List<OwnedBooksModel>>(_reservationManager.GetReservations(Id, ReservationsPeriod.allTime).Result);
            BookdInOwn.ForEach(b => b = _configMapper.Map(book, b));
            BookdInOwn = BookdInOwn.OrderByDescending(b => b.ReservationDate).ToList();
        }

        /// <summary>
        /// ������ �������� �����
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostDelete()
        {
            _stateService.DeleteBook(Id.GetValueOrDefault());
            return Redirect("../Index");
        }

        /// <summary>
        /// ������ ����� ������� ����� �� �������
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostLosted()
        {
            _stateService.LostBook(Id.GetValueOrDefault());
            return Redirect("../Index");
        }
    }
}
