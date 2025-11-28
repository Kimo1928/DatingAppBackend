using AutoMapper;
using DatingAppWebApi.DTOs;
using DatingAppWebApi.Entities;

namespace DatingAppWebApi.Mapping
{
    public class Mapper :Profile
    {
        public Mapper()
        {
            CreateMap<User, GetUserDTO>();
        }
    }
}
