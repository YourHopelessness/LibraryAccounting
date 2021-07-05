using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAccounting.DAL.Repositories;
using LibraryAccounting.DAL.Entities;
using LibraryAccounting.DAL.DB;
using AutoMapper;
using LibraryAccounting.BL.Dto;

namespace LibraryAccounting.BL.Services
{
    /// <summary>
    /// Интерфейс для сервиса показа книг
    /// </summary>
    public interface IBooksVisable
    {
        /// <summary>
        /// Получение списка имеющихся книг
        /// </summary>
        /// <param name="currentPage">Текущая отображаемая страница</param>
        /// <param name="pageSize"></param>
        /// <param name="field"></param>
        /// <returns>Список книг</returns>
        public Task<List<LibraryDTO>> GetBooks(int currentPage = 1, int pageSize = 10, string field = "");
        /// <summary>
        /// Получение длины массива книг
        /// </summary>
        /// <returns></returns>
        public Task<int> GetCount();
    }
    /// <summary>
    /// Сервис показа списка книг и работы с ним
    /// </summary>
    public class BooksListService : IBooksVisable
    {
        private readonly LibraryDbContext _context;

        private LibraryUOW libraryUoW;
        private List<LibraryDTO> library;
        /// <summary>
        /// Конструктор со внедрением зависимости контекста базы данных
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        public BooksListService(LibraryDbContext context)
        {
            _context = context;
            libraryUoW = new LibraryUOW(context);
        }
        /// <inherets></inherets>>
        public Task<int> GetCount() => Task.FromResult(library.Count);
        /// <inherets></inherets>>
        public Task<List<LibraryDTO>> GetBooks(int currentPage, int pageSize, string field)
        {
            /*MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Books, LibraryDTO>()
                   .ReverseMap();
                cfg.CreateMap<Changes, ChangesDto>();
                cfg.CreateMap<Employees, ChangesDto>()
                   .ForMember(dest => dest.ChangemakerFullName, m => m.MapFrom(src => src.FirstName + " " + src.LastName));
                cfg.CreateMap<Reservations, ReadersDto>();
                cfg.CreateMap<Employees, ReadersDto>()
                   .ForMember(dest => dest.ReaderName, m => m.MapFrom(src => src.FirstName + " " + src.LastName))
                   .ForMember(dest => dest.ReaderWorkEmail, m => m.MapFrom(src => src.WorkEmail))
                   .ForMember(dest => dest.ReaderPersonalEmail, m => m.MapFrom(src => src.PersonalEmail));
                cfg.CreateMap<ReadersDto, LibraryDTO>();
                cfg.CreateMap<ChangesDto, LibraryDTO>();

            });
            var mapper = new Mapper(mapperConfiguration);
            library = mapper.Map<List<Books>, List<LibraryDTO>>(libraryUoW.Books.Get(orderBy: bk => bk.OrderBy(b => b.Title)).ToList());
            var changesList = mapper.Map<List<Changes>, List<ChangesDto>>(libraryUoW.Changes.Get().ToList());
            changesList = mapper.Map(libraryUoW.Employees.Get(proretries: "changesList.Where()").ToList(), changesList);
            var readersList = mapper.Map<List<Reservations>, List<ReadersDto>>(libraryUoW.Reservations.Get().ToList());
            library = mapper.Map(changesList, library);
            library = mapper.Map(readersList, library);*/
            library = (from b in _context.Books
                           join st in _context.BooksStatuses on b.StatusId equals st.Id
                           join r in _context.Reservations on b.Id equals r.BookId into reserved
                            from rs in reserved.DefaultIfEmpty()
                           join er in _context.Employees on (rs == null ? Guid.Empty : rs.ReaderId) equals er.Id into readers
                           from rds in readers.DefaultIfEmpty()
                           join c in _context.Changes on b.Id equals c.BookId into changes
                            from ch in changes.DefaultIfEmpty()
                           join ec in _context.Employees on (ch == null ? Guid.Empty : ch.ChangemakerId) equals ec.Id into changemakers
                           from chms in changemakers.DefaultIfEmpty()
                           select new LibraryDTO
                           {
                                BookId = b.Id,
                                ISBN = b.ISBN,
                                Title = b.Title,
                                Author = b.Author,
                                PublishedBy = b.PublishedBy,
                                PublishedDate = b.PublishedDate.ToString("yyyy"),
                                Status = st.Status,
                                ReservationDate = rs.ReservationDate.ToString("d"),
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
