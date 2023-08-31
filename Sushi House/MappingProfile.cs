using AutoMapper;
using Sushi_House.Models;

namespace Sushi_House
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDTO, User>().ReverseMap();
        } 
    }
}
