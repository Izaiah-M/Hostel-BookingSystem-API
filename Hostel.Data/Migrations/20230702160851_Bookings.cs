using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hostel.Data.Migrations
{
    /// <inheritdoc />
    public partial class Bookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1089e040-373a-428a-9f19-57f2fd745060");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db1312fb-a4bb-4dc7-b000-14cc13791ce2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ee2ed9b3-f124-4630-bf36-48c0ef4d514d");

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    HostelId = table.Column<int>(type: "int", nullable: false),
                    SemesterStartDate = table.Column<DateTime>(type: "Date", nullable: false),
                    SemesterEndDate = table.Column<DateTime>(type: "Date", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "AccessLevel", "ConcurrencyStamp", "Description", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "78b6a711-69fc-4591-8c8d-656f9f2366c5", "/[\"manager dashboard/\"]", null, "Hostel manager role", "ApiRoles", "Hostel Manager", "HOSTEL MANAGER" },
                    { "c95c6c09-c2ac-4042-9c99-6182cf28ed8c", "/[\"admin dashboard/\", \"manager dashboard\", \"user dashboard\"]", null, "Super Admin role", "ApiRoles", "Super Administrator", "SUPER ADMINISTRATOR" },
                    { "f3311253-b1ed-461d-ab78-5ef73a7a687b", "/[\"user dashboard/\"]", null, "customer role", "ApiRoles", "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CustomerId",
                table: "Bookings",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78b6a711-69fc-4591-8c8d-656f9f2366c5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c95c6c09-c2ac-4042-9c99-6182cf28ed8c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f3311253-b1ed-461d-ab78-5ef73a7a687b");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "AccessLevel", "ConcurrencyStamp", "Description", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1089e040-373a-428a-9f19-57f2fd745060", "/[\"manager dashboard/\"]", null, "Hostel manager role", "ApiRoles", "Hostel Manager", "HOSTEL MANAGER" },
                    { "db1312fb-a4bb-4dc7-b000-14cc13791ce2", "/[\"admin dashboard/\", \"manager dashboard\", \"user dashboard\"]", null, "Super Admin role", "ApiRoles", "Super Administrator", "SUPER ADMINISTRATOR" },
                    { "ee2ed9b3-f124-4630-bf36-48c0ef4d514d", "/[\"user dashboard/\"]", null, "customer role", "ApiRoles", "User", "USER" }
                });
        }
    }
}
