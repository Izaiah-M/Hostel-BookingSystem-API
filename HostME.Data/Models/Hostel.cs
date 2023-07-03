namespace HostME.Data.Models;

public  class Hostel
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public int? NoOfRooms { get; set; }

    // Navigation Property
    public virtual HostelManager? Manager { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}


