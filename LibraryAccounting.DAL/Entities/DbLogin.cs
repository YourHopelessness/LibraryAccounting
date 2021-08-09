using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.DAL.Entities
{
    /// <summary>Таблица аутентификации</summary>
    [Table("auth")]
    public class DbLogin
    {
        /// <summary>Ник</summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>Идентификатор сотрудника</summary>
        [Required]
        public Guid EmployeeId { get; set; }

        /// <summary>Пароль</summary>
        [Required]
        public string Password { get; set; }

         /// <summary>Соль</summary>
        [Required]
        public byte[] Salt { get; set; }
    }
}
