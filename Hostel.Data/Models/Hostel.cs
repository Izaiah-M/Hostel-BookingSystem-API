using System;
using System.Collections.Generic;

namespace Hostel.Data.Models;

public partial class Hostel
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public int? NoOfRooms { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
