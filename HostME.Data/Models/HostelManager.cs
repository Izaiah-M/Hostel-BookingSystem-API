using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HostME.Data.Models
{
    public class HostelManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ManagerId { get; set; }

        public int HostelId { get; set; }

        [ForeignKey("ManagerId")]
        public ApiUser? Manager { get; set; }

        [ForeignKey("HostelId")]
        public Hostel? Hostel { get; set; }
    }
}
