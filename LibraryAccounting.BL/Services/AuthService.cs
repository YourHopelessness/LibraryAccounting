using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LibraryAccounting.DAL.Repositories;
using LibraryAccounting.BL.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using LibraryAccounting.DAL.DB;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Linq.Expressions;
using LibraryAccounting.DAL.Entities;

namespace LibraryAccounting.BL.Services
{
    /// <summary>
    /// Интерфейс аутентификации
    /// </summary>
    public interface IAuthenticable
    {
        /// <summary>
        /// Получение аутентификационных данных пользователя
        /// </summary>
        /// <returns></returns>
        public Task<EmployeeLoginDto> EmployeeLoginInfo(string username, string password);
        /// <summary>
        /// Метод аутентификации пользователя
        /// </summary>
        /// <param name="username"></param>
        /// <param name="employeeName"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public Task Autentificate(string username, string employeeName, string role);
    }
    /// <summary>
    /// Сервис аутентификации
    /// </summary>
    public class AuthService : IAuthenticable
    {
        private readonly LibraryUOW _libraryUOW;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public AuthService(BaseLibraryContext libraryContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _libraryUOW = new LibraryUOW(libraryContext);
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        /// <inheritdoc></inheritdoc>
        public async Task<EmployeeLoginDto> EmployeeLoginInfo(string username, string password)
        {
            EmployeeLoginDto employee = null;
            var logins = _libraryUOW.Logins.Get(filter: l => l.Password == password && l.UserName == username);
            if (logins.ToList().Count != 0)
            {
                employee = _mapper.Map<EmployeeLoginDto>(logins.FirstOrDefault());
                int? roleId = _libraryUOW.UserRoles.Get(filter: e => e.EmployeeId == employee.EmployeeId).FirstOrDefault().RoleId;
                employee.Role = _libraryUOW.Roles.Get(filter: r => r.Id == roleId).FirstOrDefault().Name;
                var empl = _libraryUOW.Employees.Get(filter: e => e.Id == employee.EmployeeId).FirstOrDefault();
                employee.EmployeeName = empl.FirstName + " " + empl.LastName;
            }
            return employee;
        }
        /// <inheritdoc></inheritdoc>
        public async Task Autentificate(string username, string employeeName, string role)
        {
            // создаем один claim
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim("Name", employeeName),
                    new Claim(ClaimTypes.Role, role),
                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(2),
            };

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
}
