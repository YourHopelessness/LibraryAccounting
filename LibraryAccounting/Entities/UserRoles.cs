using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.Entities
{
    [Table("user_roles")]
    public class UserRoles
    {
        [Required]
        public Guid EmployeeId { get; set; }
        [Required]
        public int RoleId { get; set; }
    }
}
