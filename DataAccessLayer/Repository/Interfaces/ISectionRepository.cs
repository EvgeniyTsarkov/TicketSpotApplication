using Common.Models;

namespace DataAccessLayer.Repository.Interfaces
{
    public interface ISectionRepository
    {
        Task<List<Section>> GetAllAsync();
        Task<Section> GetAsync(int id);
        Task<Section> CreateAsync(Section sectionToCreate);
        Task<Section> UpdateAsync(Section updatedSection);
        Task DeleteAsync(int id);
    }
}
