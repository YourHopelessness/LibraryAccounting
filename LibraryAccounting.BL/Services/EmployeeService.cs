using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LibraryAccounting.BL.Dto;
using LibraryAccounting.DAL.Repositories;
using LibraryAccounting.DAL.DB;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LibraryAccounting.BL.Services
{
    /// <summary>
    /// Интерфейс работы с сотрудниками
    /// </summary>
    public interface IEmployeesStatable
    {
        /// <summary>
        /// Получение текущего пользователя/читалеля
        /// </summary>
        /// <returns></returns>
        Task<ReadersDto> GetReader(Guid? readerId = null);

        /// <summary>
        /// Получение списка всех читателей
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ReadersDto>> GetAllReaders();
    }
    /// <summary>
    /// Сервис работы с пользователями
    /// </summary>
    public class EmployeeService : IEmployeesStatable
    {
        private readonly LibraryUOW _libraryUOW;
        private readonly IMapper _mapper;
        private readonly IAuthenticable _auth;

        /// <summary>
        /// Контсруктор класса
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        /// <param name="auth"></param>
        public EmployeeService(BaseLibraryContext context, 
                                IMapper mapper, 
                                IAuthenticable auth)
        {
            _libraryUOW = new LibraryUOW(context);
            _mapper = mapper;
            _auth = auth;
        }
        /// <inheritdoc></inheritdoc>>
        public async Task<ReadersDto> GetReader(Guid? readerId = null)
        {
            if (!readerId.HasValue) readerId = _auth.GetUserId();
            var reader = await _libraryUOW.Employees.Get(filter: e => e.Id == readerId);
            return _mapper.Map<ReadersDto>(reader.FirstOrDefault());
        }
        /// <inheritdoc></inheritdoc>>
        public async Task<IEnumerable<ReadersDto>> GetAllReaders()
        {
            var reader = await _libraryUOW.Employees.Get();
            return _mapper.Map<IEnumerable<ReadersDto>>(reader);
        }
    }
}
