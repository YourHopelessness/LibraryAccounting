using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LibraryAccounting.BL.Services;
using LibraryAccounting.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace LibraryAccounting.Pages.Account
{
    /// <summary>
    /// КЛасс-модель, обработывающий страницу обо мне
    /// </summary>
    [Authorize]
    public class AboutMeModel : PageModel
    {
        readonly IMapper _mapper;
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly IEmployeesStatable _employees;
        /// <summary>
        /// информация о сотруднике
        /// </summary>
        public EmployeeInfoModel EmployeeInfo { get; set; }

        /// <summary>
        /// Контсруктор
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="employees"></param>
        public AboutMeModel(IMapper mapper, IHttpContextAccessor httpContextAccessor, IEmployeesStatable employees)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _employees = employees;
        }
        /// <summary>
        /// гет
        /// </summary>
        /// <returns></returns>
        public async Task OnGetAsync()
        {
            if (ModelState.IsValid)
            {
                EmployeeInfo = _mapper.Map<EmployeeInfoModel>(await _employees.GetReader());
            }
        }
        /// <summary>
        /// Выход из аккаунта
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetLogoutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Account/Login");
        }
    }
}
