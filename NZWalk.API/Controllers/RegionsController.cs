using Microsoft.AspNetCore.Mvc;
using NZWalk.API.Data;
using NZWalk.API.Models.Domain;
using NZWalk.API.Models.DTO;

namespace NZWalk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _context;

        public RegionsController(NZWalksDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllRegions()
        {
            var regionsDomain = _context.Regions.ToList();

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
        public IActionResult GetRegionById([FromRoute] Guid id)
        {
            var regionDomain = _context.Regions.FirstOrDefault(r => r.Id == id);

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
        public IActionResult CreateRegion([FromBody] CreateRegionRequestDTO requestDTO)
        {
            var regionDomain = new Region
            {
                Code = requestDTO.Code,
                Name = requestDTO.Name,
                RegionImageUrl = requestDTO.RegionImageUrl,
            };

            _context.Regions.Add(regionDomain);
            _context.SaveChanges();

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
        public IActionResult UpdateRegion(Guid id, [FromBody] UpdateRegionRequestDTO updateRequestDTO)
        {
            var regionDomain = _context.Regions.FirstOrDefault(r => r.Id == id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            regionDomain.Code = updateRequestDTO.Code;
            regionDomain.Name = updateRequestDTO.Name;
            regionDomain.RegionImageUrl = updateRequestDTO.RegionImageUrl;

            _context.SaveChanges();

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
        public IActionResult DeleteRegion(Guid id)
        {
            var regionDomain = _context.Regions.FirstOrDefault(r => r.Id == id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            _context.Regions.Remove(regionDomain);
            _context.SaveChanges();

            return NoContent();
        }
    }
}