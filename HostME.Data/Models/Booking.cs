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

        // To add a foreignkey, it has to be in the format below
        // Name of the foreignKey
        // Table from which it comes from.
        [ForeignKey(nameof(Hostel))]
        public int HostelId { get; set; }
        public virtual Hostel? Hostel { get; set; }

        [ForeignKey(nameof(Room))]
        public int RoomId { get; set; }
        public virtual Room? Room { get; set; }


        [ForeignKey(nameof(ApiUser))]
        public int UserId { get; set; }
        public virtual ApiUser? Customer { get; set; }

    }
}
