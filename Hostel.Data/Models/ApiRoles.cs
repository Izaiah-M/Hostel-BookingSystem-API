using Microsoft.AspNetCore.Identity;

namespace Hostel.Data.Models
{
    public class ApiRoles : IdentityRole
    {
        public string Description { get; set; } = null!;

        public string AccessLevel { get; set; } = null!;
    }
}
