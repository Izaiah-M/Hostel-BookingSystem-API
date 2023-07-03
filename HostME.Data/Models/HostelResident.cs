using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HostME.Data.Models
{
    public class HostelResident
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 

        public int RoomId { get; set; }

        public int ResidentId { get; set; }

        [ForeignKey("ResidentId")]
        public ApiUser? Resident { get; set; } 
        
        [ForeignKey("RoomId")]
        public Room? Room { get; set; }


    }
}
