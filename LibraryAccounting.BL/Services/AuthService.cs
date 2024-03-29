﻿using Microsoft.AspNetCore.Http;
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
using System.Security.Cryptography;

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
        Task<EmployeeLoginDto> EmployeeLoginInfo(string username, string password);
        /// <summary>
        /// Метод аутентификации пользователя
        /// </summary>
        /// <param name="username"></param>
        /// <param name="employeeName"></param>
        /// <param name="role"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Autentificate(string username, string employeeName, string role, string id);
        /// <summary>
        /// Получить текущего юзера сессии по id 
        /// </summary>
        /// <returns></returns>
        Guid GetUserId();
    }
    /// <summary>
    /// Сервис аутентификации
    /// </summary>
    public class AuthService : IAuthenticable
    {
        private readonly LibraryUOW _libraryUOW;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private byte[] globalSalt = Encoding.ASCII.GetBytes("Dear Math, grow up and solve your own problems");

        /// <summary>
        /// контсруктор
        /// </summary>
        /// <param name="libraryContext"></param>
        /// <param name="mapper"></param>
        /// <param name="httpContextAccessor"></param>
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
            var logins = await _libraryUOW.Logins.Get(filter: l => l.UserName == username);
            if (logins.Count() != 0)
            {
                var globalCrypt = new HMACSHA512(globalSalt);
                var decPass = globalCrypt.ComputeHash(Encoding.ASCII.GetBytes(password));
                var personalCrypt = new HMACSHA512(logins.First().Salt);
                var Pass = Convert.ToBase64String(personalCrypt.ComputeHash(decPass));
                if (logins.First().Password == Pass)
                {
                    employee = _mapper.Map<EmployeeLoginDto>(logins.FirstOrDefault());
                    int? roleId = (await _libraryUOW.UserRoles.Get(filter: e => e.EmployeeId == employee.EmployeeId)).FirstOrDefault().RoleId;
                    employee.Role = (await _libraryUOW.Roles.Get(filter: r => r.Id == roleId)).FirstOrDefault().Name;
                    var empl = (await _libraryUOW.Employees.Get(filter: e => e.Id == employee.EmployeeId)).FirstOrDefault();
                    employee.EmployeeName = empl.FirstName + " " + empl.LastName;
                }
            }
            return employee;
        }
        /// <inheritdoc></inheritdoc>
        public async Task Autentificate(string username, string employeeName, string role, string id)
        {
            // создаем один claim
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.NameIdentifier, id),
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


        /// <inheritdoc></inheritdoc>>
        public Guid GetUserId() => Guid.Parse(_httpContextAccessor.
            HttpContext.User.Claims.
            Where(c => c.Type == ClaimTypes.NameIdentifier).
            First().Value);
    }
}
