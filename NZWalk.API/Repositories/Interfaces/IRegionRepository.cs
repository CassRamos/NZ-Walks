using NZWalk.API.Models.Domain;

namespace NZWalk.API.Repositories.Interfaces
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();
        Task<Region?> GetByIdAsync(Guid id);
        Task<Region> CreateAsync(Region requestDTO);
        Task<Region?> UpdateAsync(Guid id, Region updateDTO);
        Task<Region?> DeleteAsync(Guid id);
    }
}
