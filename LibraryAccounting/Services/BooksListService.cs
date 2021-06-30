using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace LibraryAccounting.Services
{
    public interface IBooksVisable
    {
        public Task<List<DTO.LibraryDTO>> GetBooks();
    }
    public class BooksListService : IBooksVisable
    {
        private readonly DB.LibraryDbContext _context;
        public BooksListService(DB.LibraryDbContext context) => _context = context;

        public  Task<List<DTO.LibraryDTO>> GetBooks()
        {
            var library = (from b in _context.Books
                           join st in _context.BooksStatuses on b.StatusId equals st.Id
                           join r in _context.Reservations on b.Id equals r.BookId into reserved
                            from rs in reserved.DefaultIfEmpty()
                           join er in _context.Emloyees on (rs == null ? Guid.Empty : rs.ReaderId) equals er.Id into readers
                           from rds in readers.DefaultIfEmpty()
                           join c in _context.Changes on b.Id equals c.BookId into changes
                            from ch in changes.DefaultIfEmpty()
                           join ec in _context.Emloyees on (ch == null ? Guid.Empty : ch.ChangemakerId) equals ec.Id into changemakers
                           from chms in changemakers.DefaultIfEmpty()
                           select new DTO.LibraryDTO
                           {
                                ISBN = b.ISBN,
                                Title = b.Title,
                                Author = b.Author,
                                PublishedBy = b.PublishedBy,
                                PublishedDate = b.PublishedDate.Value.Year.ToString(),
                                Status = st.Status,
                                ReservationDate = rs.ReservationDate.Value.ToString("d"),
                                ReturningDate = rs.ReturnDate.ToString(),
                                Reader = $"{rds.FirstName} {rds.LastName}",
                                Comment = rs.Comment ?? "-",
                                ChangeDate = ch.ChangeDate.Value.ToString("d"),
                                Changemaker = $"{chms.FirstName} {chms.LastName}"
                           }).DefaultIfEmpty().ToList();
            library.Sort(delegate(DTO.LibraryDTO x, DTO.LibraryDTO y)
        {
                if (x.Title == null && y.Title == null) return 0;
                else if (x.Title == null) return -1;
                else if (y.Title == null) return 1;
                else return x.Title.CompareTo(y.Title);
            });
            return Task.FromResult(library);
        }
    }
}
