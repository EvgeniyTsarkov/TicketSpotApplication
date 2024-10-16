namespace DataAccessLayer.Repository.Interfaces;

public interface IRepository<TEntity>
{
    Task<List<TEntity>> GetAllAsync();
    Task<TEntity> GetAsync(int id);
    Task<TEntity> CreateAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task DeleteAsync(int id);
}
