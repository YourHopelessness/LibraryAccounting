using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.DAL.Entities
{
    /// <summary>
    /// Сущность справочника статусов книг
    /// </summary>
    [Table("books_statuses")]
    public class BooksStatuses
    {
        /// <summary>
        /// Id для связи по ключам
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// название статуса
        /// </summary>
        [Required]
        public string Status { get; set; }
    }
}
