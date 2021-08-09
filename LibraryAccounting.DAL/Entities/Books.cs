using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.DAL.Entities
{
    /// <summary>
    /// Сущность книги
    /// </summary>
    [Table("books")]
    public class Books
    {
        /// <summary>
        /// Id книги
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// Код ISBN
        /// </summary>
        public string ISBN { get; set; }
        /// <summary>
        /// Тазвание книги
        /// </summary>
        [Required]
        public string Title { get; set; }
        /// <summary>
        /// Вытор книги
        /// </summary>
        [Required]
        public string Author { get; set; }
        /// <summary>
        /// Издательство
        /// </summary>
        [Required]
        public string PublishedBy { get; set; }
        /// <summary>
        /// Дата публикации
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }
        /// <summary>
        /// Текущий статус
        /// </summary>
        [Required]
        public int StatusId { get; set; }
    }
}
