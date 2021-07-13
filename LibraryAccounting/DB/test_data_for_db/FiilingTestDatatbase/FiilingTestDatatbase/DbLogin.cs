using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.Entities
{
    [Table("auth")]
    public class DbLogin
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public Guid? EmployeeId { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public byte[] Salt { get; set; }
    }
}
