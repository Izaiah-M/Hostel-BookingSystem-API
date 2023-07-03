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
                    Name = "Super Administrator",
                    NormalizedName = "SUPER ADMINISTRATOR",
                    Description = "Super Admin role",
                    AccessLevel = "/[\"admin dashboard/\", \"manager dashboard\", \"user dashboard\"]"
                },
                new ApiRoles
                {
                    Name = "Hostel Manager",
                    NormalizedName = "HOSTEL MANAGER",
                    Description = "Hostel manager role",
                    AccessLevel = "/[\"manager dashboard/\"]"
                },
                new ApiRoles
                {
                    Name = "User",
                    NormalizedName = "USER",
                    Description = "customer role",
                    AccessLevel = "/[\"user dashboard/\"]"
                }

                );
        }
    }
}
