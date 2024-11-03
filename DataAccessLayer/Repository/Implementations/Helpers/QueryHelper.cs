using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccessLayer.Repository.Implementations.Helpers;

public static class QueryHelper<TEntity> where TEntity : class, new()
{
    public static IQueryable<TEntity> IncludeMultiple<T>(
    IQueryable<TEntity> query,
    params Expression<Func<TEntity, object>>[] includes)
    where T : class
    {
        if (includes != null)
        {
            query = includes.Aggregate(query,
                      (current, include) => current.Include(include));
        }

        return query;
    }
}
