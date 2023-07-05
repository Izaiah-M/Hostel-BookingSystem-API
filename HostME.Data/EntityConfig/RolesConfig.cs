using HostME.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HostME.Data.EntityConfig
{
    public class RolesConfig : IEntityTypeConfiguration<ApiRoles>
    {
        public void Configure(EntityTypeBuilder<ApiRoles> builder)
        {
            builder.HasData(
                new ApiRoles
                {
                    Id = 1,
                    Name = "Super Administrator",
                    NormalizedName = "SUPER ADMINISTRATOR",
                    Description = "Super Admin role",
                    AccessLevel = "/[\"admin dashboard/\", \"manager dashboard\", \"user dashboard\"]"
                },
                new ApiRoles
                {
                    Id = 2,
                    Name = "Hostel Manager",
                    NormalizedName = "HOSTEL MANAGER",
                    Description = "Hostel manager role",
                    AccessLevel = "/[\"manager dashboard/\"]"
                },
                new ApiRoles
                {
                    Id = 3,
                    Name = "Resident",
                    NormalizedName = "RESIDENT",
                    Description = "resident role",
                    AccessLevel = "/[\"resident dashboard/\"]"
                },
                new ApiRoles
                {
                    Id = 4,
                    Name = "Default",
                    NormalizedName = "DEFAULT",
                    Description = "default role",
                    AccessLevel = "/[\"user dashboard/\"]"
                }

                );
        }
    }
}
