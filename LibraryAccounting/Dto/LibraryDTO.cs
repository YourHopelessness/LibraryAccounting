using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.DTO
{
    public class LibraryDTO
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string PublishedBy { get; set; }
        public DateTime PublishedDate { get; set; }
        public int StatusId { get; set; }
        public DateTime ReturningDate {get; set;}
        public string Reader { get; set; }
        public string Comment { get; set; }
        public string Changemaker { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}
