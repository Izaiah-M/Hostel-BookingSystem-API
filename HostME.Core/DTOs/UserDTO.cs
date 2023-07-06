using System.ComponentModel.DataAnnotations;

namespace HostME.Core.DTOs
{
    public class UserManagerDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int RoleId { get; set; }
    }

    public class LoginDTO
    {
        [Required]
        [StringLength(255)]
        public required string Email { get; set; }

        [Required]
        [StringLength(15)]
        public required string Password { get; set; }
    }

    public class OneUserDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public required string LastName { get; set; }

        [Required]
        public required string PhoneNumber { get; set; }

        [Required]
        [StringLength(255)]
        public required string Email { get; set; }

        public ICollection<string>? Roles { get; set; }

        public GetRoomDTO? Room { get; set; }
    }

    public class UserDTO : LoginDTO
    {
        [Required]
        [StringLength(100)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public required string LastName { get; set; }

        [Required]
        public required string PhoneNumber { get; set; }

    }
}
