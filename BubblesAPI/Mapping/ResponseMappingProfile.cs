using AutoMapper;
using BubblesAPI.Database.Models;
using BubblesAPI.DTOs;

namespace BubblesAPI.Mapping
{
    public class ResponseMappingProfile : Profile
    {
        public ResponseMappingProfile()
        {
            this.CreateMap<User, LoginResponse>();
            this.CreateMap<BubblesEngine.Models.Database, DatabaseResponse>();
        }
    }
}