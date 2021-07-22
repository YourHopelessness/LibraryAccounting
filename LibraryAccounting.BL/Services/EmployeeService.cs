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
        public Task<ReadersDto> GetReader();
    }
    public class EmployeeService : IEmployeesStatable
    {
        private readonly LibraryUOW _libraryUOW;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmployeeService(BaseLibraryContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _libraryUOW = new LibraryUOW(context);
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ReadersDto> GetReader()
        {
            Guid id = Guid.Parse(_httpContextAccessor.HttpContext.User.Claims.Where
                                        (c => c.Type == ClaimTypes.NameIdentifier).First().Value);
            return _mapper.Map<ReadersDto>((await _libraryUOW.Employees.Get(filter: e => e.Id == id)).FirstOrDefault());
        }
    }
}
