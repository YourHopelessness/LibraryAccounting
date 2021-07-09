using System;

namespace LibraryAccounting.BL.Dto
{
    /// <summary>
    /// Информацие по книге
    /// </summary>
    public class BooksDto
    {
        /// ид книги
        public Guid BookId { get; set; }

        /// код книги (может не быть)
        public string ISBN { get; set; }

        /// название
        public string Title { get; set; }

        /// автор
        public string Author { get; set; }

        /// издательство
        public string PublishedBy { get; set; }

        /// год издания книги
        public DateTime? PublishedDate { get; set; }

        /// текущий статус книги
        public string Status { get; set; }
    }
}
