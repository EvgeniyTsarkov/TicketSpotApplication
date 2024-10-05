using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
            await _dbSet.AsNoTracking().ToListAsync();

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> expression) =>
            await _dbSet.AsNoTracking()
            .AsQueryable()
            .Where(expression)
            .SingleOrDefaultAsync();
    }
}
