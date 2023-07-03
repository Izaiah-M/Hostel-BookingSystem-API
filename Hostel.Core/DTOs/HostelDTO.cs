using System.ComponentModel.DataAnnotations;

namespace HostME.Core.DTOs
{
    public class HostelDTO
    {

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        public int? NoOfRooms { get; set; }


    }

    public class GetHostelDTO : HostelDTO
    {
        [Required]
        public int Id { get; set; }

        public IList<RoomDTO>? Rooms { get; set; }
    }

}
