using Microsoft.EntityFrameworkCore;
using RouteWise.Data.Contexts;
using RouteWise.Data.IRepositories;
using RouteWise.Domain.Entities;
using System.Linq.Expressions;

namespace RouteWise.Data.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : Auditable
{
    private readonly AppDbContext _appDbContext;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
        _dbSet = appDbContext.Set<TEntity>();
    }

    public async Task CreateAsync(TEntity entity)
        => await _dbSet.AddAsync(entity);

    public void Delete(TEntity entity)
        => entity.IsDeleted = true;

    public void Destroy(TEntity entity)
        => _appDbContext.Entry(entity).State = EntityState.Deleted;

    public void Update(TEntity entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _appDbContext.Entry(entity).State = EntityState.Modified;
    }

    public async Task<TEntity?> SelectAsync(Expression<Func<TEntity, bool>> expression,
        bool asNoTracking = false, string[]? includes = null)
    {
        var query = _dbSet.Where(e => !e.IsDeleted);

        if (asNoTracking) query = query.AsNoTracking();

        if (includes is not null) query = includes.Aggregate(query, (current, include) => current.Include(include));

        return await query.FirstOrDefaultAsync(expression);
    }

    public async Task<TEntity?> SelectAsync(int id, bool asNoTracking = false, string[]? includes = null)
        => await this.SelectAsync(e => e.Id.Equals(id), asNoTracking, includes);

    public IQueryable<TEntity> SelectAll(Expression<Func<TEntity, bool>>? expression = null, bool asNoTracking = true, string[]? includes = null)
    {
        IQueryable<TEntity> query = _dbSet;

        if (expression is not null)
            query = query.Where(expression);

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includes is not null)
            query = includes.Aggregate(query, (current, include) => current.Include(include));

        return query.Where(e => !e.IsDeleted);
    }

    public async Task<int?> DestroyAllAsync(Expression<Func<TEntity, bool>>? expression = null, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet;

        if (expression is not null)
            query.Where(expression);

        return await query.ExecuteDeleteAsync(cancellationToken);
    }
}
