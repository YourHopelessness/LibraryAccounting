using System;
using System.ComponentModel;

namespace LibraryAccounting.BL.Dto
{
    /// <summary>
    /// Дто собирает данные с табцы изменений
    /// </summary>
    public class ChangesDto
    {
        /// идентификатор автора
        public Guid ChangemakerId { get; set; }

        /// идентификатор изменнной книги
        public Guid BookId { get; set; }

        /// автор изменений
        public string ChangemakerFullName { get; set; }

        /// дата измений
        public DateTime ChangeDate { get; set; }

        /// комментарий
        public string Comment { get; set; }
    }
}
