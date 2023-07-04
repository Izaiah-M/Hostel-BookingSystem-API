using System.ComponentModel.DataAnnotations;

namespace HostME.Core.DTOs
{
    public class RoomDTO
    {
        [Required]
        public int? HostelId { get; set; }

        [Required]
        [StringLength(255)]
        public string? RoomType { get; set; }

        [Required]
        public int? Capacity { get; set; }

        [Required]
        public decimal? PricePerSemester { get; set; }

        [Required]
        [StringLength(255)]
        public string? RoomStatus { get; set; }

    }

    public class AllRoomsDTO
    {
        [Required]
        public int? HostelId { get; set; }
    }

    public class DeleteRoomDTO : AllRoomsDTO
    {
        public int Id { get; set; }
    }

    public class UpdateRoomDTO : RoomDTO
    {
        public int Id { get; set; }
    }

    public class GetRoomDTO : RoomDTO
    {
        public int Id { get; set; }
    }
}
