using AutoMapper;
using PhotoZoneBooking.BLL.Dtos;
using PhotoZoneBooking.DAL.Entities;

namespace PhotoZoneBooking.BLL.Profiles;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<PhotoZone, PhotoZoneDto>()
            .ForMember(dest => dest.PhotoZoneDetailId, 
                t => t.MapFrom(src => src.PhotoZoneDetail.Id))
            .ForMember(dest => dest.EndProgram,
                t => t.MapFrom(src => src.PhotoZoneDetail.EndProgram))
            .ForMember(dest => dest.StartProgram,
                t => t.MapFrom(src => src.PhotoZoneDetail.StartProgram))
            .ForMember(dest => dest.Description,
                t => t.MapFrom(src => src.PhotoZoneDetail.Description))
            .ForMember(dest => dest.Address, t => t.MapFrom(src => src.PhotoZoneDetail.Address))
            .ReverseMap();
        CreateMap<Reservation, ReservationDto>();
        CreateMap<ReservationDto, Reservation>();
    }
}