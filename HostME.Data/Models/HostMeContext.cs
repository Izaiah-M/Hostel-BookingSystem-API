using HostME.Data.EntityConfig;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HostME.Data.Models;

public partial class HostMeContext : IdentityDbContext<ApiUser, ApiRoles, int>
{
    public HostMeContext()
    {
    }

    public HostMeContext(DbContextOptions<HostMeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Hostel> Hostels { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<HostelManager> HostelManagers { get; set; }

    public virtual DbSet<HostelResident> HostelResidents { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

        // Adding the role Configuration
        modelBuilder.ApplyConfiguration(new RolesConfig());

    }
}
