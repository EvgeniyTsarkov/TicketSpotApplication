using Common.Models;

namespace DataAccessLayer.Repository.Interfaces
{
    public interface IRowRepository
    {        
        Task<List<Row>> GetAllAsync();
        Task<Row> GetAsync(int id);
        Task<Row> CreateAsync(Row rowToCreate);
        Task<Row> UpdateAsync(Row updatedRow);
        Task DeleteAsync(int id);
    }
}
