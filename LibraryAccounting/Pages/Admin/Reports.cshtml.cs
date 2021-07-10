using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryAccounting.Pages.Admin
{
    [Authorize(Roles = "admin")]
    public class ReportsModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}