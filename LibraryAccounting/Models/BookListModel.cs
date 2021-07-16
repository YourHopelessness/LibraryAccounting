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
        public DateTime? PublishedDate { get; set; }

        /// текущий статус книги
        [DisplayName("Текущий статус")]
        [DefaultValue("")]
        public string Status { get; set; }

        /// Id читателя
        public Guid? ReaderId { get; set; }

        /// имя читателя
        [DisplayName("Текущий читатель")]
        [DefaultValue("")]
        public string ReaderName { get; set; }

        /// дата выдачи
        [DisplayName("Дата выдачи")]
        [DefaultValue("")]
        public string ReservationDate { get; set; }

        ///дата сдачи
        [DisplayName("Планируемая дата сдачи")]
        [DefaultValue("")]
        public string ReturningDate { get; set; }

        /// Id читателя
        public Guid? ChangemakerId { get; set; }

        /// автор изменений
        [DisplayName("Автор изменений")]
        [DefaultValue("")]
        public string ChangemakerFullName { get; set; }

        /// дата измений
        [DisplayName("Дата последнего изменения")]
        [DefaultValue("")]
        public string ChangeDate { get; set; }

        /// комментарий
        [DisplayName("Комментарий")]
        [DefaultValue("")]
        public string Comment { get; set; }
    }
}
