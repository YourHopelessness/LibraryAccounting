using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibraryAccounting.Models
{
    /// <summary>
    /// Модель для отображения списка книг
    /// </summary>
    public class BookListModel : IValidatableObject
    {
        /// ид книги
        public Guid BookId { get; set; }

        /// код книги (может не быть)
        [DisplayName("Код ISBN")]
        [RegularExpression(@"(^$)|(^(978-|)(\d{10}|\d{9}-\d)|((978-)|)\d-\d{2}-\d{6}-\d$)", ErrorMessage = "Не соответствует правилам ISBN кодов")]
        [DefaultValue("")]
        public string ISBN { get; set; }

        /// название
        [DisplayName("Название")]
        [Required(ErrorMessage = "Название не может быь пустым")]
        [DefaultValue("")]
        public string Title { get; set; }

        /// автор
        [DisplayName("Автор")]
        [Required(ErrorMessage = "Поле автор не может быь пустым")]
        [DefaultValue("")]
        public string Author { get; set; }

        /// издательство
        [DisplayName("Издательство")]
        [Required(ErrorMessage = "Поле издательство не может быь пустым")]
        [DefaultValue("")]
        public string PublishedBy { get; set; }

        /// год издания книги
        [DisplayName("Год издания")]
        [Required(ErrorMessage = "Поле год издания не может быь пустым")]
        public DateTime? PublishedDate { get; set; }

        /// текущий статус книги
        [DisplayName("Текущий статус")]
        [DefaultValue("В библиотеке")]
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

        /// <summary>
        /// Выполняет валидацию модели при заполнении пользователем
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PublishedDate > DateTime.Now)
            {
                yield return new ValidationResult(
                    $"Книга должна быть опубликована не менее, чем за сутки от текущей даты",
                    new[] { "InvalidPublishedDate" });
            }
        }
    }
}
