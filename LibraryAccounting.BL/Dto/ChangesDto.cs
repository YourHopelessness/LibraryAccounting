using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAccounting.BL.Dto
{
    public class ChangesDto
    {
        public Guid ChangemakerId { get; set; }
        public Guid BookId { get; set; }
        public string ChangemakerFullName { get; set; }
        public DateTime ChangeDate { get; set; }
        public string Comment { get; set; }
    }
}
