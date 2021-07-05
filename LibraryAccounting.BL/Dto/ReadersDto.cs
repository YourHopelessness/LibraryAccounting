using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAccounting.BL.Dto
{
    class ReadersDto
    {
        public Guid ReaderId { get; set; }
        public Guid BookId { get; set; }
        public string ReaderName { get; set; }
        public string ReaderWorkEmail { get; set; }
        public string ReaderPersonalEmail { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime ReturningDate { get; set; }
    }
}
