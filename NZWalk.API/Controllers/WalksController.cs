using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalk.API.CustomActionsFilter;
using NZWalk.API.Models.Domain;
using NZWalk.API.Models.DTO;
using NZWalk.API.Repositories.Interfaces;

namespace NZWalk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            _mapper = mapper;
            _walkRepository = walkRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalks()
        {
            List<Walk> walksDomain = await _walkRepository.GetAllAsync();

            return Ok(_mapper.Map<List<WalkResponseDTO>>(walksDomain));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetWalkById([FromRoute] Guid id)
        {
            Walk? walkDomain = await _walkRepository.GetByIdAsync(id);

            if (walkDomain == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WalkResponseDTO>(walkDomain));
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateWalk([FromBody] CreateWalkRequestDTO requestDTO)
        {
            Walk walkDomain = _mapper.Map<Walk>(requestDTO);

            await _walkRepository.CreateAsync(walkDomain);

            WalkResponseDTO walkDTO = _mapper.Map<WalkResponseDTO>(walkDomain);

            return Ok(walkDTO);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateWalk([FromRoute] Guid id, [FromBody] UpdateWalkRequestDTO requestDTO)
        {
            Walk? walkDomain = _mapper.Map<Walk>(requestDTO);

            walkDomain = await _walkRepository.UpdateAsync(id, walkDomain);

            if (walkDomain == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WalkResponseDTO>(walkDomain));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteWalk([FromRoute] Guid id)
        {
            Walk? walkDomain = await _walkRepository.DeleteAsync(id);

            if (walkDomain == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WalkResponseDTO>(walkDomain));
        }
    }
}