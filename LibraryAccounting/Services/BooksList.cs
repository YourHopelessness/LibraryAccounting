using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryAccounting.Services
{
    public interface IBooksVisable
    {
        public Task<List<DTO.LibraryDTO>> GetBooks();
    }
    public class BooksList : IBooksVisable
    {
        private DB.LibraryDbContext _context;
        public BooksList(DB.LibraryDbContext context)
        {
            _context = context;
        }

        public Task<List<DTO.LibraryDTO>> GetBooks()
        {
            List<DTO.LibraryDTO> library = new List<DTO.LibraryDTO>();
            var books = _context.Books.Select(b => b).ToList();
            foreach(var book in books)
            {
                DTO.LibraryDTO bookView = new DTO.LibraryDTO();
                bookView.ISBN = book.ISBN;
                bookView.Title = book.Title;
                bookView.Author = book.Author;
                bookView.PublishedBy = book.PublishedBy;
                bookView.PublishedDate = book.PublishedDate;
                library.Add(bookView);
            }

            return Task.FromResult(library);
        }
    }
}
