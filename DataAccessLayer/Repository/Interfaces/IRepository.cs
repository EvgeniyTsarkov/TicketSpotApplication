using Common.Models;
using System.Linq.Expressions;

namespace DataAccessLayer.Repository.Interfaces;

public interface IRepository<TEntity> where TEntity : class, IEntity
{
    Task<List<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes);
    Task<TEntity> GetAsync(int id, params Expression<Func<TEntity, object>>[] includes);
    Task<List<TEntity>> GetAllByConditionAsync(
        Expression<Func<TEntity, bool>> expression,
        params Expression<Func<TEntity, object>>[] includes);
    Task<TEntity> GetByConditionAsync(
        Expression<Func<TEntity, bool>> expression,
        params Expression<Func<TEntity, object>>[] includes);
    Task<TEntity> CreateAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task DeleteAsync(int id);
}
