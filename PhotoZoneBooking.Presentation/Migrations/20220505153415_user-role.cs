using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PhotoZoneBooking.Presentation.Migrations
{
    /// <inheritdoc />
    public partial class userrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0906d4b5-8132-4e7e-b135-2025ffaa3746", "0906d4b5-8132-4e7e-b135-2025ffaa3746", "admin", "ADMIN" },
                    { "651cd387-0f79-41a6-84aa-6fe5c57d9b35", "651cd387-0f79-41a6-84aa-6fe5c57d9b35", "user", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "e3333d0a-afd5-4b63-af81-076dd1bbb7ce", 0, "b55777b2-aa54-443f-b5c3-89190c88359c", "admin@gmail.com", true, "admin", "admin", false, null, null, "ADMIN@GMAIL.COM", "AQAAAAEAACcQAAAAEN20YnQa/Dv+25O/F2SHeh5SmhhZ13HI0MdYLYUwQ57wdXIacObpEw8WAekvMZiVHA==", null, false, "d8a2d177-db2e-4259-9dc2-889cdafd44e9", false, "admin@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "0906d4b5-8132-4e7e-b135-2025ffaa3746", "e3333d0a-afd5-4b63-af81-076dd1bbb7ce" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "651cd387-0f79-41a6-84aa-6fe5c57d9b35");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "0906d4b5-8132-4e7e-b135-2025ffaa3746", "e3333d0a-afd5-4b63-af81-076dd1bbb7ce" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0906d4b5-8132-4e7e-b135-2025ffaa3746");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e3333d0a-afd5-4b63-af81-076dd1bbb7ce");
        }
    }
}
