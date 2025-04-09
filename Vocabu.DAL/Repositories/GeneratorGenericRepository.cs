using Vocabu.DAL.Contexts;
using Vocabu.Domain.Entities;
using Vocabu.Domain.Interfaces;

namespace Vocabu.DAL.Repositories;

public class GeneratorGenericRepository<T> : Repository<GeneratorDbContext, T>, IRepository<T> where T : BaseEntity
{
    public GeneratorGenericRepository(GeneratorDbContext context) : base(context) { }
}
