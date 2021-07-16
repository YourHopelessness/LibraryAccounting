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
                .ForMember(res => res.ReturningDate, m => m.MapFrom(r => r.ReturnDate));
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
                .ForMember(dest => dest.ReaderWorkEmail, m => m.MapFrom(src => src.WorkEmail))
                .ForMember(dest => dest.ReaderPersonalEmail, m => m.MapFrom(src => src.PersonalEmail));
            CreateMap<DbLogin, EmployeeLoginDto>()
                .ForMember(dest => dest.EmployeeUsername, m => m.MapFrom(src => src.UserName));
        }
    }
}
