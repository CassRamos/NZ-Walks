using NZWalk.API.Models.Domain;
using NZWalk.API.Models.DTO;

namespace NZWalk.API.Repositories.Interfaces
{
    public interface IWalkRepository
    {
        Task<List<Walk>> GetAllAsync();
        Task<Walk?> GetByIdAsync(Guid walkId);
        Task<Walk> CreateAsync(Walk walk);
        Task<Walk?> UpdateAsync(Guid walkId, Walk walk);
        Task<Walk?> DeleteAsync(Guid walkId);
    }
}
