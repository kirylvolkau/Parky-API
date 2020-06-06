using AutoMapper;
using ParkyAPI.Models;
using ParkyAPI.Models.DTO;

namespace ParkyAPI.Mapper
{
    public class ParkyMappings : Profile
    {
        public ParkyMappings()
        {
            CreateMap<NationalPark, NationalParkDto>();
            CreateMap<NationalParkDto, NationalPark>();
        }
    }
}