using System.ComponentModel.DataAnnotations;

namespace HostME.Core.DTOs
{
    public class ManagerDTO
    {
        [Required]
        public int HostelId { get; set; }

        [Required]
        public int ManagerId { get; set; }
    }
}
