using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.Entities
{
    [Table("Reservations")]
    public class Reservations
    {
        [Required]
        public Guid ReaderId { get; set; }
        [Required]
        public Guid BookId { get; set; }
        [Required]
        public DateTime ReservationDate { get; set; }
        [Required]
        public DateTime ReturnDate { get; set; }
        [Required]
        public bool ReturningFlag { get; set; }
        public string Comment { get; set; }
    }
}
