using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository.Implementations
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly TicketSpotDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(TicketSpotDbContext northwindContext)
        {
            _context = northwindContext;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<List<TEntity>> GetAllAsync() =>
            await _dbSet.ToListAsync();
    }
}

