using System.ComponentModel.DataAnnotations.Schema;

namespace HostME.Data.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Column(TypeName = "Date")]
        public DateTime SemesterStartDate { get; set; }

        [Column(TypeName = "Date")]
        public DateTime SemesterEndDate { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalPrice { get; set; }

        public int HostelId { get; set; }

        public int RoomId { get; set; }


        public int UserId { get; set; }
        
        [ForeignKey("HostelId")]
        public virtual Hostel? Hostel { get; set; }

        [ForeignKey("RoomId")]
        public virtual Room? Room { get; set; }


        [ForeignKey("UserId")]
        public virtual ApiUser? Customer { get; set; }

    }
}
