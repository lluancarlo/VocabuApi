using Microsoft.AspNetCore.Identity;

namespace Vocabu.DAL.Entities;

public class User : IdentityUser<Guid>
{
    public required string Name { get; set; }

    public Guid CountryId { get; set; }
    public virtual Country? Country { get; set; }
}