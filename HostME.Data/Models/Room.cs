﻿using System.ComponentModel.DataAnnotations.Schema;

namespace HostME.Data.Models
{

    public partial class Room
    {
        public int Id { get; set; }

        public int? HostelId { get; set; }

        public string? RoomType { get; set; }

        public int? Capacity { get; set; }


        public double? PricePerSemester { get; set; }

        public string? RoomStatus { get; set; }

        [ForeignKey("HostelId")]
        public Hostel? Hostel { get; set; }
    }

}

