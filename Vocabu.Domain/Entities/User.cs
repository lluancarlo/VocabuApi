using Microsoft.AspNetCore.Identity;
using Vocabu.Domain.Entities;

namespace Vocabu.DAL.Entities;

public class User : IdentityUser<Guid>
{
    public int CountryId { get; set; }
    public virtual Country? Country { get; set; }

    public required string Name { get; set; }
    public int Level { get; set; }
    public int Experience { get; set; }
    public int Gold { get; set; }
}