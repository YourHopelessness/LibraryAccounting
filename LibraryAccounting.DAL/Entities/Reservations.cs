using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.DAL.Entities
{
    /// <summary>бронирование </summary>
    [Table("reservations")]
    public class Reservations
    {
        /// <summary>читатель </summary>
        [Required]
        public Guid ReaderId { get; set; }

        /// <summary>книга </summary>
        [Required]
        public Guid BookId { get; set; }

        /// <summary>время брони </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime ReservationDate { get; set; }

        /// <summary>время вовзращения </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }

        /// <summary>флаг возвращения </summary>
        [Required]
        public bool? ReturningFlag { get; set; }

        /// <summary>комментарий к брони </summary>
        public string Comment { get; set; }
    }
}
