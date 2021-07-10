using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryAccounting.BL.Dto;
using AutoMapper;
using LibraryAccounting.DAL.Entities;

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
                .ForMember(l => l.PublishedDate, m => m.MapFrom(b => b.PublishedDate.Date))
                .ReverseMap();
            CreateMap<BooksDto, BookListModel>()
                .ReverseMap();

            CreateMap<Employees, ChangesDto>()
                .ForMember(dest => dest.ChangemakerFullName, m => m.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForAllOtherMembers(opt => opt.Ignore());
                CreateMap<Changes, ChangesDto>()
                    .ReverseMap();

            CreateMap<Reservations, ReadersDto>()
                .ReverseMap();

            CreateMap<Employees, ReadersDto>()
                .ForMember(dest => dest.ReaderName, m => m.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.ReaderWorkEmail, m => m.MapFrom(src => src.WorkEmail))
                .ForMember(dest => dest.ReaderPersonalEmail, m => m.MapFrom(src => src.PersonalEmail));
            CreateMap<DbLogin, EmployeeLoginDto>()
                .ForMember(dest => dest.EmployeeUsername, m => m.MapFrom(src => src.UserName));
        }
    }
}
