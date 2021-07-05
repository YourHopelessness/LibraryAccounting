using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.Entities
{
    [Table("books")]
    public class Books
    {
        [Key]
        public Guid? Id { get; set; }
        public string ISBN { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string PublishedBy { get; set; }
        [Required]
        public DateTime PublishedDate { get; set; }
        [Required]
        public int StatusId { get; set; }
    }
}
