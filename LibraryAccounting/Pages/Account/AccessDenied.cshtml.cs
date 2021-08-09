using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryAccounting.Pages.Account
{
    /// <summary>
    /// Модель для отобржения отстутвия лдоступа к данному ресурсу у пользователя
    /// </summary>
    public class AccessDeniedModel : PageModel
    {
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string Message { get; set; } 

        /// <inheritdoc></inheritdoc>
        public void OnGet()
        {
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            Message = Response.StatusCode.ToString();
        }
    }
}
