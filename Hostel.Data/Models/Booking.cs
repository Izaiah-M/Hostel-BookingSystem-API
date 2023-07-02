using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hostel.Data.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int RoomId { get; set; }

        public int HostelId { get; set; }

        [Column(TypeName = "Date")]
        public DateTime SemesterStartDate { get; set; }

        [Column(TypeName = "Date")]
        public DateTime SemesterEndDate { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalPrice { get; set; }

        public virtual Hostel? Hostel { get; set; }
        public virtual Room? Room { get; set; }
        public virtual ApiUser? Customer { get; set; }

    }
}
