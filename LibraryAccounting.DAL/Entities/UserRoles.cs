using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.DAL.Entities
{
    /// <summary>
    /// таблица связей роли ей сотруднов
    /// </summary>
    [Table("user_roles")]
    public class UserRoles
    {
        /// <summary>
        /// ид сотрудника
        /// </summary>
        [Required]
        public Guid EmployeeId { get; set; }
        /// <summary>
        /// роль сотрудника
        /// </summary>
        [Required]
        public int? RoleId { get; set; }
    }
}
