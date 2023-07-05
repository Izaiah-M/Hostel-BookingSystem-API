using System.ComponentModel.DataAnnotations;

namespace HostME.Core.DTOs
{
    public class HostelResidentDTO
    {

        [Required]
        public int RoomId { get; set; }

        [Required]
        public int ResidentId { get; set; }

    }
}
