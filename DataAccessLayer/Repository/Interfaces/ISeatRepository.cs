using Common.Models;
using DataAccessLayer.Repository.Interfaces;

namespace DataAccessLayer.Repository.Interfaces;

public interface ISeatRepository
{
    Task<List<Seat>> GetAllAsync();
    Task<Seat> GetAsync(int id);
    Task<Seat> CreateAsync(Seat seat);
    Task<Seat> UpdateAsync(Seat seat);
    Task DeleteAsync(int id);
}
