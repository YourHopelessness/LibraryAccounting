using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAccounting.BL.Dto
{
    /// <summary>
    /// Информация по бронированию одной книги
    /// </summary>
    public class BookInReservationsDto
    {
        /// книга
        public Guid BookId { get; set; }

        /// сотрудник, взявший книгу
        public Guid ReaderId { get; set; }

        /// сотрудники, бравшие книгу
        public string ReaderName { get; set; }

        /// дата выдачи
        public DateTime ReservationDate { get; set; }

        ///дата сдачи, отстутвует, если книга утеряна
        public DateTime? ReturningDate { get; set; }
    }
}
