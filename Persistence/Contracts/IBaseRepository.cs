namespace Persistence.Contracts;

public interface IBaseRepository<T> where T : class
{
    Task<T> AddAsync(T entity);
    Task<T> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync();
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}