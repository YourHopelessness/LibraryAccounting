using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace LibraryAccounting.Services
{
    public interface IBooksVisable
    {
        public Task<List<DTO.LibraryDTO>> GetBooks(int currentPage = 1, int pageSize = 10, string field = "");
        public Task<int> GetCount();
    }
    public class BooksListService : IBooksVisable
    {
        private readonly DB.LibraryDbContext _context;
        private List<DTO.LibraryDTO> library;
        public BooksListService(DB.LibraryDbContext context) => _context = context;
        public Task<int> GetCount() => Task.FromResult(library.Count);

        public Task<List<DTO.LibraryDTO>> GetBooks(int currentPage, int pageSize, string field)
        {
            library = (from b in _context.Books
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
                                BookId = b.Id,
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
            switch(field)
            {
                case "ISBN":
                    library = library.AsQueryable().OrderBy(b => b.ISBN).ToList();
                    break;
                case "Title":
                    library = library.AsQueryable().OrderBy(b => b.Title).ToList();
                    break;
                case "Author":
                    library = library.AsQueryable().OrderBy(b => b.Author).ToList();
                    break;
                case "PublishedBy":
                    library = library.AsQueryable().OrderBy(b => b.PublishedBy).ToList();
                    break;
                case "PublishedDate":
                    library = library.AsQueryable().OrderBy(b => b.PublishedDate).ToList();
                    break;
                case "Status":
                    library = library.AsQueryable().OrderBy(b => b.Status).ToList();
                    break;
                case "ReservationDate":
                    library = library.AsQueryable().OrderBy(b => b.ReservationDate).ToList();
                    break;
                case "ReturningDate":
                    library = library.AsQueryable().OrderBy(b => b.ReturningDate).ToList();
                    break;
                case "Reader":
                    library = library.AsQueryable().OrderBy(b => b.Reader).ToList();
                    break;
                case "Comment":
                    library = library.AsQueryable().OrderBy(b => b.Comment).ToList();
                    break;
                case "ChangeDate":
                    library = library.AsQueryable().OrderBy(b => b.ChangeDate).ToList();
                    break;
                case "Changemaker":
                    library = library.AsQueryable().OrderBy(b => b.Changemaker).ToList();
                    break;
                default:
                    library = library.AsQueryable().OrderBy(b => b.Title).ToList();
                    break;
            }
            return Task.FromResult(library.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList());
        }
    }
}
