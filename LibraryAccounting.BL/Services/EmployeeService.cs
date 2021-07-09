using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LibraryAccounting.BL.Dto;
using LibraryAccounting.DAL.Repositories;
using LibraryAccounting.DAL.DB;

namespace LibraryAccounting.BL.Services
{
    /// <summary>
    /// Интерфейс работы с сотрудниками
    /// </summary>
    public interface IEmployeesStatable
    {
    }
    public class EmployeeService : IEmployeesStatable
    {
        private readonly LibraryUOW _libraryUOW;
        private readonly IMapper _mapper;

        public EmployeeService(BaseLibraryContext libraryContext, IMapper mapper)
        {
            _libraryUOW = new LibraryUOW(libraryContext);
            _mapper = mapper;
        }
    }
}
