using HostME.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace HostME.Core.DTOs
{
    public class BookingDTO
    {

        [Required]
        public DateTime SemesterStartDate { get; set; }

        [Required]
        public DateTime SemesterEndDate { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public int HostelId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public int UserId { get; set; }
    }

    public class AllBookingsDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime SemesterStartDate { get; set; }

        [Required]
        public DateTime SemesterEndDate { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public BookHostelDTO? Hostel { get; set; }

        [Required]
        public GetRoomDTO? Room { get; set; }

        [Required]
        public OneUserDTO? Customer { get; set; }
    }
}
