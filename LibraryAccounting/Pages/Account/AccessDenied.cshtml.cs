using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryAccounting.Pages.Account
{
    /// <summary>
    /// ћодель дл€ отобржени€ отстутви€ лдоступа к данному ресурсу у пользовател€
    /// </summary>
    public class AccessDeniedModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string Message { get; set; }

        /// <inheritdoc></inheritdoc>
        public AccessDeniedModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc></inheritdoc>
        public async Task OnGetAsync()
        {
            _httpContextAccessor.HttpContext.Response.StatusCode = 403;
            Message = _httpContextAccessor.HttpContext.Response.StatusCode.ToString();
        }
    }
}
