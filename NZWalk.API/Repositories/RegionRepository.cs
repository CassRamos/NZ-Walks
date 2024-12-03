using Microsoft.EntityFrameworkCore;
using NZWalk.API.Data;
using NZWalk.API.Models.Domain;
using NZWalk.API.Models.DTO;
using NZWalk.API.Repositories.Interfaces;

namespace NZWalk.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _nZWalksDbContext;

        public RegionRepository(NZWalksDbContext nZWalksDbContext)
        {
            _nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await _nZWalksDbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await _nZWalksDbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Region> CreateAsync(Region requestDTO)
        {
            await _nZWalksDbContext.AddAsync(requestDTO);
            await _nZWalksDbContext.SaveChangesAsync();

            return requestDTO;
        }

        public async Task<Region?> UpdateAsync(Guid id, Region updateDTO)
        {
            var existingRegion = await _nZWalksDbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);

            if (existingRegion == null)
            {
                return null;
            }

            existingRegion.Code = updateDTO.Code;
            existingRegion.Name = updateDTO.Name;
            existingRegion.RegionImageUrl = updateDTO.RegionImageUrl;

            await _nZWalksDbContext.SaveChangesAsync();
            return existingRegion;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var existingRegion = await _nZWalksDbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);

            if (existingRegion == null)
            {
                return null;
            }

            _nZWalksDbContext.Remove(existingRegion);
            await _nZWalksDbContext.SaveChangesAsync();
            return existingRegion;
        }
    }
}
