namespace Vocabu.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    IQueryable<T> AsQueryable();
    Task<IEnumerable<T>> GetAllAsync(bool tracking = true);
    Task<T?> GetByIdAsync(Guid id, bool tracking = true);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<bool> SaveChangesAsync();
}
