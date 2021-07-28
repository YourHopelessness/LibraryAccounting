using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.Models
{
    /// <summary>
    /// Модель для отображения книг в пользованиии
    /// </summary>
    public class OwnedBooksModel
    {
        /// <summary>
        /// ID Книги
        /// </summary>
        public Guid BookId { get; set; }

        /// <summary>
        /// ISBN код кнги (может быть пустым)
        /// </summary>
        [DisplayName("Код ISBN")]
        [DefaultValue("")]
        public string ISBN { get; set; }

        /// <summary>
        /// Название книги
        /// </summary>
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

        /// <summary>
        /// Год издания книги
        /// </summary>
        [DisplayName("Год издания")]
        public string PublishedDate { get; set; }

        /// <summary>
        /// Читатель
        /// </summary>
        [DisplayName("Читатель")]
        public string ReaderName { get; set; }

        /// <summary>
        /// Дата выдачи
        /// </summary>
        [DisplayName("Дата выдачи")]
        [DefaultValue("")]
        public string ReservationDate { get; set; }

        /// <summary>
        /// Дата возвращения
        /// </summary>
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
