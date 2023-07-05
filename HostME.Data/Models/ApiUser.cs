using Microsoft.AspNetCore.Identity;

namespace HostME.Data.Models
{
    public class ApiUser : IdentityUser<int>
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? FirstLogin { get; set; }
    }
}
