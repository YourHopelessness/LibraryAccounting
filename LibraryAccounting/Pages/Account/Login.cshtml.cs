using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LibraryAccounting.BL.Services;
using LibraryAccounting.Models;
using LibraryAccounting.BL.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace LibraryAccounting.Pages.Account
{
    /// <summary>
    /// Модель представления страницы
    /// </summary>
    public class AuthModel : PageModel
    {
        private readonly IAuthenticable _auth;
        private readonly IMapper _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Входные данные
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public LoginModel Auth { get; set; }

        [TempData]
        public string ErrMessage {get; set;}

        /// <summary>
        /// Конструктор страницы с DI
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="mapper"></param>
        public AuthModel(IAuthenticable auth, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _auth = auth;
            _config = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public void OnGet()
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                Redirect("/Index");
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            EmployeeLoginDto employee = await _auth.EmployeeLoginInfo(Auth.UserName, Auth.Password); 
            if (employee != null)
            {
                await _auth.Autentificate(employee.EmployeeUsername, employee.EmployeeName, employee.Role); // аутентификация
                return RedirectToPage("/Index");
            }
            else
            {
                ErrMessage = "Введены неверные регистрационные данные";
                ModelState.AddModelError("Error", "Введены неверные регистрационные данные");
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> Logout()
        {
            //await _httpContextAccessor.HttpContext.Session.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage();
        }
    }
}
