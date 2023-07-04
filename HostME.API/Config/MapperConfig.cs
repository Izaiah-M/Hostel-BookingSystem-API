using AutoMapper;
using HostME.Core.DTOs;
using HostME.Data.Models;

namespace HostME.API.Config
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<HostelDTO, Hostel>().ReverseMap();

            CreateMap<GetHostelDTO, Hostel>().ReverseMap();

            CreateMap<UpdateHostelDTO, Hostel>().ReverseMap();

            CreateMap<UserDTO, ApiUser>().ReverseMap();

            CreateMap<RoomDTO, Room>().ReverseMap();

            CreateMap<ManagerDTO, HostelManager>().ReverseMap();

        }
    }
}
