﻿using RouteWise.Domain.Entities;
using System.Linq.Expressions;

namespace RouteWise.Data.IRepositories;

public interface IRepository<TEntity> where TEntity : Auditable
{
    Task CreateAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    void Destroy(TEntity entity);
    Task<TEntity?> SelectAsync(int id, bool asNoTracking = false, string[]? includes = null);
    Task<TEntity?> SelectAsync(Expression<Func<TEntity, bool>> expression, bool asNoTracking = false, string[]? includes = null);
    IQueryable<TEntity> SelectAll(Expression<Func<TEntity, bool>>? expression = null, bool asNoTracking = true, string[]? includes = null);
    Task<int?> DestroyAllAsync(Expression<Func<TEntity, bool>>? expression = null, CancellationToken cancellationToken = default);
}
