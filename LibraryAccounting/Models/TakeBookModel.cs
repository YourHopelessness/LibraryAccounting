using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.Models
{
    public class TakeBookModel
    {

        public TakeBookModel(List<BookListModel> booksList)
        {
            Books = booksList;
            ReturningTime = DateTime.Now;
            Comment = "";
        }

        [Required]
        [DisplayName("Список книг")]
        public IEnumerable<BookListModel> Books { get; set; }

        [Required]
        [DisplayName("Дата вовзращения книги")]
        public DateTime ReturningTime { get; set; }

        [DisplayName("Комментарий")]
        public string Comment { get; set; }
    }
}
