using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryAccounting.Pages.Admin
{
    /// <summary>
    /// Статистика по книгам
    /// </summary>
    [Authorize(Roles = "admin")]
    public class StatsModel: PageModel
    {
        /// <summary>
        /// гет метод
        /// </summary>
        public void OnGet()
        {
           
        }
    }
}
