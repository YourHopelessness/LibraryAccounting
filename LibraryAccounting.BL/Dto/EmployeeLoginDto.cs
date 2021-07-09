using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAccounting.BL.Dto
{
    /// <summary>Дто содержащее данные аутентификации и роли пользователя</summary>
    public class EmployeeLoginDto
    {
        /// <summary>идентификатор читателя</summary>
        public Guid EmployeeId { get; set; }

        /// <summary>Ник сотрудника</summary>
        public string EmployeeUsername { get; set; }

        /// <summary>Имя сотрудника</summary>
        public string EmployeeName { get; set; }

        /// <summary>Пароль</summary>
        public string Password { get; set; }

        /// <summary>Персональная соль</summary>
        public string Salt { get; set; }

        ///<summary>Роль сотрудника</summary>
        public string Role { get; set; }
    }
}
