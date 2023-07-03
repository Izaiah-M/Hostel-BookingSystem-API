using System.ComponentModel.DataAnnotations.Schema;

namespace HostME.Data.Models
{
    public class HostelManager
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Hostel))]
        public int HostelId { get; set; }
        public virtual Hostel? Hostel { get; set; }


        [ForeignKey(nameof(ApiUser))]
        public int ManagerId { get; set; }
        public virtual ApiUser? Manager { get; set; }
    }
}
