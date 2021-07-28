using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.Models
{
    /// <summary>
    /// Модель для бронирования книги
    /// </summary>
    [Serializable]
    public class TakeBookModel : IValidatableObject
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
        /// дата возвращения
        /// </summary>
        [Required(ErrorMessage = "Дата возвращения не может быь пустой")]
        [DisplayName("Дата вовзращения книги")]
        public DateTime ReturningTime { get; set; } = DateTime.Now.AddDays(1);

        /// <summary>
        /// Комментарий
        /// </summary>
        [DisplayName("Комментарий")]
        public string Comment { get; set; } = "";

        /// <summary>
        /// Выполняет валидацию модели при заполнении пользователем
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ReturningTime <= DateTime.Now.AddDays(1))
            {
                yield return new ValidationResult(
                    $"Книга не может быть сдана раньше, чем завтра",
                    new[] { "InvalidReturningTime" });
            }
        }
    }
}
