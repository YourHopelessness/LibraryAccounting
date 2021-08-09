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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace LibraryAccounting.Pages.Account
{
    /// <summary>
    /// Модель представления страницы
    /// </summary>
    public class AuthModel : PageModel
    {
        private readonly IAuthenticable _auth;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Входные данные
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public LoginModel Auth { get; set; }

        /// <summary>
        /// Сообщение об ошибки
        /// </summary>
        [TempData]
        public string ErrMessage { get; set; }

        /// <summary>
        /// Конструктор страницы с DI
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="httpContextAccessor"></param>
        public AuthModel(IAuthenticable auth, IHttpContextAccessor httpContextAccessor)
        {
            _auth = auth;
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// ПРроверка, если пользователь авторизован, то перенаправлять на страницу личного кабинета
        /// </summary>
        public void OnGet()
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Account/AboutMe");
            }
            else
            {
                ErrMessage ??= "Войдите чтобы просматривать контент";
            }
        }
        /// <summary>
        /// Вход в аккаунт
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            EmployeeLoginDto employee = await _auth.EmployeeLoginInfo(Auth.UserName, Auth.Password); 
            if (employee != null)
            {
                await _auth.Autentificate(employee.EmployeeUsername, employee.EmployeeName, employee.Role, Convert.ToString(employee.EmployeeId)); // аутентификация
                return RedirectToPage("../Index");
            }
            else
            {
                ErrMessage = "Введены неверные регистрационные данные";
            }
            return RedirectToPage();
        }
    }
}
