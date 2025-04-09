namespace Vocabu.Domain.Interfaces;

public interface IRepository<TEntity> 
    where TEntity : class
{
    IQueryable<TEntity> AsQueryable();
    Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = true);
    Task<TEntity?> GetByIdAsync(Guid id, bool tracking = true);
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task<bool> SaveChangesAsync();
}
