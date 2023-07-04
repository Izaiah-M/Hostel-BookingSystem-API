using AutoMapper;
using HostME.Core.DTOs;
using HostME.Data.Models;

namespace HostME.API.Config
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            // Hostel DTOs
            CreateMap<HostelDTO, Hostel>().ReverseMap();

            CreateMap<GetHostelDTO, Hostel>().ReverseMap();

            CreateMap<UpdateHostelDTO, Hostel>().ReverseMap();

            CreateMap<DeleteHostelDTO, Hostel>().ReverseMap();

            CreateMap<OneHostelDTO, Hostel>().ReverseMap();

            CreateMap<BookHostelDTO, Hostel>().ReverseMap();

            // User DTOs
            CreateMap<UserDTO, ApiUser>().ReverseMap();
            CreateMap<OneUserDTO, ApiUser>().ReverseMap();

            // Room DTOs
            CreateMap<RoomDTO, Room>().ReverseMap();

            CreateMap<AllRoomsDTO, Room>().ReverseMap();

            CreateMap<GetRoomDTO, Room>().ReverseMap();

            CreateMap<DeleteRoomDTO, Room>().ReverseMap();

            CreateMap<UpdateRoomDTO, Room>().ReverseMap();

            // Manager DTOs
            CreateMap<ManagerDTO, HostelManager>().ReverseMap();

            // Booking DTOs
            CreateMap<BookingDTO, Booking>().ReverseMap();

            CreateMap<AllBookingsDTO, Booking>().ReverseMap();

        }
    }
}
