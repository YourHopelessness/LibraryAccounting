using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.Entities
{
    [Table("BooksStatuses")]
    public class BooksStatuses
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
