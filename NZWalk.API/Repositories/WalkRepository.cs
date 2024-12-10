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

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null,
                                                  bool isAscending = true, int pageNumber = 1, int pageSize = 100)
        {
            var walks = _nZWalksDbContext.Walks
                .Include(w => w.Difficulty)
                .Include(w => w.Region)
                .AsQueryable();

            // filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(w => w.Name.Contains(filterQuery));
                }
            }

            // sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(w => w.Name) : walks.OrderByDescending(w => w.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(w => w.LengthInKm);
                }
            }

            // pagination
            int skipResults = (pageNumber - 1) * pageSize;

            return await walks
                .Skip(skipResults)
                .Take(pageSize)
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
