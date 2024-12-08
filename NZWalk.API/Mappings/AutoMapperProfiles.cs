using AutoMapper;
using NZWalk.API.Models.Domain;
using NZWalk.API.Models.DTO;

namespace NZWalk.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Region, RegionResponseDTO>().ReverseMap();
            CreateMap<CreateRegionRequestDTO, Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDTO, Region>().ReverseMap();

            CreateMap<CreateWalkRequestDTO, Walk>().ReverseMap();
            CreateMap<Walk, WalkResponseDTO>()
                .ForMember(dest => dest.DifficultyResponse, opt => opt.MapFrom(src => src.Difficulty))
                .ForMember(dest => dest.RegionResponse, opt => opt.MapFrom(src => src.Region))
                .ReverseMap();
            CreateMap<UpdateWalkRequestDTO, Walk>().ReverseMap();


            CreateMap<Difficulty, DifficultyResponseDTO>().ReverseMap();
        }

    }
}
