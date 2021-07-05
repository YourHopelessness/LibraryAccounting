using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.Entities
{
    [Table("books_statuses")]
    public class BooksStatuses
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
