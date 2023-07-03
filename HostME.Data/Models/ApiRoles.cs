using Microsoft.AspNetCore.Identity;

namespace HostME.Data.Models
{
    public class ApiRoles : IdentityRole<int>
    {
        public string Description { get; set; } = null!;

        public string AccessLevel { get; set; } = null!;
    }
}
