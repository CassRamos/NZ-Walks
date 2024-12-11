using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalk.API.CustomActionsFilter;
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
        private readonly IMapper _mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAllRegions()
        {
            var regionsDomain = await _regionRepository.GetAllAsync();

            var regionsDTO = _mapper.Map<List<RegionResponseDTO>>(regionsDomain);

            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            var regionDomain = await _regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            var regionDTO = _mapper.Map<RegionResponseDTO>(regionDomain);

            return Ok(regionDTO);
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateRegion([FromBody] CreateRegionRequestDTO requestDTO)
        {
            var regionDomain = _mapper.Map<Region>(requestDTO);

            await _regionRepository.CreateAsync(regionDomain);

            var regionDTO = _mapper.Map<RegionResponseDTO>(regionDomain);

            return CreatedAtAction(nameof(GetRegionById), new { id = regionDomain.Id }, regionDTO);

        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRequestDTO)
        {
            //map to domain model
            var regionDomain = _mapper.Map<Region>(updateRequestDTO);

            regionDomain = await _regionRepository.UpdateAsync(id, regionDomain);

            if (regionDomain == null)
            {
                return NotFound();
            }

            var responseDTO = _mapper.Map<RegionResponseDTO>(regionDomain);

            return Ok(responseDTO);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
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