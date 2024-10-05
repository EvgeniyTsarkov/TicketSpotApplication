using System.Linq.Expressions;

namespace DataAccessLayer.Repository.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<List<TEntity>> GetAllAsync();

    Task<TEntity> Get(Expression<Func<TEntity, bool>> expression);
}
