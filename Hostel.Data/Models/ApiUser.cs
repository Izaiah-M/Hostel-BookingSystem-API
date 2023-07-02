using Microsoft.AspNetCore.Identity;

namespace Hostel.Data.Models
{
    public class ApiUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int HostelId { get; set; }
        public int RoomId { get; set; }
    }
}
