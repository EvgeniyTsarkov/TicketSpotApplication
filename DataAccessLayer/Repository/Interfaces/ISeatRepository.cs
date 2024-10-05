using Common.Models;
using DataAccessLayer.Repository.Interfaces;

namespace DataAccessLayer.Repository.Interfaces;

public interface ISeatRepository : IBaseRepository<Seat>
{
    Task<Seat> CreateAsync(Seat seat);
    Task<Seat> UpdateAsync(Seat seat);
    Task DeleteAsync(int id);
}
