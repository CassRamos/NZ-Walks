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
        }

    }
}
