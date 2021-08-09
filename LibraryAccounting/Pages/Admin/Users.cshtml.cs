using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using LibraryAccounting.BL;
using LibraryAccounting.Models;
using LibraryAccounting.BL.Services;
using AutoMapper;

namespace LibraryAccounting.Pages.Admin
{
    /// <summary>
    /// Модель страницы с пользователями
    /// </summary>
    [Authorize(Roles = "admin")]
    public class UserModel : PageModel
    {
        private readonly IEmployeesStatable _employee;
        private readonly IMapper _mapper;
        private readonly IReservable _reservation;

        /// <summary>
        /// Список пользователей
        /// </summary>
        public List<EmployeeInfoModel> Users { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public UserModel(IEmployeesStatable employee,
                         IMapper mapper,
                         IReservable reservation)
        {
            _employee = employee;
            _mapper = mapper;
            _reservation = reservation;
        }

        /// <summary>
        /// get
        /// </summary>
        public void OnGet()
        {
            Users = _mapper.Map(_employee.GetAllReaders().Result, Users).OrderBy(u => u.FullName).ToList();
            var allTimeReservations = _reservation.GetReservations(tagTime: ReservationsPeriod.allTime).Result;
            var currentReservations = _reservation.GetReservations().Result;
            Users.ForEach(u =>
            {
                u.CurrentReservationCount = currentReservations.Where(r => r.ReaderId == u.Id).Count(); // книги, которое хоть раз брались
                u.AllTimeReservationCount = allTimeReservations.Where(r => r.ReaderId == u.Id).Count(); // количетсво читаемых сейчас
                u.DelayedBooksCount = 
                            allTimeReservations.Where(r => r.ReaderId == u.Id &&
                                                      r.ReturningDate < DateTime.Now &&
                                                      !r.ReturnFlag).Count(); // количетсво просроченных
            });
        }
    }
}
