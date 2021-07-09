using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LibraryAccounting.BL;

namespace LibraryAccounting.Pages.Admin
{
    [Authorize(Roles = "admin")]
    public class UserModel : PageModel
    {
       // public List<Employees> emls { get; private set; }

        public void OnGet()
        {
            
        }
    }
}
