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
    /// ������ ������������� ��������
    /// </summary>
    public class AuthModel : PageModel
    {
        private readonly IAuthenticable _auth;
        private readonly IMapper _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// ������� ������
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public LoginModel Auth { get; set; }
        [TempData]
        public string ErrMessage { get; set; }

        /// <summary>
        /// ����������� �������� � DI
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="mapper"></param>
        public AuthModel(IAuthenticable auth, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _auth = auth;
            _config = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// �������� �������� �� ��������
        /// </summary>
        public void OnGet()
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                Redirect("/Index");
            }
            else
            {
                ErrMessage ??= "������� ����� ������������� �������";
            }
        }
        /// <summary>
        /// ���� � �������
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            EmployeeLoginDto employee = await _auth.EmployeeLoginInfo(Auth.UserName, Auth.Password); 
            if (employee != null)
            {
                await _auth.Autentificate(employee.EmployeeUsername, employee.EmployeeName, employee.Role, Convert.ToString(employee.EmployeeId)); // ��������������
                return RedirectToPage();
            }
            else
            {
                ErrMessage = "������� �������� ��������������� ������";
                return RedirectToPage();
            }
        }
        /// <summary>
        /// ����� �� ��������
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetLogoutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage();
        }
    }
}
