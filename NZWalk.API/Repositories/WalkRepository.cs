using Microsoft.EntityFrameworkCore;
using NZWalk.API.Data;
using NZWalk.API.Models.Domain;
using NZWalk.API.Models.DTO;
using NZWalk.API.Repositories.Interfaces;

namespace NZWalk.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _nZWalksDbContext;

        public WalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            _nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<List<Walk>> GetAllAsync()
        {
            return await _nZWalksDbContext.Walks
                .Include(w => w.Difficulty)
                .Include(w => w.Region)
                .ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid walkId)
        {
            return await _nZWalksDbContext.Walks
                .Include(w => w.Difficulty)
                .Include(w => w.Region)
                .FirstOrDefaultAsync(w => w.Id == walkId);
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {

            await _nZWalksDbContext.AddAsync(walk);
            await _nZWalksDbContext.SaveChangesAsync();

            return walk;
        }

        public async Task<Walk?> UpdateAsync(Guid walkId, Walk walk)
        {
            Walk? existingWalk = await _nZWalksDbContext.Walks
                .FirstOrDefaultAsync(w => w.Id == walkId);

            if (existingWalk == null)
            {
                return null;
            }

            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.RegionId = walk.RegionId;

            await _nZWalksDbContext.SaveChangesAsync();

            return existingWalk;
        }

        public async Task<Walk?> DeleteAsync(Guid walkId)
        {
            Walk? exisitingWalk = await _nZWalksDbContext.Walks.FirstOrDefaultAsync(w => w.Id == walkId);

            if (exisitingWalk == null)
            {
                return null;
            }

            _nZWalksDbContext.Remove(exisitingWalk);
            await _nZWalksDbContext.SaveChangesAsync();

            return exisitingWalk;

        }
    }
}
