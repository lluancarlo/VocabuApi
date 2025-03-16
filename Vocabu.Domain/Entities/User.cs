using Microsoft.AspNetCore.Identity;

namespace Vocabu.DAL.Entities;

public class User : IdentityUser<Guid>
{
    //public Guid Id { get; set; }
    //public required string Email { get; set; }
    //public required string Password { get; set; }
    public required string Name { get; set; }

    public Guid? CountryId { get; set; }
    public virtual Country? Country { get; set; }
}