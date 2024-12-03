using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalk.API.Data;
using NZWalk.API.Models.Domain;
using NZWalk.API.Models.DTO;
using NZWalk.API.Repositories.Interfaces;

namespace NZWalk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;

        public RegionsController(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            var regionsDomain = await _regionRepository.GetAllAsync();

            var regionsDTO = new List<RegionResponseDTO>();

            foreach (var region in regionsDomain)
            {
                regionsDTO.Add(new RegionResponseDTO()
                {
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl,
                });
            }

            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            var regionDomain = await _regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            var regionDTO = new RegionResponseDTO()
            {
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };

            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRegion([FromBody] CreateRegionRequestDTO requestDTO)
        {
            var regionDomain = new Region
            {
                Code = requestDTO.Code,
                Name = requestDTO.Name,
                RegionImageUrl = requestDTO.RegionImageUrl,
            };

            await _regionRepository.CreateAsync(regionDomain);

            var regionDTO = new RegionResponseDTO
            {
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };

            return CreatedAtAction(nameof(GetRegionById), new { id = regionDomain.Id }, regionDTO);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateRegion(Guid id, [FromBody] UpdateRegionRequestDTO updateRequestDTO)
        {

            //map to domain model
            var regionDomain = new Region
            {
                Code = updateRequestDTO.Code,
                Name = updateRequestDTO.Name,
                RegionImageUrl = updateRequestDTO.RegionImageUrl,
            };

            regionDomain = await _regionRepository.UpdateAsync(id, regionDomain);

            if (regionDomain == null)
            {
                return NotFound();
            }

            var responseDTO = new RegionResponseDTO
            {
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };

            return Ok(responseDTO);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteRegion(Guid id)
        {
            var regionDomain = await _regionRepository.DeleteAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}