using AutoMapper;
using FileManager.DTOs;
using FileManager.Models;

namespace FileManager.Configurations
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Credentials, LoginDTO>().ReverseMap();
        }
    }
}
