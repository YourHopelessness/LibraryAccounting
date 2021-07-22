using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.Models
{
    /// <summary>
    /// Модель для отображения книг в пользованиии
    /// </summary>
    public class OwnedBooksModel
    {
        /// ид книги
        public Guid BookId { get; set; }

        /// код книги (может не быть)
        [DisplayName("Код ISBN")]
        [DefaultValue("")]
        public string ISBN { get; set; }

        /// название
        [DisplayName("Название")]
        [DefaultValue("")]
        public string Title { get; set; }

        /// автор
        [DisplayName("Автор")]
        [DefaultValue("")]
        public string Author { get; set; }

        /// издательство
        [DisplayName("Издательство")]
        [DefaultValue("")]
        public string PublishedBy { get; set; }

        /// год издания книги
        [DisplayName("Год издания")]
        public string PublishedDate { get; set; }

        /// дата выдачи
        [DisplayName("Дата выдачи")]
        [DefaultValue("")]
        public string ReservationDate { get; set; }

        ///дата сдачи
        [DisplayName("Дата сдачи")]
        [DefaultValue("")]
        public string ReturningDate { get; set; }

        /// <summary>Цвет отображения статуса</summary>
        [DefaultValue("#7CFC00")]
        public string Colour { get; set; }

        /// <summary>Просрочка</summary>
        [DisplayName("Осталось дней")]
        [DefaultValue("Сдана")]
        public string Delay { get; set; }
    }
}
