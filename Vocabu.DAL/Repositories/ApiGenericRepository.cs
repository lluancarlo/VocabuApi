using Vocabu.DAL.Contexts;
using Vocabu.Domain.Entities;
using Vocabu.Domain.Interfaces;

namespace Vocabu.DAL.Repositories;

public class ApiGenericRepository<T> : Repository<ApiDbContext, T>, IRepository<T> where T : BaseEntity
{
    public ApiGenericRepository(ApiDbContext context) : base(context) { }
}
