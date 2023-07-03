using HostME.Data.EntityConfig;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HostME.Data.Models;

public partial class HostMeContext : IdentityDbContext<ApiUser>
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

        // Adding the role Configuration
        modelBuilder.ApplyConfiguration(new RolesConfig());

        modelBuilder.Entity<Hostel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Hostels__3214EC0763BFA788");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rooms__3214EC07E7EB0CE7");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.PricePerSemester)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Price_per_semester");
            entity.Property(e => e.RoomStatus)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Room_status");
            entity.Property(e => e.RoomType)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Hostel).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.HostelId)
                .HasConstraintName("FK__Rooms__HostelId__4BAC3F29");
        });

        OnModelCreatingPartial(modelBuilder);

        modelBuilder.Ignore<Room>();
        modelBuilder.Ignore<Hostel>();
        modelBuilder.Ignore<IdentityRole>();
        modelBuilder.Ignore<IdentityUser>();
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
