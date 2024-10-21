using Common.Models;
using System.Linq.Expressions;

namespace DataAccessLayer.Repository.Interfaces;

public interface IEventRepository
{
    Task<List<Event>> GetAllAsync();
    Task<Event> GetAsync(int id);
    Task<List<Event>> GetAllByConditionAsync(
        Expression<Func<Event, bool>> expression,
        params Expression<Func<Event, object>>[] includes);
    Task<Event> CreateAsync(Event entity);
    Task<Event> UpdateAsync(Event entity);
    Task DeleteAsync(int id);
}
