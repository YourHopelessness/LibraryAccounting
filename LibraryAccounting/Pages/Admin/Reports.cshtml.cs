using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using LibraryAccounting.BL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LibraryAccounting.Models;

namespace LibraryAccounting.Pages.Admin
{
    /// <summary>
    /// Страница формирования отчетов
    /// </summary>
    [Authorize(Roles = "admin")]
    public class ReportsModel : PageModel
    {
        private readonly ILibraryCurrentable _library;
        private readonly IReservable _reservations;
        private readonly IEmployeesStatable _employees;

        /// <summary>
        /// Параметры отчета
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public ReportsSettingsModel ReportsSettings { get; set; }


        /// <inheritdoc></inheritdoc>
        public ReportsModel(ILibraryCurrentable library,
                            IReservable reservations,
                            IEmployeesStatable employees)
        {
            _library = library;
            _reservations = reservations;
            _employees = employees;
        }
        /// <summary>
        /// гет метод
        /// </summary>
        public void OnGet()
        {
            ReportsSettings = new();
            ReportsSettings.Propetries.Add(typeof(BookListModel).GetProperties().Select(x => x.ToString()).ToList());

        }

        public async Task<IActionResult> OnPostUpdateSettingsAsync()
        {
            return Redirect("as");
        }
    }
}
