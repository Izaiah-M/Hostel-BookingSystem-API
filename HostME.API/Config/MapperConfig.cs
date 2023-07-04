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

            // User DTOs
            CreateMap<UserDTO, ApiUser>().ReverseMap();

            // Room DTOs
            CreateMap<RoomDTO, Room>().ReverseMap();

            // Manager DTOs
            CreateMap<ManagerDTO, HostelManager>().ReverseMap();

        }
    }
}
