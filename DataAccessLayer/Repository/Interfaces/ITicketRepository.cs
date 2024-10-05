using Common.Models;

namespace DataAccessLayer.Repository.Interfaces
{
    public interface ITicketRepository : IBaseRepository<Ticket>
    {
        Task<Ticket> CreateAsync(Ticket ticket);
        Task<Ticket> UpdateAsync(Ticket ticket);
        Task DeleteAsync(int id);
    }
}
