using Basic_Authentication.Contracts;
using Basic_Authentication.Data;

using Microsoft.EntityFrameworkCore;

namespace Basic_Authentication.Repositories;

public class BaseRepository<T>(BloggingContext dbContext) : IBaseRepository<T> where T : class
{
    protected readonly BloggingContext _dbContext = dbContext;

    public async Task<T> GetByIdAsync(int id)
    {
        var entity = await _dbContext.Set<T>().FindAsync(id);
        if (entity is not null) return entity;
        else throw new NullReferenceException();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _dbContext.Set<T>().FindAsync(id);
        _dbContext.Set<T>().Remove(entity!);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<T>> GetAllAsync()
        => await _dbContext.Set<T>().AsNoTracking().ToListAsync();
}