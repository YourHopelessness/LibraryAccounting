using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LibraryAccounting.BL.Services;
using LibraryAccounting.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryAccounting.Pages.Account
{
    /// <summary>
    /// Класс для отображения личной библиотеке читателя
    /// </summary>
    [Authorize]
    public class PersonalBiblioModel : PageModel
    {
        private readonly ILibraryCurrentable _library;
        private readonly IReservable _reservations;
        private readonly IMapper _config;
        private readonly IEmployeesStatable _employees;
        /// <summary>
        /// Список книг
        /// </summary>
        public List<OwnedBooksModel> BooksList { get; set; } = new();

        /// <inheritdoc></inheritdoc>
        public PersonalBiblioModel(ILibraryCurrentable library,
                                    IMapper mapper,
                                    IReservable reservations,
                                    IEmployeesStatable employees)
        {
            _library = library;
            _config = mapper;
            _reservations = reservations;
            _employees = employees;
        }

        /// <inheritdoc></inheritdoc>
        public async Task OnGetAsync()
        {
            var reservations = await _reservations.GetReservations(tagTime: ReservationsPeriod.allTime);
            var user = await _employees.GetReader();
            reservations = reservations.Where(r => r.ReaderId == user.ReaderId).ToList();
            if (reservations.Count() != 0)
            {
                foreach(var reservation in reservations) 
                {
                    var book = await _library.GetBookById(reservation.BookId);
                    BooksList.Add(_config.Map<OwnedBooksModel>(book));
                    _config.Map(reservation, BooksList.Find(b => b.BookId == reservation.BookId));
                }
            }
        }
    }
}
