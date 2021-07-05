using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.DAL.Entities
{
    [Table("changes")]
    public class Changes
    {
        [Required]
        public Guid ChangemakerId { get; set; }
        [Required]
        public Guid BookId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime? ChangeDate { get; set; }
        public string Comment { get; set;  }
    }
}
