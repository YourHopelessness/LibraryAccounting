using System;
using System.ComponentModel;

namespace LibraryAccounting.Models
{
    /// <summary>
    /// Модель для отображения списка книг
    /// </summary>
    public class BookListModel
    {
        /// ид книги
        public Guid BookId { get; set; }

        /// код книги (может не быть)
        [DisplayName("Код ISBN")]
        public string ISBN { get; set; }

        /// название
        [DisplayName("Название")]
        public string Title { get; set; }

        /// автор
        [DisplayName("Автор")]
        public string Author { get; set; }

        /// издательство
        [DisplayName("Издательство")]
        public string PublishedBy { get; set; }

        /// год издания книги
        [DisplayName("Год издания")]
        public DateTime PublishedDate { get; set; }

        /// текущий статус книги
        [DisplayName("Текущий статус")]
        public string Status { get; set; }

        /// Id читателя
        public Guid? ReaderId { get; set; }

        /// имя читателя
        [DisplayName("Текущий читатель")]
        public string ReaderName { get; set; }

        /// дата выдачи
        [DisplayName("Дата выдачи")]
        public string ReservationDate { get; set; }

        ///дата сдачи
        [DisplayName("Планируемая дата сдачи")]
        public string ReturningDate { get; set; }

        /// Id читателя
        public Guid? ChangemakerId { get; set; }

        /// автор изменений
        [DisplayName("Автор изменений")]
        public string ChangemakerFullName { get; set; }

        /// дата измений
        [DisplayName("Дата последнего изменения")]
        public string ChangeDate { get; set; }

        /// комментарий
        [DisplayName("Комментарий")]
        public string Comment { get; set; }
    }
}
