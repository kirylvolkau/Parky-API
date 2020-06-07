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
            CreateMap<Trail, TrailDto>();
            CreateMap<TrailDto, Trail>();
            CreateMap<TrailUpdateDto, Trail>();
            CreateMap<Trail, TrailUpdateDto>();
            
            CreateMap<TrailCreateDto, Trail>();
            CreateMap<Trail, TrailCreateDto>();
        }
    }
}