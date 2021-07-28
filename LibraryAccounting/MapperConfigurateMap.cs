using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryAccounting.BL.Dto;
using AutoMapper;
using LibraryAccounting.DAL.Entities;
using LibraryAccounting.Models;

namespace LibraryAccounting
{
    /// <summary> Класс, хранящий карты сопоставления для маппера </summary>
    public class MapperConfigurateMap : Profile
    {
        /// <summary>
        /// Конфигурация карт для маппера
        /// </summary>
        /// <returns></returns>
        public MapperConfigurateMap()
        {
            CreateMap<Books, BooksDto>()
                .ForMember(l => l.BookId, m => m.MapFrom(b => b.Id))
                .ForMember(l => l.Status, m => m.MapFrom(b => b.StatusId.ToString()))
                .ForMember(l => l.PublishedDate, m => m.MapFrom(b => b.PublishedDate))
                .ReverseMap();
            CreateMap<BookListModel, BooksDto>()
                .ForMember(l => l.BookId, m => m.MapFrom(b => b.BookId == Guid.Empty ? Guid.NewGuid() : b.BookId))
                .ReverseMap();
            CreateMap<TakeBookModel, BooksDto>()
                .ReverseMap()
                .ForMember(l => l.PublishedDate, m => m.MapFrom(b => b.PublishedDate.Value.ToString("dd.MM.yyyy")));


            CreateMap<Changes, ChangesDto>()
                    .ReverseMap();
            CreateMap<Employees, ChangesDto>()
                .ForMember(dest => dest.ChangemakerFullName, m => m.MapFrom(src => src.FirstName + " " + src.LastName))
                .ReverseMap();
            CreateMap<ChangesDto, BookListModel>()
                .ForMember(dest => dest.ChangemakerFullName, m => m.MapFrom(src => src.ChangemakerFullName))
                .ForMember(l => l.ChangeDate, m => m.MapFrom(b => b.ChangeDate.ToString("dd.MM.yyyy")))
                .ForMember(l => l.Comment, m => m.MapFrom(b => b.Comment))
                .ForAllOtherMembers(m => m.Ignore());

            CreateMap<Reservations, BookInReservationsDto> ()
                .ForMember(res => res.ReturningDate, m => m.MapFrom(r => r.ReturnDate))
                .ForMember(res => res.ReturnFlag, m => m.MapFrom(r => r.ReturningFlag))
                .ReverseMap();
            CreateMap<Employees, BookInReservationsDto>()
                .ForMember(dest => dest.ReaderName, m => m.MapFrom(src => src.FirstName + " " + src.LastName))
                .ReverseMap();
            CreateMap<BookInReservationsDto, BookListModel>()
                .ForMember(dest => dest.ReaderName, m => m.MapFrom(src => src.ReaderName))
                .ForMember(l => l.ReservationDate, m => m.MapFrom(b => b.ReservationDate.ToString("dd.MM.yyyy")))
                .ForMember(l => l.ReturningDate, map => map.MapFrom(b => b.ReturningDate.GetValueOrDefault().ToString("dd.MM.yyyy")))
                .ForAllOtherMembers(m => m.Ignore());

            CreateMap<Employees, ReadersDto>()
                .ForMember(dest => dest.ReaderName, m => m.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.ReaderId, m => m.MapFrom(src => src.Id))
                .ForMember(dest => dest.ReaderWorkEmail, m => m.MapFrom(src => src.WorkEmail))
                .ForMember(dest => dest.ReaderPersonalEmail, m => m.MapFrom(src => src.PersonalEmail))
                .ForMember(dest => dest.ReaderPhone, m => m.MapFrom(src => src.PhoneNumber));
            CreateMap<DbLogin, EmployeeLoginDto>()
                .ForMember(dest => dest.EmployeeUsername, m => m.MapFrom(src => src.UserName));

            CreateMap<ReadersDto, EmployeeInfoModel>()
                .ForMember(dest => dest.FullName, m => m.MapFrom(src => src.ReaderName))
                .ForMember(dest => dest.WorkEmail, m => m.MapFrom(src => src.ReaderWorkEmail))
                .ForMember(dest => dest.PersonalEmail, m => m.MapFrom(src => src.ReaderPersonalEmail))
                .ForMember(dest => dest.PersonalPhone, m => m.MapFrom(src => src.ReaderPhone));

            CreateMap<BooksDto, OwnedBooksModel>()
                .ForMember(l => l.PublishedDate, m => m.MapFrom(b => b.PublishedDate.Value.Year.ToString()));
            CreateMap<BookInReservationsDto, OwnedBooksModel>()
                .ForMember(dest => dest.ReaderName, m => m.MapFrom(src => src.ReaderName))
                .ForMember(l => l.ReservationDate, m => m.MapFrom(b => b.ReservationDate.ToString("dd.MM.yyyy")))
                .ForMember(l => l.ReturningDate, map => map.MapFrom(b => b.ReturningDate.GetValueOrDefault().ToString("dd.MM.yyyy")))
                .ForMember(l => l.Delay, map => map.MapFrom(b => 
                    b.ReturningDate < DateTime.Today && !b.ReturnFlag ? 
                            $"Просрочено на {( DateTime.Today - b.ReturningDate).Value.Days}" :
                    !b.ReturnFlag ? 
                            $"Осталось дней {(b.ReturningDate - DateTime.Today).Value.Days}" : 
                            "Сдана"))
                .ForMember(l => l.Colour, map => map.MapFrom(b => 
                    b.ReturningDate < DateTime.Today && !b.ReturnFlag ? "#FF6347" :
                    !b.ReturnFlag ? "#FFD700" : "#7CFC00"))
                .ForAllOtherMembers(m => m.Ignore());
        }
    }
}
