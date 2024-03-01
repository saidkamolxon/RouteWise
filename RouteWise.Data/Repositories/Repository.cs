using RouteWise.Data.IRepositories;
using RouteWise.Domain.Entities;
using System.Linq.Expressions;

namespace RouteWise.Data.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : Auditable
{
    public Task CreateAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public void Destroy(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public IQueryable<TEntity> SelectAll(Expression<Func<TEntity, bool>>? expression = null, bool asNoTracked = true, string[]? includes = null)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> SelectAsync(long id, string[]? includes = null)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> SelectAsync(Expression<Func<TEntity, bool>> expression, string[]? includes = null)
    {
        throw new NotImplementedException();
    }

    public void Update(TEntity entity)
    {
        throw new NotImplementedException();
    }
}
