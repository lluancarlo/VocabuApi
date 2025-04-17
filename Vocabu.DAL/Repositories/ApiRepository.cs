using Vocabu.DAL.Contexts;
using Vocabu.Domain.Entities;
using Vocabu.Domain.Interfaces;

namespace Vocabu.DAL.Repositories;

public class ApiRepository<T> : Repository<ApiDbContext, T>, IRepository<T> where T : BaseEntity
{
    public ApiRepository(ApiDbContext context) : base(context) { }
}
