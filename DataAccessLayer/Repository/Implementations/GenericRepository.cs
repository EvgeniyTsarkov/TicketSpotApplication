﻿using Common.Models;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccessLayer.Repository.Implementations;

public class GenericRepository<TEntity>(TicketSpotDbContext context)
    : IRepository<TEntity> where TEntity : class, IEntity, new()
{
    protected TicketSpotDbContext _context = context;
    protected DbSet<TEntity> _entities = context.Set<TEntity>();

    // For Ticket include Customer, Seat, Event
    // For Venue include EventManager 
    public async Task<List<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes)
    {
        var query = _entities.AsNoTracking();

        if (includes.Length != 0)
        {
            query = IncludeMultiple<TEntity>(query, includes);
        }

        return await query.ToListAsync();
    }

    // For Ticket include Customer, Seat, Event
    // For Venue include EventManager 
    public async Task<TEntity> GetAsync(int id, params Expression<Func<TEntity, object>>[] includes)
    {
        var query = _entities.AsNoTracking();

        if (includes.Length != 0)
        {
            query = IncludeMultiple<TEntity>(query, includes);
        }

        return await query.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        await _entities.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        var itemToUpdate = await GetAsync(entity.Id)
            ?? throw new RecordNotFoundException("The entity to be updated is not found in the database");

        _entities.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var itemToDelete = new TEntity { Id = id };
        _entities.Attach(itemToDelete);
        _entities.Remove(itemToDelete);
        await _context.SaveChangesAsync();
    }

    private static IQueryable<TEntity> IncludeMultiple<T>(
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