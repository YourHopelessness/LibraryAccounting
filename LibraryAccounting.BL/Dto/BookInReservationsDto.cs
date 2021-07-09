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
    class BookInReservationsDto
    {
        /// книга
        public BooksDto Book { get; set; }

        /// сотрудники, бравшие книгу
        public ReadersDto Readers { get; set; }

        /// дата выдачи
        public DateTime ReservationDate { get; set; }

        ///дата сдачи, отстутвует, если книга утеряна
        public DateTime? ReturningDate { get; set; }
    }
}
