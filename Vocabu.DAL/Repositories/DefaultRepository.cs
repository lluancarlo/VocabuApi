using Microsoft.EntityFrameworkCore;
using Vocabu.DAL.Contexts;
using Vocabu.Domain.Interfaces;

namespace Vocabu.DAL.Repositories;

public class DefaultRepository<T> : IRepository<T> where T : class
{
    private readonly DefaultDbContext _context;
    private readonly DbSet<T> _dbSet;

    public DefaultRepository(DefaultDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public IQueryable<T> AsQueryable() => _dbSet.AsQueryable();

    public async Task<IEnumerable<T>> GetAllAsync(bool tracking = true)
    {
        var query = _dbSet.AsQueryable();
        if (tracking)
            query = query.AsNoTracking();

        return await query.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id, bool tracking = true)
    {
        var query = _dbSet.AsQueryable();
        if (tracking)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
    }

    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

    public void Update(T entity) => _dbSet.Update(entity);

    public void Delete(T entity) => _dbSet.Remove(entity);

    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
}
