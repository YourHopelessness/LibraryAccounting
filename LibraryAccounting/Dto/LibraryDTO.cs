using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.DTO
{
    public class LibraryDTO
    {
        public Guid? BookId { get; set; }
        [DisplayName("Код ISBN")]
        public string ISBN { get; set; }
        [DisplayName("Название")]
        public string Title { get; set; }
        [DisplayName("Автор")]
        public string Author { get; set; }
        [DisplayName("Издательство")]
        public string PublishedBy { get; set; }
        [DisplayName("Год издания")]
        public string PublishedDate { get; set; }
        [DisplayName("Статус")]
        public string Status { get; set; }
        [DisplayName("Дата выдачи")]
        public string ReservationDate { get; set;}
        [DisplayName("Дата возврата")]
        public string ReturningDate {get; set;}
        [DisplayName("Читатель")]
        public string Reader { get; set; }
        [DisplayName("Комментарии")]
        public string Comment { get; set; }
        [DisplayName("Внес изменения")]
        public string Changemaker { get; set; }
        [DisplayName("Последнее изменение")]
        public string ChangeDate { get; set; }
    }
}
