using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HostME.Data.Models;

public  class Hostel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public int? NoOfRooms { get; set; }

    // Navigation Property
    public virtual IList<Room>? Rooms { get; set; }

    public HostelManager? HostelManager { get; set; }
}


