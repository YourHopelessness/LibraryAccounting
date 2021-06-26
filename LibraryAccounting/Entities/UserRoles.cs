using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.Entities
{
    [Table("UserRoles")]
    public class UserRoles
    {
        [Required]
        public Guid EmployeeId { get; set; }
        [Required]
        public Guid RoleId { get; set; }
    }
}
