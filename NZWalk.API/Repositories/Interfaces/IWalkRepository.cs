using NZWalk.API.Models.Domain;

namespace NZWalk.API.Repositories.Interfaces
{
    public interface IWalkRepository
    {
        Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null,
                                     bool isAscending = true, int pageNumber = 1, int pageSize = 100);
        Task<Walk?> GetByIdAsync(Guid walkId);
        Task<Walk> CreateAsync(Walk walk);
        Task<Walk?> UpdateAsync(Guid walkId, Walk walk);
        Task<Walk?> DeleteAsync(Guid walkId);
    }
}
